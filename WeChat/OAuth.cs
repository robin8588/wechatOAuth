using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using Codeplex.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace WeChat
{
    public class OAuth
    {
        private const string AUTHORIZE_URL = "https://open.weixin.qq.com/connect/oauth2/authorize";
        private const string ACCESS_TOKEN_URL = "https://api.weixin.qq.com/sns/oauth2/access_token";
        private const string REFRESH_TOKEN = "https://api.weixin.qq.com/sns/oauth2/refresh_token";
        private const string PC_AUTHORIZE_URL = "https://open.weixin.qq.com/connect/qrconnect";

        /// <summary>
        /// 获取AppId
        /// </summary>
        public string AppId
        {
            get;
            internal set;
        }
        /// <summary>
        /// 获取App Secret
        /// </summary>
        public string AppSecret
        {
            get;
            internal set;
        }

        /// <summary>
        /// 获取Access Token
        /// </summary>
        public string AccessToken
        {
            get;
            internal set;
        }

        /// <summary>
        /// 获取或设置回调地址
        /// </summary>
        public string RedirectUri
        {
            get;
            set;
        }

        /// <summary>
        /// Refresh Token 似乎目前没用
        /// </summary>
        public string RefreshToken
        {
            get;
            internal set;
        }

        public string OpenId
        {
            get;
            internal set;
        }


        /// <summary>
        /// 实例化OAuth类（用于授权）
        /// </summary>
        /// <param name="appKey">AppKey</param>
        /// <param name="appSecret">AppSecret</param>
        /// <param name="callbackUrl">回调地址</param>
        public OAuth(string appId, string appSecret, string redirectUri = null)
        {
            this.AppId = appId;
            this.AppSecret = appSecret;
            this.AccessToken = string.Empty;
            this.RedirectUri = redirectUri;
            this.OpenId = string.Empty;
        }

        /// <summary>
        /// 实例化OAuth类（用于实例化操作类）
        /// </summary>
        /// <param name="appKey">AppKey</param>
        /// <param name="appSecret">AppSecret</param>
        /// <param name="accessToken">已经获取的AccessToken，若Token没有过期即可通过操作类Client调用接口</param>
        /// <param name="refreshToken">目前还不知道这个参数会不会开放，保留</param>
        public OAuth(string appId, string appSecret, string accessToken,string openId, string refreshToken = null)
        {
            this.AppId = appId;
            this.AppSecret = appSecret;
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken ?? string.Empty;
            this.OpenId = openId;
        }

        internal string Request(string url, RequestMethod method = RequestMethod.Get, bool multi = false, params WeChatParameter[] parameters)
        {
            string rawUrl = string.Empty;
            UriBuilder uri = new UriBuilder(url);
            string result = string.Empty;

            switch (method)
            {
                case RequestMethod.Get:
                    {
                        uri.Query = Utility.BuildQueryString(parameters);
                    }
                    break;
                case RequestMethod.Post:
                    {
                        if (!multi)
                        {
                            uri.Query = Utility.BuildQueryString(parameters);
                        }
                    }
                    break;
            }

            HttpWebRequest http = WebRequest.Create(uri.Uri) as HttpWebRequest;
            http.ServicePoint.Expect100Continue = false;
            http.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0)";

            if (!string.IsNullOrEmpty(AccessToken))
            {
                http.Headers["Authorization"] = string.Format("OAuth2 {0}", AccessToken);
            }

            switch (method)
            {
                case RequestMethod.Get:
                    {
                        http.Method = "GET";
                    }
                    break;
                case RequestMethod.Post:
                    {
                        http.Method = "POST";

                        if (multi)
                        {
                            string boundary = Utility.GetBoundary();
                            http.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
                            http.AllowWriteStreamBuffering = true;
                            using (Stream request = http.GetRequestStream())
                            {
                                try
                                {
                                    var raw = Utility.BuildPostData(boundary, parameters);
                                    request.Write(raw, 0, raw.Length);
                                }
                                finally
                                {
                                    request.Close();
                                }
                            }
                        }
                        else
                        {
                            http.ContentType = "application/x-www-form-urlencoded";

                            using (StreamWriter request = new StreamWriter(http.GetRequestStream()))
                            {
                                try
                                {
                                    request.Write(Utility.BuildQueryString(parameters));
                                }
                                finally
                                {
                                    request.Close();
                                }
                            }
                        }
                    }
                    break;
            }

            try
            {
                using (WebResponse response = http.GetResponse())
                {

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        try
                        {
                            result = reader.ReadToEnd();
                            if (result.Contains("callback"))
                            {
                                result = result.Replace("callback(", "").Replace(");", "");
                            }
                            if (result.Contains("errcode"))
                            {
                                result = result.Replace("\n", "");

                                Error error = JsonConvert.DeserializeObject<Error>(result);

                                throw new WeChatException(string.Format("{0}", error.Code), error.Message);
                            }
                            if (result.Contains("&"))
                            {
                                var trmpr = result.Split('&');
                                result = "{";
                                foreach (var pars in trmpr)
                                {
                                    var par = pars.Split('=');
                                    if (par[0] == "access_token")
                                    {
                                        result += "\"" + par[0] + "\"" + ":" + "\"" + par[1] + "\",";

                                    }
                                }
                                foreach (var pars in trmpr)
                                {
                                    var par = pars.Split('=');
                                    if (par[0] == "expires_in")
                                    {
                                        result += "\"" + par[0] + "\"" + ":" + "\"" + par[1] + "\"";
                                    }
                                }
                                result += "}";
                            }
                        }
                        catch (WeChatException)
                        {
                            throw;
                        }
                        finally
                        {
                            reader.Close();
                        }
                    }


                    response.Close();
                }
            }
            catch (System.Net.WebException webEx)
            {
//                if (webEx.Message != null)
//                {
                    
//                        string errorInfo = webEx.Message;
//#if DEBUG
//                        Debug.WriteLine(errorInfo);
//#endif
//                        Error error = JsonConvert.DeserializeObject<Error>(errorInfo);

//                        throw new WeiboException(string.Format("{0}", error.Code), error.Message, error.Request);
//                }
//                else
//                {
                    throw webEx;
//                }

            }
            catch
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// OAuth2的authorize接口
        /// </summary>
        /// <param name="state">用于保持请求和回调的状态，在回调时，会在Query Parameter中回传该参数。 </param>
        /// <returns></returns>
        public string GetAuthorizeURL(ScopeType Scope=ScopeType.snsapi_userinfo, string State=null)
        {
            Dictionary<string, string> config = new Dictionary<string, string>()
			{
				{"appid",AppId},
				{"redirect_uri",RedirectUri},
				{"response_type","code"},
                {"scope",Scope.ToString()},
				{"state",State??string.Empty}
			};
            UriBuilder builder = new UriBuilder(AUTHORIZE_URL);
            builder.Query = Utility.BuildQueryString(config);

            return builder.ToString()+"#wechat_redirect";
        }

        public string GetPCAuthorizeURL(ScopeType Scope = ScopeType.snsapi_login, string State = null)
        {
            Dictionary<string, string> config = new Dictionary<string, string>()
            {
                {"appid",AppId},
                {"redirect_uri",RedirectUri},
                {"response_type","code"},
                {"scope",Scope.ToString()},
                {"state",State??string.Empty}
            };
            UriBuilder builder = new UriBuilder(PC_AUTHORIZE_URL);
            builder.Query = Utility.BuildQueryString(config);

            return builder.ToString() + "#wechat_redirect";
        }

        /// <summary>
        /// 判断AccessToken有效性
        /// </summary>
        /// <returns></returns>
        public TokenResult VerifierAccessToken()
        {
            try
            {
                List<WeChatStringParameter> config = new List<WeChatStringParameter>()
                {
                    new WeChatStringParameter(){ Name= "access_token", Value= AccessToken},
                    new WeChatStringParameter(){ Name= "openid", Value= OpenId}
                };
                var json = Request("https://api.weixin.qq.com/sns/userinfo", RequestMethod.Get, false, config.ToArray());

                //if (json != null)
                //{
                //    OpenClientId result = JsonConvert.DeserializeObject<OpenClientId>(json);
                //    OpenId = result.OpenId;
                //    ClientId = result.ClientId;
                //}
            }
            catch (WeChatException ex)
            {
                switch (ex.ErrorCode)
                {
                    case "40001":
                        return TokenResult.TokenInvalid;
                    case "40014":
                        return TokenResult.TokenIliegal;
                    case "42001":
                        return TokenResult.TokenExpired;
                    default:
                        return TokenResult.OtherReason;
                }
            }

            return TokenResult.Success;
        }

        /// <summary>
        /// 使用code方式获取AccessToken
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns></returns>
        public AccessToken GetAccessTokenByAuthorizationCode(string code)
        {
            return GetAccessToken(GrantType.AuthorizationCode, new Dictionary<string, string> { 
				{"code",code},
				{"redirect_uri", RedirectUri}
			});
        }

        /// <summary>
        /// 使用password方式获取AccessToken
        /// </summary>
        /// <param name="passport">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public AccessToken GetAccessTokenByPassword(string passport, string password)
        {
            return GetAccessToken(GrantType.Password, new Dictionary<string, string> { 
				{"username",passport},
				{"password", password}
			});
        }

        /// <summary>
        /// 使用token方式获取AccessToken
        /// </summary>
        /// <param name="refreshToken">refresh token，目前还不知道从哪里获取这个token，未开放</param>
        /// <returns></returns>
        public AccessToken GetAccessTokenByRefreshToken(string refreshToken)
        {
            return GetAccessToken(GrantType.RefreshToken, new Dictionary<string, string> { 
				{"refresh_token",refreshToken}
			});
        }

        /// <summary>
        /// 使用模拟方式进行登录并获得AccessToken
        /// </summary>
        /// <param name="passport">微博账号</param>
        /// <param name="password">微博密码</param>
        /// <returns></returns>
        public bool ClientLogin(string passport, string password)
        {
            bool result = false;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                return true;
            };
            CookieContainer MyCookieContainer = new CookieContainer();
            HttpWebRequest http = WebRequest.Create(AUTHORIZE_URL) as HttpWebRequest;
            http.Referer = GetAuthorizeURL();
            http.Method = "POST";
            http.ContentType = "application/x-www-form-urlencoded";
            http.AllowAutoRedirect = true;
            http.KeepAlive = true;
            http.CookieContainer = MyCookieContainer;
            string postBody = string.Format("action=submit&withOfficalFlag=0&ticket=&isLoginSina=&response_type=token&regCallback=&redirect_uri={0}&client_id={1}&state=&from=&userId={2}&passwd={3}&display=js", HttpUtility.UrlEncode(RedirectUri), HttpUtility.UrlEncode(AppId), HttpUtility.UrlEncode(passport), HttpUtility.UrlEncode(password));
            byte[] postData = Encoding.Default.GetBytes(postBody);
            http.ContentLength = postData.Length;

            using (Stream request = http.GetRequestStream())
            {
                try
                {
                    request.Write(postData, 0, postData.Length);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    request.Close();
                }
            }
            string code = string.Empty;
            try
            {
                using (HttpWebResponse response = http.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            try
                            {
                                var html = reader.ReadToEnd();
                                if (!string.IsNullOrEmpty(html) && Regex.IsMatch(html, @"\{""access_token"":""(?<token>.{0,32})"",""remind_in"":""(?<remind>\d+)"",""expires_in"":(?<expires>\d+),""uid"":""(?<uid>\d+)""\}"))
                                {
                                    var group = Regex.Match(html, @"\{""access_token"":""(?<token>.{32})"",""remind_in"":""(?<remind>\d+)"",""expires_in"":(?<expires>\d+),""uid"":""(?<uid>\d+)""\}");
                                    AccessToken = group.Groups["token"].Value;
                                    result = true;
                                }
                            }
                            catch { }
                            finally
                            {
                                reader.Close();
                            }
                        }
                    }
                    response.Close();
                }
            }
            catch (System.Net.WebException)
            {
                throw;
            }

            return result;

        }

        internal AccessToken GetAccessToken(GrantType type, Dictionary<string, string> parameters)
        {

            List<WeChatStringParameter> config = new List<WeChatStringParameter>()
			{
				new WeChatStringParameter(){ Name= "appid", Value= AppId},
				new WeChatStringParameter(){ Name="secret", Value=AppSecret}
			};

            switch (type)
            {
                case GrantType.AuthorizationCode:
                    {
                        config.Add(new WeChatStringParameter() { Name = "grant_type", Value = "authorization_code" });
                        config.Add(new WeChatStringParameter() { Name = "code", Value = parameters["code"] });
                        //config.Add(new WeChatStringParameter() { Name = "redirect_uri", Value = parameters["redirect_uri"] });
                    }
                    break;
                case GrantType.Password:
                    {
                        config.Add(new WeChatStringParameter() { Name = "grant_type", Value = "password" });
                        config.Add(new WeChatStringParameter() { Name = "username", Value = parameters["username"] });
                        config.Add(new WeChatStringParameter() { Name = "password", Value = parameters["password"] });
                    }
                    break;
                case GrantType.RefreshToken:
                    {
                        config.Add(new WeChatStringParameter() { Name = "grant_type", Value = "refresh_token" });
                        config.Add(new WeChatStringParameter() { Name = "refresh_token", Value = parameters["refresh_token"] });
                    }
                    break;
            }

            var response = Request(ACCESS_TOKEN_URL, RequestMethod.Post, false, config.ToArray());

            if (!string.IsNullOrEmpty(response))
            {
                //dynamic json = DynamicJson.Parse(response);
                //AccessToken = json.access_token;
                //UserID = json.uid;
                //ExpiresIn = json.expires_in;
                //return json.access_token;
                AccessToken token = JsonConvert.DeserializeObject<AccessToken>(response);
                AccessToken = token.Token;
                return token;
            }
            else
            {
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;

namespace WeChat
{
    internal static class Utility
    {
        public static Dictionary<string, string> GetDictionaryFromJSON(string json)
        {
            var result = JsonConvert.DeserializeObject<IEnumerable<JObject>>(json);

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var loc in result)
            {
                foreach (var x in loc.Properties())
                {
                    dict.Add(x.Name, x.Value.ToString());
                }

            }
            return dict;
        }

        public static IEnumerable<string> GetStringListFromJSON(string json)
        {
            var result = JsonConvert.DeserializeObject<IEnumerable<JObject>>(json);
            List<string> list = new List<string>();
            foreach (var loc in result)
            {
                foreach (var x in loc.Properties())
                {
                    list.Add(x.Value.ToString());
                }

            }
            return list;
        }


        public static string BuildQueryString(Dictionary<string, string> parameters)
        {
            List<string> pairs = new List<string>();
            foreach (KeyValuePair<string, string> item in parameters)
            {
                pairs.Add(string.Format("{0}={1}", HttpUtility.UrlEncode(item.Key), HttpUtility.UrlEncode(item.Value)));
            }

            return string.Join("&", pairs.ToArray());
        }

        public static string BuildQueryString(params WeChatParameter[] parameters)
        {
            List<string> pairs = new List<string>();
            foreach (var item in parameters)
            {
                if (item is WeChatStringParameter)
                {
                    pairs.Add(string.Format("{0}={1}", HttpUtility.UrlEncode(item.Name), HttpUtility.UrlEncode(((WeChatStringParameter)item).Value)));
                }
            }

            return string.Join("&", pairs.ToArray());
        }

        public static string GetBoundary()
        {
            return HttpUtility.UrlEncode(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
        }

        public static byte[] BuildPostData(string boundary, params WeChatParameter[] parameters)
        {
            List<WeChatParameter> pairs = new List<WeChatParameter>(parameters);
            pairs.Sort(new WeChatParameterComparer());
            string division = GetBoundary();

            string header = string.Format("--{0}", boundary);
            string footer = string.Format("--{0}--", boundary);
            string encoding = "iso-8859-1";//iso-8859-1

            StringBuilder contentBuilder = new StringBuilder();

            foreach (WeChatParameter p in pairs)
            {
                if (p is WeChatStringParameter)
                {
                    WeChatStringParameter param = p as WeChatStringParameter;
                    contentBuilder.AppendLine(header);
                    contentBuilder.AppendLine(string.Format("content-disposition: form-data; name=\"{0}\"", param.Name));
                    //contentBuilder.AppendLine("Content-Type: text/plain; charset=US-ASCII");// utf-8
                    //contentBuilder.AppendLine("Content-Transfer-Encoding: 8bit");
                    contentBuilder.AppendLine();
                    //contentBuilder.AppendLine(HttpUtility.UrlEncode(param.Value).Replace("+", "%20"));
                    contentBuilder.AppendLine(HttpUtility.UrlEncode(param.Value).Replace("+", "%20"));
                }
                else
                {
                    WeChatBinaryParameter param = p as WeChatBinaryParameter;
                    contentBuilder.AppendLine(header);
                    contentBuilder.AppendLine(string.Format("content-disposition: form-data; name=\"{0}\"; filename=\"{1}\"", param.Name, string.Format("upload{0}", BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0))));
                    contentBuilder.AppendLine("Content-Type: image/unknown");
                    contentBuilder.AppendLine("Content-Transfer-Encoding: binary");
                    contentBuilder.AppendLine();
                    contentBuilder.AppendLine(Encoding.GetEncoding(encoding).GetString(param.Value));

                }
            }

            contentBuilder.Append(footer);

            return Encoding.GetEncoding(encoding).GetBytes(contentBuilder.ToString());

        }



    }

    internal enum GrantType
    {
        AuthorizationCode,
        Password,
        RefreshToken
    }

    internal enum RequestMethod
    {
        Get,
        Post
    }

    public enum TokenResult
    {
        /// <summary>
        /// 正常
        /// </summary>
        Success,
        /// <summary>
        /// 获取access_token时AppSecret错误，或者access_token无效
        /// </summary>
        TokenInvalid,
        /// <summary>
        /// 不合法的access_token
        /// </summary>
        TokenIliegal,
        /// <summary>
        /// access_token超时
        /// </summary>
        TokenExpired,
        /// <summary>
        /// 其他原因
        /// </summary>
        OtherReason
    }

    public enum ScopeType
    {
        snsapi_base,
        snsapi_userinfo,
        snsapi_login
    }

    internal class WeChatParameterComparer : IComparer<WeChatParameter>
    {

        public int Compare(WeChatParameter x, WeChatParameter y)
        {
            return StringComparer.CurrentCulture.Compare(x.Name, y.Name);
        }
    }
}

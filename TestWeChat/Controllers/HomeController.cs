using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWeChat.Helper;
using System.Web.Security;
using Elmah;

namespace TestWeChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "你的应用程序说明页。";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "你的联系方式页。";

            return View();
        }

        public string TokenVerify()
        {
            var Oauth = WeChatHelper.GetWeChatOauth("OezXcEiiBSKSxW0eoylIeK3x6rAclmHWD9s1lGkVKyhwy-EDWU0f9kwJNAmS1_11ndxPqyA5NoL8zN8D0TlEMo8cmlXSpTgiPui4Z7gSCc79E-uzGkZ4G1aNJQikmt7_H5HBbPDZ-2YwX-GhXOibZg", "ouAbjtwKlbjJqsn1Y-Vul2VYaLW4");

            return Oauth.VerifierAccessToken().ToString();
        }

        #region 微信一键登录
        public ActionResult WeChatLogOn()
        {
            try
            {
                if (Session["WeChatTokon"] != null)
                //if (Request.Cookies["sinaweibotoken"] != null)
                {
                    WeChat.AccessToken WeChatToken = Session["WeChatTokon"] as WeChat.AccessToken;
                    //HttpCookie weibocookie = Request.Cookies["sinaweibotoken"];

                    if (WeChatToken != null)
                    {
                        var Oauth = WeChatHelper.GetWeChatOauth(WeChatToken.Token,WeChatToken.OpenId);

                        if (Oauth.VerifierAccessToken() == WeChat.TokenResult.Success)//验证AccessToken是否有效
                        {
                            var qq = new WeChat.Client(Oauth);

                            var qqUser = qq.API.Entity.Users.Show();
                            string Ousername = "qq_" + qq.OAuth.OpenId;



                            ViewBag.UserName = Ousername;
                            ViewBag.QQID = qq.OAuth.OpenId;
                            ViewBag.Name = qqUser.Nickname;
                            return View();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Error", "Home", new { errorMessage = ex.Message });
            }
            var oauth = WeChatHelper.GetWeChatToken(WeChatHelper.callbackurl);
            var authUrl = oauth.GetPCAuthorizeURL();
            return Redirect(authUrl);
        }

        [AllowAnonymous]
        public ActionResult WeChatLogOnSuccess(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var oauth = WeChatHelper.GetWeChatToken(WeChatHelper.callbackurl);
                var accessToken = oauth.GetAccessTokenByAuthorizationCode(code);

                Session["WeChatTokon"] = accessToken;
            }
            return RedirectToAction("WeChatLogOn");
        }
        #endregion
    }
}

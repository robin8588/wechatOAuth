using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat;

namespace TestWeChat.Helper
{
    public class WeChatHelper
    {
        static string appId = "";
        static string appSecret = "";

        public static string callbackurl = "";

        /// <summary>
        /// 使用授权
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public static OAuth GetWeChatOauth(string accessToken,string openId)
        {
            return new WeChat.OAuth(appId, appSecret, accessToken, openId, null);
        }

        /// <summary>
        /// 获得授权
        /// </summary>
        /// <returns></returns>
        public static OAuth GetWeChatToken(string backurl)
        {
            return new WeChat.OAuth(appId, appSecret, backurl);
        }
    }
}
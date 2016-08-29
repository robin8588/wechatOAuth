using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat
{
    /// <summary>
    /// 微博操作类
    /// </summary>
    public class Client
    {

        const string BASE_URL = "https://api.weixin.qq.com/sns/";

        /// <summary>
        /// OAuth实例
        /// </summary>
        public OAuth OAuth
        {
            get;
            private set;
        }


        /// <summary>
        /// 微博动态类型接口
        /// </summary>
        public Interface.InterfaceSelector API
        {
            get;
            private set;
        }



        /// <summary>
        /// 实例化微博操作类
        /// </summary>
        /// <param name="oauth">OAuth实例</param>
        public Client(OAuth oauth)
        {
            this.OAuth = oauth;

            API = new Interface.InterfaceSelector(this);

        }


        internal string PostCommand(string command, params WeChatParameter[] parameters)
        {
            return PostCommand(command, false, parameters);
        }

        internal string PostCommand(string command, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            List<WeChatParameter> list = new List<WeChatParameter>();
            foreach (var item in parameters)
            {
                list.Add(new WeChatStringParameter(item.Key, item.Value));
            }
            return PostCommand(command, false, list.ToArray());
        }

        internal string PostCommand(string command, bool multi = true, params WeChatParameter[] parameters)
        {
            return Http(command, RequestMethod.Post, multi, parameters);
        }


        internal string GetCommand(string command, params WeChatParameter[] parameters)
        {
            List<WeChatParameter> list = new List<WeChatParameter>()
            {
                new WeChatStringParameter(){ Name= "access_token", Value= OAuth.AccessToken},
                new WeChatStringParameter(){ Name= "openid", Value= OAuth.OpenId}
            };
            foreach (var item in parameters)
            {
                list.Add(new WeChatStringParameter(item.Name, item.Value));
            }
            return Http(command, RequestMethod.Get, false, list.ToArray());
        }

        internal string GetCommand(string command, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            List<WeChatParameter> list = new List<WeChatParameter>()
            {
                new WeChatStringParameter(){ Name= "access_token", Value= OAuth.AccessToken},
                new WeChatStringParameter(){ Name= "openid", Value= OAuth.OpenId}
            };
            foreach (var item in parameters)
            {
                list.Add(new WeChatStringParameter(item.Key, item.Value));
            }
            return Http(command, RequestMethod.Get, false, list.ToArray());
        }

        private string Http(string command, RequestMethod method, bool multi, params WeChatParameter[] parameters)
        {
            string url = string.Empty;

            if (command.StartsWith("http://") || command.StartsWith("https://"))
            {
                url = command;
            }
            else
            {
                url = string.Format("{0}{1}", BASE_URL, command);
            }

            return OAuth.Request(url, method, multi, parameters);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace WeChat.Interface.Entity
{
    public class UserInterface : WeChatInterface
    {
        public UserInterface(Client client)
            : base(client)
        {

        }
        /// <summary>
        /// 获取用户信
        /// </summary>
        /// <returns></returns>
        public Entities.User.Entity Show()
        {
            Entities.User.Entity userinfo = new Entities.User.Entity();
            userinfo = JsonConvert.DeserializeObject<Entities.User.Entity>(Client.GetCommand("userinfo"));
            return userinfo;
        }
    }
}

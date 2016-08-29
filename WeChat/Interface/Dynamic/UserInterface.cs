using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codeplex.Data;

namespace WeChat.Interface.Dynamic
{
    /// <summary>
    /// User接口
    /// </summary>
    public class UserInterface :WeChatInterface
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="client">操作类实例</param>
        public UserInterface(Client client)
            : base(client)
        {

        }
        /// <summary>
        /// 获取用户信
        /// </summary>
        /// <returns></returns>
        public dynamic Show()
        {
            return DynamicJson.Parse(Client.GetCommand("userinfo"));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeChat.Interface.Dynamic;

namespace WeChat.Interface
{
    /// <summary>
    /// 动态类型API接口封装
    /// </summary>
    public class DynamicInterfaces
    {

        ///// <summary>
        ///// 用户接口
        ///// </summary>
        public UserInterface Users { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="client">操作器</param>
        public DynamicInterfaces(Client client)
        {

            Users = new UserInterface(client);
        }
    }
}

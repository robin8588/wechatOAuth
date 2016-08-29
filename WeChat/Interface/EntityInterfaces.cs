using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeChat.Interface.Entity;

namespace WeChat.Interface
{
    /// <summary>
    /// 实体类型类型API接口封装
    /// </summary>
    public class EntityInterfaces
    {


        /// <summary>
        /// 用户接口
        /// </summary>
        public UserInterface Users { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="client">操作器</param>
        public EntityInterfaces(Client client)
        {

            Users = new UserInterface(client);
        }
    }
}

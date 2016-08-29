using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat.Interface
{
    /// <summary>
    /// 微博API接口封装基类
    /// </summary>
    public abstract class WeChatInterface
    {
        /// <summary>
        /// 操作类
        /// </summary>
        protected Client Client;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="client">操作类实例</param>
        public WeChatInterface(Client client)
        {
            this.Client = client;
        }
    }
}

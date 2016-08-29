using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace WeChat
{
    public class Error
    {
        /// <summary>
        /// 错误编码
        /// </summary>
        [JsonProperty(PropertyName = "errcode")]
        public string Code
        {
            get;
            internal set;
        }
        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty(PropertyName = "errmsg")]
        public string Message
        {
            get;
            internal set;
        }
    }
}

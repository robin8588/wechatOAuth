using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace WeChat.Entities.User
{
    public class Entity
    {
        [JsonProperty(PropertyName = "openid")]
        public string Openid { get; internal set; }
        [JsonProperty(PropertyName = "nickname")]
        public string Nickname { get; internal set; }
        [JsonProperty(PropertyName = "sex")]
        public string Sex { get; internal set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; internal set; }
        [JsonProperty(PropertyName = "country")]
        public string Country { get; internal set; }
        [JsonProperty(PropertyName = "province")]
        public string Province { get; internal set; }
        [JsonProperty(PropertyName = "headimgurl")]
        public string Headimgurl { get; internal set; }
        [JsonProperty(PropertyName = "privilege")]
        public string[] Privilege { get; internal set; }
    }
}

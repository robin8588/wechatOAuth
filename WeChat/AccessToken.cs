using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace WeChat
{
    public class AccessToken
    {
        [JsonProperty(PropertyName = "access_token")]
        public string Token { get; internal set; }
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; internal set; }
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; internal set; }
        [JsonProperty(PropertyName = "openid")]
        public string OpenId { get; internal set; }
        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; internal set; }
        [JsonProperty(PropertyName = "unionid")]
        public string UnionId { get; internal set; }
    }
}

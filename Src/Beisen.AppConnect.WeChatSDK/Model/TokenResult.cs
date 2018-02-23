using Newtonsoft.Json;

namespace Beisen.AppConnect.WeChatSDK.Model
{
    /// <summary>
    /// 公众号票据
    /// </summary>
    public class TokenResult : WeChatResult
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}

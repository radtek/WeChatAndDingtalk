using Newtonsoft.Json;

namespace Beisen.AppConnect.WeChatSDK.Model
{
    /// <summary>
    /// 企业成员信息（OAuth）
    /// </summary>
    public class OAuthQYUserInfoResult:WeChatResult
    {
        /// <summary>
        /// 成员UserID
        /// </summary>
        [JsonProperty("UserId")]
        public string UserId { get; set; }

        /// <summary>
        /// 非企业成员的标识，对当前企业号唯一
        /// </summary>
        [JsonProperty("OpenId")]
        public string OpenId { get; set; }

        /// <summary>
        /// 手机设备号(由微信在安装时随机生成，删除重装会改变，升级不受影响)
        /// </summary>
        [JsonProperty("DeviceId")]
        public string DeviceId { get; set; }

    }
}

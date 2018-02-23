using Newtonsoft.Json;

namespace Beisen.AppConnect.WeChatSDK.Model
{
    /// <summary>
    /// 微信返回结果
    /// </summary>
    public class WeChatResult
    {
        /// <summary>
        /// 错误码（0：成功，其他失败）
        /// </summary>
        [JsonProperty("errcode")]
        public int ErrCode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        [JsonProperty("errmsg")]
        public string ErrMsg { get; set; }
    }
}

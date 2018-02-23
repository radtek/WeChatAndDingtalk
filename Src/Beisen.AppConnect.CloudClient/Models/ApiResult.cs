using Newtonsoft.Json;

namespace Beisen.AppConnect.CloudClient.Models
{
    public class ApiResult
    {
        /// <summary>
        /// 错误码（0：成功，其他失败）
        /// 2001:未开通应用
        /// 2002:账户不存在
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

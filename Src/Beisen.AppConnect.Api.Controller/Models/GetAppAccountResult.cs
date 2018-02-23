using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class GetAppAccountResult:ApiResult
    {
        /// <summary>
        /// 多租赁Id
        /// </summary>
        [JsonProperty("appaccount_id")]
        public string AppAccountId { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        [JsonProperty("tenant_id")]
        public int TenantId { get; set; }

        /// <summary>
        /// 账户名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 账户类型
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }

        /// <summary>
        /// 原始ID
        /// </summary>
        [JsonProperty("source_id")]
        public string SourceId { get; set; }

        /// <summary>
        /// AppId
        /// </summary>
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        /// <summary>
        /// AppSecret
        /// </summary>
        [JsonProperty("app_secret")]
        public string AppSecret { get; set; }

        /// <summary>
        /// 企业应用的id
        /// </summary>
        [JsonProperty("agent_id")]
        public string AgentId { get; set; }

        /// <summary>
        /// 账户状态
        /// </summary>
        [JsonProperty("state")]
        public int State { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }
    }
}

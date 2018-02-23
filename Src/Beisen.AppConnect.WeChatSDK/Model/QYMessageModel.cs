using Newtonsoft.Json;

namespace Beisen.AppConnect.WeChatSDK.Model
{
    /// <summary>
    /// 企业号消息
    /// </summary>
    public class QYMessageModel
    {
        /// <summary>
        /// 成员ID列表（消息接收者，多个接收者用‘|’分隔，最多支持1000个）。特殊情况：指定为@all，则向关注该企业应用的全部成员发送
        /// </summary>
        [JsonProperty("touser")]
        public string ToUser { get; set; }

        /// <summary>
        /// 部门ID列表，多个接收者用‘|’分隔，最多支持100个。当touser为@all时忽略本参数
        /// </summary>
        [JsonProperty("toparty")]
        public string ToParty { get; set; }

        /// <summary>
        /// 标签ID列表，多个接收者用‘|’分隔，最多支持100个。当touser为@all时忽略本参数
        /// </summary>
        [JsonProperty("totag")]
        public string ToTag { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [JsonProperty("msgtype")]
        public string MsgType { get; set; }

        /// <summary>
        /// 企业应用的id
        /// </summary>
        [JsonProperty("agentid")]
        public int AgentId { get; set; }

        /// <summary>
        /// text消息内容
        /// </summary>
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public object Text { get; set; }

        /// <summary>
        /// image消息内容
        /// </summary>
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public object Image { get; set; }

        /// <summary>
        /// voice消息内容
        /// </summary>
        [JsonProperty("voice", NullValueHandling = NullValueHandling.Ignore)]
        public object Voice { get; set; }

        /// <summary>
        /// video消息内容
        /// </summary>
        [JsonProperty("video", NullValueHandling = NullValueHandling.Ignore)]
        public object Video { get; set; }

        /// <summary>
        /// file消息内容
        /// </summary>
        [JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        public object File { get; set; }

        /// <summary>
        /// news消息内容
        /// </summary>
        [JsonProperty("news", NullValueHandling = NullValueHandling.Ignore)]
        public object News { get; set; }

        /// <summary>
        /// mpnews消息内容
        /// </summary>
        [JsonProperty("mpnews", NullValueHandling = NullValueHandling.Ignore)]
        public object MpNews { get; set; }

        /// <summary>
        /// 是否是保密消息，0表示否，1表示是，默认0
        /// </summary>
        [JsonProperty("safe", NullValueHandling = NullValueHandling.Ignore)]
        public int Safe { get; set; }
    }
}

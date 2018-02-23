using Newtonsoft.Json;

namespace Beisen.AppConnect.WeChatSDK.Model
{
    /// <summary>
    /// 发送模板消息结果
    /// </summary>
    public class TemplateForSendResult:WeChatResult
    {
        /// <summary>
        /// 模板消息发送Id
        /// </summary>
        [JsonProperty("msgid")]
        public long MsgId { get; set; }
    }
}

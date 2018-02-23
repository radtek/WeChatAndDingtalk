using Newtonsoft.Json;

namespace Beisen.AppConnect.ServiceInterface.Model.WebModel
{
    public class MessageSendResult : ResultModel
    {
        /// <summary>
        /// 模板消息发送内部Id
        /// </summary>
        [JsonProperty("msgid")]
        public long MsgId { get; set; }
    }
}

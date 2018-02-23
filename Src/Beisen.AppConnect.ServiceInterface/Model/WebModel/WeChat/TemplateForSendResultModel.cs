using Newtonsoft.Json;

namespace Beisen.AppConnect.ServiceInterface.Model.WebModel.WeChat
{
    /// <summary>
    /// 发送模板消息结果
    /// </summary>
    public class TemplateForSendResultModel : ResultModel
    {
        /// <summary>
        /// 模板消息发送内部Id
        /// </summary>
        [JsonProperty("msgid")]
        public long MsgId { get; set; }
    }
}

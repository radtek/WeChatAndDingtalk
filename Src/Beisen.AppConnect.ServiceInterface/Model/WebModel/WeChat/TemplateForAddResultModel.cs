using Newtonsoft.Json;

namespace Beisen.AppConnect.ServiceInterface.Model.WebModel.WeChat
{
    public class TemplateForAddResultModel: ResultModel
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        [JsonProperty("template_id")]
        public string TemplateId { get; set; }
    }
}

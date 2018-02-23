using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beisen.AppConnect.WeChatSDK.Model
{
    /// <summary>
    /// 模板列表
    /// </summary>
    public class TemplateForGetAllPrivateResult:WeChatResult
    {
        /// <summary>
        /// 模板列表
        /// </summary>
        [JsonProperty("template_list")]
        public List<TemplateForGetAllPrivateDetail> TemplateList { get; set; }
    }

    /// <summary>
    /// 模板列表明细
    /// </summary>
    public class TemplateForGetAllPrivateDetail
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        [JsonProperty("template_id")]
        public string TemplateId { get; set; }
        /// <summary>
        /// 模板标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// 模板所属行业的一级行业
        /// </summary>
        [JsonProperty("primary_industry")]
        public string PrimaryIndustry { get; set; }
        /// <summary>
        /// 模板所属行业的二级行业
        /// </summary>
        [JsonProperty("deputy_industry")]
        public string DeputyIndustry { get; set; }
        /// <summary>
        /// 模板内容
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
        /// <summary>
        /// 模板示例
        /// </summary>
        [JsonProperty("example")]
        public string Example { get; set; }
    }
}

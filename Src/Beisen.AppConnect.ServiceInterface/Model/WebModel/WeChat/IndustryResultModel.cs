using Newtonsoft.Json;

namespace Beisen.AppConnect.ServiceInterface.Model.WebModel.WeChat
{
    public class IndustryResultModel: ResultModel
    {
        /// <summary>
        /// 帐号设置的主营行业
        /// </summary>
        [JsonProperty("primary_industry")]
        public IndustryDetailModel PrimaryIndustry { get; set; }

        /// <summary>
        /// 帐号设置的副营行业
        /// </summary>
        [JsonProperty("secondary_industry")]
        public IndustryDetailModel SecondaryIndustry { get; set; }
    }

    /// <summary>
    /// 行业信息明细
    /// </summary>
    public class IndustryDetailModel
    {
        /// <summary>
        /// 主行业
        /// </summary>
        [JsonProperty("first_class")]
        public string FirstClass { get; set; }

        /// <summary>
        /// 副行
        /// </summary>
        [JsonProperty("second_class")]
        public string SecondClass { get; set; }
    }
}

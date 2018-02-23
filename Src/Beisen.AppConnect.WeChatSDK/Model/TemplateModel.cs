using Newtonsoft.Json;

namespace Beisen.AppConnect.WeChatSDK.Model
{
    /// <summary>
    /// 模板消息
    /// </summary>
    public class TemplateModel
    {
        /// <summary>
        /// 目标用户OpenId
        /// </summary>
        [JsonProperty("touser")]
        public string ToUser { get; set; }

        /// <summary>
        /// 模板Id
        /// </summary>
        [JsonProperty("template_id")]
        public string TemplateId { get; set; }

        /// <summary>
        /// 点击模板后调转url
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [JsonProperty("data")]
        public object Data { get; set; }
    }

    /// <summary>
    /// 模板消息字段数据项。
    /// </summary>
    public sealed class TemplateMessageFieldDataItem
    {
        /// <summary>
        /// 初始化一个新的模板消息字段数据项。
        /// </summary>
        /// <param name="value">字段值。</param>
        public TemplateMessageFieldDataItem(string value)
            : this(value, "#000000")
        {
        }

        /// <summary>
        /// 初始化一个新的模板消息字段数据项。
        /// </summary>
        /// <param name="color">字段颜色。</param>
        /// <param name="value">字段值。</param>
        public TemplateMessageFieldDataItem(string value, string color)
        {
            Value = value;
            Color = color;
        }

        /// <summary>
        /// 值。
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// 颜色。
        /// </summary>
        [JsonProperty("color")]
        public string Color { get; set; }
    }
}

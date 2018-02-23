namespace Beisen.AppConnect.ServiceInterface.Model
{
    public class JsApiConfig
    {
        /// <summary>
        /// 当前请求页面url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 应用id
        /// </summary>
        public string AgentId { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public double TimeStamp { get; set; }

        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 钉钉签名包
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; set; }

        public string RedirectUri { get; set; }

        public string State { get; set; }
    }
}

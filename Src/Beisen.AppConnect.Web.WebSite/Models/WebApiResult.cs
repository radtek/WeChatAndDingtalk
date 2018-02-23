namespace Beisen.AppConnect.Web.WebSite.Models
{
    public class WebApiResult
    {
        /// <summary>
        /// 返回状态码（见ApiResultCode）
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回错误信息
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// Api返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WebApiResult<T>
    {
        /// <summary>
        /// 返回状态码（见ApiResultCode）
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回错误信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }
    }
}

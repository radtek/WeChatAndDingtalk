namespace Beisen.AppConnect.ServiceImp.Provider
{
    /// <summary>
    /// 模板常量字段
    /// </summary>
    public class TemplateConst
    {
        #region 接口名称

        /// <summary>
        /// 网页授权
        /// </summary>
        public const string AuthorizeUrl = "AUTHORIZE_URL";

        /// <summary>
        /// 获取OpenId
        /// </summary>
        public const string GetOpenId = "GET_OPENID";

        /// <summary>
        /// PC 网页授权
        /// </summary>
        public const string AuthorizePCUrl = "AUTHORIZE_PCURL";
        #endregion

        #region 请求变量

        /// <summary>
        /// Api域名
        /// </summary>
        public const string Host = "<%HOST%>";

        /// <summary>
        /// AppId
        /// </summary>
        public const string AppId = "<%APPID%>";

        /// <summary>
        /// AppSecret
        /// </summary>
        public const string Secret = "<%SECRET%>";

        /// <summary>
        /// 回调地址
        /// </summary>
        public const string RedirectUri = "<%REDIRECT_URI%>";

        /// <summary>
        /// 返回类型
        /// </summary>
        public const string ResponseType = "<%RESPONSE_TYPE%>";

        /// <summary>
        /// 授权、作用域，目前只微信使用（snsapi_bas、snsapi_userinfo  ）
        /// </summary>
        public const string Scope = "<%SCOPE%>";

        /// <summary>
        /// 重定向后的state参数
        /// </summary>
        public const string State = "<%STATE%>";

        /// <summary>
        /// Token
        /// </summary>
        public const string Token = "<%TOKEN%>";

        /// <summary>
        /// 网页授权Token
        /// </summary>
        public const string OAuthToken = "<%OAUTHTOKEN%>";

        /// <summary>
        /// 网页授权Code
        /// </summary>
        public const string Code = "<%CODE%>";

        public const string Batch = "<%BATCH%>";

        #endregion

        #region 扩展变量

        /// <summary>
        /// State参数
        /// </summary>
        public const string ExtendState = "EXTEND_STATE";

        /// <summary>
        /// 返回Url批次号
        /// </summary>
        public const string ExtendBatch = "EXTEND_BATCH";

        /// <summary>
        /// 授权Code
        /// </summary>
        public const string ExtendCode = "EXTEND_CODE";

        public const string ExtendToken = "EXTEND_TOKEN";

        #endregion

        #region 返回变量

        /// <summary>
        /// 网页授权Token
        /// </summary>
        public const string OpenId = "OPENID";

        #endregion
    }
}
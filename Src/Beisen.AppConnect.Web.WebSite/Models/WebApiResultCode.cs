namespace Beisen.AppConnect.Web.WebSite.Models
{
    public enum WebApiResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 未登录
        /// </summary>
        UnLogin = 1,

        /// <summary>
        /// OpenId无效
        /// </summary>
        OpenIdError=2,

        /// <summary>
        /// 登录错误
        /// </summary>
        LoginError=4,

        /// <summary>
        /// 用户不存在
        /// </summary>
        NoAccount=5,

        /// <summary>
        /// 邮箱校验错误
        /// </summary>
        EmailCheckError=6,

        /// <summary>
        /// 帐号已绑定
        /// </summary>
        HasBind=7,

        /// <summary>
        /// 当前帐号已经绑定
        /// </summary>
        FinishBind=8
    }
}

namespace Beisen.AppConnect.ServiceInterface.Model.Enum
{
    /// <summary>
    /// 用户绑定类型
    /// </summary>
    public enum AppUserAccountType
    {
        /// <summary>
        /// 登录绑定
        /// </summary>
        Login = 0,

        /// <summary>
        /// 邮件绑定
        /// </summary>
        Email = 1,

        /// <summary>
        /// 手机绑定
        /// </summary>
        Phone = 2,
        /// <summary>
        /// 批量导入绑定
        /// </summary>
        Batch = 3
    }
}

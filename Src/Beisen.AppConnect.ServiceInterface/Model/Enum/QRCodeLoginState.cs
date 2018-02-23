namespace Beisen.AppConnect.ServiceInterface.Model.Enum
{
    /// <summary>
    /// 二维码登录状态
    /// </summary>
    public enum QrCodeLoginState
    {
        /// <summary>
        /// 未登录
        /// </summary>
        UnLogin=0,

        /// <summary>
        /// 已扫描
        /// </summary>
        Scanned = 1,

        /// <summary>
        /// 已登录
        /// </summary>
        Login = 2,

        /// <summary>
        /// 已取消
        /// </summary>
        Cancel = 3,

        /// <summary>
        /// 已失效
        /// </summary>
        Invalid=4
    }
}

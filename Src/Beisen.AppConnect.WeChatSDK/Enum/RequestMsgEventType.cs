namespace Beisen.AppConnect.WeChatSDK.Enum
{
    /// <summary>
    /// 当RequestMsgType类型为Event时，Event属性的类型
    /// </summary>
    public enum RequestMsgEventType
    {
        /// <summary>
        /// 关注（包含扫码关注）
        /// </summary>
        Subscribe = 1,

        /// <summary>
        /// 取消订阅
        /// </summary>
        Unsubscribe = 2,

        /// <summary>
        /// 扫码（已关注）
        /// </summary>
        Scan = 3,

        /// <summary>
        /// 上报地理位置
        /// </summary>
        Location = 4,

        /// <summary>
        /// 自定义菜单点击事件
        /// </summary>
        Click = 5,

        /// <summary>
        /// URL跳转
        /// </summary>
        View = 6,

        /// <summary>
        /// 模板消息完成推送事件
        /// </summary>
        TemplateSendJobFinish = 7
    }
}

using Beisen.AppConnect.WeChatSDK.Handler.Request;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    /// <summary>
    /// 消息回调接口
    /// </summary>
    public interface IRequestMsgProvider
    {
        #region 普通消息
        /// <summary>
        /// 增加文本消息请求
        /// </summary>
        /// <param name="message">消息</param>
        void InsertText(RequestMsgText message);

        /// <summary>
        /// 增加图片消息请求
        /// </summary>
        /// <param name="message">消息</param>
        void InsertImage(RequestMsgImage message);

        /// <summary>
        /// 增加语音消息请求
        /// </summary>
        /// <param name="message">消息</param>
        void InsertVoice(RequestMsgVoice message);

        /// <summary>
        /// 增加视频消息请求
        /// </summary>
        /// <param name="message">消息</param>
        void InsertVideo(RequestMsgVideo message);

        /// <summary>
        /// 增加小视频消息请求
        /// </summary>
        /// <param name="message">消息</param>
        void InsertShortVideo(RequestMsgShortVideo message);

        /// <summary>
        /// 增加上报地理位置事件消息请求
        /// </summary>
        /// <param name="message">消息</param>
        void InsertLocation(RequestMsgLocation message);

        /// <summary>
        /// 增加链接消息请求
        /// </summary>
        /// <param name="message">消息</param>
        void InsertLink(RequestMsgLink message);
        #endregion

        #region 事件消息
        /// <summary>
        /// 增加关注事件请求(扫描带参数二维码关注)
        /// </summary>
        /// <param name="message">消息</param>
        void InsertEventSubscribe(RequestMsgEventSubscribe message);

        /// <summary>
        /// 增加取消关注事件请求
        /// </summary>
        /// <param name="message">消息</param>
        void InsertEventUnsubscribe(RequestMsgEventUnsubscribe message);

        /// <summary>
        /// 增加扫描带参数二维码事件请求(用户已关注)
        /// </summary>
        /// <param name="message">消息</param>
        void InsertEventScan(RequestMsgEventScan message);

        /// <summary>
        /// 增加上报地理位置事件消息请求
        /// </summary>
        /// <param name="message">消息</param>
        void InsertEventLocation(RequestMsgEventLocation message);

        /// <summary>
        /// 增加点击菜单拉取消息时的事件推送消息请求
        /// </summary>
        /// <param name="message">消息</param>
        void InsertEventClick(RequestMsgEventClick message);

        /// <summary>
        /// 增加点击菜单跳转链接时的事件推送消息请求
        /// </summary>
        /// <param name="message">消息</param>
        void InsertEventView(RequestMsgEventView message);
        #endregion
    }
}

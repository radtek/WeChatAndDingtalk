using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 取消关注事件
    /// </summary>
    public class RequestMsgEventUnsubscribe : RequestMsgEventBase
    {
        /// <summary>
        /// 初始化取消关注事件
        /// </summary>
        /// <param name="doc">XML消息</param>
        public RequestMsgEventUnsubscribe(XDocument doc)
            : base(RequestMsgEventType.Unsubscribe, doc)
        {
        }
    }
}
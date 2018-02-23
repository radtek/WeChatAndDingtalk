using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 请求消息事件基类
    /// </summary>
    public class RequestMsgEventBase : RequestMsgBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public RequestMsgEventType Event { get; set; }

        /// <summary>
        /// 初始化请求消息事件
        /// </summary>
        /// <param name="msgEvent">消息事件类型</param>
        /// <param name="doc">XML消息</param>
        public RequestMsgEventBase(RequestMsgEventType msgEvent, XDocument doc)
            :base(RequestMsgType.Event, doc)
        {
            Event = msgEvent;
        }
    }
}

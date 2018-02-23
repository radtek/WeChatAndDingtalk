using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 点击菜单跳转链接事件
    /// </summary>
    public class RequestMsgEventView : RequestMsgEventBase
    {
        /// <summary>
        /// 事件KEY值，设置的跳转URL
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 初始化点击菜单跳转链接事件
        /// </summary>
        /// <param name="doc">XML消息</param>
        public RequestMsgEventView(XDocument doc)
            :base(RequestMsgEventType.View, doc)
        {
            EventKey = doc.Root.Element("EventKey").Value;
        }
    }
}

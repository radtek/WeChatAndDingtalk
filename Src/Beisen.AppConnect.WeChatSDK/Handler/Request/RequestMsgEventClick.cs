using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 自定义菜单事件
    /// </summary>
    public class RequestMsgEventClick : RequestMsgEventBase
    {
        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 初始化自定义菜单事件
        /// </summary>
        /// <param name="doc">XML消息</param>
        public RequestMsgEventClick(XDocument doc)
            :base(RequestMsgEventType.Click, doc)
        {
            EventKey = doc.Root.Element("EventKey").Value;
        }
    }
}

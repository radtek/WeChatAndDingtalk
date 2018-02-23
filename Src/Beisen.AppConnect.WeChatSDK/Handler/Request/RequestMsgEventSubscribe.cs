using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 关注事件(扫描带参数二维码关注)
    /// </summary>
    public class RequestMsgEventSubscribe : RequestMsgEventBase
    {
        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }

        /// <summary>
        /// 初始化关注事件(扫描带参数二维码关注)
        /// </summary>
        /// <param name="doc">XML消息</param>
        public RequestMsgEventSubscribe(XDocument doc)
            : base(RequestMsgEventType.Subscribe, doc)
        {
            var element = doc.Root.Element("EventKey");
            if (element != null)
            {
                EventKey = element.Value;
            }

            var xElement = doc.Root.Element("Ticket");
            if (xElement != null)
            {
                Ticket = xElement.Value;
            }
        }
    }
}

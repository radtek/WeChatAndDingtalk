using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 扫描带参数二维码事件(用户已关注)
    /// </summary>
    public class RequestMsgEventScan : RequestMsgEventBase
    {
        /// <summary>
        /// 事件KEY值，是一个32位无符号整数，即创建二维码时的二维码scene_id
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }

        /// <summary>
        /// 初始化扫描带参数二维码事件(用户已关注)
        /// </summary>
        /// <param name="doc">XML消息</param>
        public RequestMsgEventScan(XDocument doc)
            : base(RequestMsgEventType.Scan, doc)
        {
            EventKey = doc.Root.Element("EventKey").Value;
            Ticket = doc.Root.Element("Ticket").Value;
        }
    }
}

using System;
using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 上报地理位置事件
    /// </summary>
    public class RequestMsgEventLocation : RequestMsgEventBase
    {
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// 地理位置精度
        /// </summary>
        public float Precision { get; set; }

        /// <summary>
        /// 初始化上报地理位置事件
        /// </summary>
        /// <param name="doc">XML消息</param>
        public RequestMsgEventLocation(XDocument doc)
            :base(RequestMsgEventType.Location, doc)
        {
            Latitude = Convert.ToSingle(doc.Root.Element("Latitude").Value);
            Longitude = Convert.ToSingle(doc.Root.Element("Longitude").Value);
            Precision = Convert.ToSingle(doc.Root.Element("Precision").Value);
        }
    }
}

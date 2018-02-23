using System;
using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 地理位置消息
    /// </summary>
    public class RequestMsgLocation : RequestMsgBase
    {
        /// <summary>
        /// 地理位置维度
        /// </summary>
        public float LocationX { get; set; }

        /// <summary>
        /// 地理位置经度

        /// </summary>
        public float LocationY { get; set; }

        /// <summary>
        /// 地图缩放大小

        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// 地理位置信息

        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }

        /// <summary>
        /// 初始化地理位置消息
        /// </summary>
        /// <param name="doc">XML消息</param>
        public RequestMsgLocation(XDocument doc)
            : base(RequestMsgType.Location, doc)
        {
            LocationX = Convert.ToSingle(doc.Root.Element("Location_X").Value);
            LocationY = Convert.ToSingle(doc.Root.Element("Location_Y").Value);
            Scale = Convert.ToInt32(doc.Root.Element("Scale").Value);
            Label = doc.Root.Element("Label").Value;
            MsgId = Convert.ToInt64(doc.Root.Element("MsgId").Value);
        }
    }
}
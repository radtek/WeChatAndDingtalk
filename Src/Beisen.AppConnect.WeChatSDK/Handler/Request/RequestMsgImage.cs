using System;
using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 图片消息
    /// </summary>
    public class RequestMsgImage : RequestMsgBase
    {
        /// <summary>
        /// 图片链接（由系统生成）
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 图片消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }

        /// <summary>
        /// 初始化图片消息
        /// </summary>
        /// <param name="doc">XML消息</param>
        public RequestMsgImage(XDocument doc)
            : base(RequestMsgType.Image, doc)
        {
            PicUrl = doc.Root.Element("PicUrl").Value;
            MediaId = doc.Root.Element("MediaId").Value;
            MsgId = Convert.ToInt64(doc.Root.Element("MsgId").Value);
        }
    }
}

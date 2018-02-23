using System;
using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 视频消息
    /// </summary>
    public class RequestMsgVideo : RequestMsgBase
    {
        /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string ThumbMediaId { get; set; }

        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }

        /// <summary>
        /// 初始化视频消息
        /// </summary>
        /// <param name="doc">XML消息</param>
        public RequestMsgVideo(XDocument doc)
            : base(RequestMsgType.Video, doc)
        {
            MediaId = doc.Root.Element("MediaId").Value;
            ThumbMediaId = doc.Root.Element("ThumbMediaId").Value;
            MsgId = Convert.ToInt64(doc.Root.Element("MsgId").Value);
        }
    }
}
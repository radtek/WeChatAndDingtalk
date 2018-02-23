using System;
using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 语音消息
    /// </summary>
    public class RequestMsgVoice : RequestMsgBase
    {
        /// <summary>
        /// 语音消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 语音格式，如amr，speex等
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 语音识别结果，UTF8编码
        /// </summary>
        public string Recognition { get; set; }

        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }

        /// <summary>
        /// 初始化语音消息
        /// </summary>
        /// <param name="doc">XML消息</param>
        public RequestMsgVoice(XDocument doc)
            : base(RequestMsgType.Voice, doc)
        {
            MediaId = doc.Root.Element("MediaId").Value;
            Format = doc.Root.Element("Format").Value;
            if (doc.Root.Element("Recognition") != null)
            {
                Recognition = doc.Root.Element("Recognition").Value;
            }
            MsgId = Convert.ToInt64(doc.Root.Element("MsgId").Value);
        }
    }
}
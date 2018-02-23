using System;
using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class RequestMsgText : RequestMsgBase
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }

        /// <summary>
        /// 初始化文本消息
        /// </summary>
        /// <param name="doc">XML消息</param>
        public RequestMsgText(XDocument doc)
            : base(RequestMsgType.Text, doc)
        {
            Content = doc.Root.Element("Content").Value;
            MsgId = Convert.ToInt64(doc.Root.Element("MsgId").Value);
        }
    }
}

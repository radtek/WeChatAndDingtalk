using System;
using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 链接消息
    /// </summary>
    public class RequestMsgLink : RequestMsgBase
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }

        /// <summary>
        /// 初始化链接消息
        /// </summary>
        /// <param name="doc">XML消息</param>
        public RequestMsgLink(XDocument doc)
            : base(RequestMsgType.Link, doc)
        {
            Title = doc.Root.Element("Title").Value;
            Description = doc.Root.Element("Description").Value;
            Url = doc.Root.Element("Url").Value;
            MsgId = Convert.ToInt64(doc.Root.Element("MsgId").Value);
        }
    }
}
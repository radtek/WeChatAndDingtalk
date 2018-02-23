using System;
using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;
using Beisen.AppConnect.WeChatSDK.Helper;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 请求消息基类
    /// </summary>
    public class RequestMsgBase
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public RequestMsgType MsgType { get; set; }

        /// <summary>
        /// 初始化消息
        /// </summary>
        /// <param name="msgType">消息类型</param>
        /// <param name="doc">XML消息</param>
        public RequestMsgBase( RequestMsgType msgType, XDocument doc)
        {
            MsgType = msgType;
            ToUserName = doc.Root.Element("ToUserName").Value;
            FromUserName = doc.Root.Element("FromUserName").Value;
            CreateTime = DateTimeHelper.GetDateTimeFromXml(doc.Root.Element("CreateTime").Value);
        }
    }
}

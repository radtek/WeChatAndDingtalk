using System;
using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Request
{
    /// <summary>
    /// 模板消息事件
    /// </summary>
    public class RequestMsgEventTemplate : RequestMsgEventBase
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgID { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        public TemplateSendStatus Status { get; set; }

        /// <summary>
        /// 初始化模板消息事件
        /// </summary>
        /// <param name="doc">XML消息</param>
        public RequestMsgEventTemplate(XDocument doc)
            :base(RequestMsgEventType.TemplateSendJobFinish, doc)
        {
            MsgID = Convert.ToInt64(doc.Root.Element("MsgID").Value);
            Status = TemplateSendStatusHelper.GetEnum(doc.Root.Element("Status").Value);
        }
    }
}
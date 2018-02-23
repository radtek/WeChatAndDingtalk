using System;
using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;
using Beisen.AppConnect.WeChatSDK.Helper;

namespace Beisen.AppConnect.WeChatSDK.Handler.Response
{
    public class ResponseMsgBase
    {
        public string ToUserName { get; set; }

        public string FromUserName { get; set; }

        public DateTime CreateTime { get; set; }

        public ResponseMsgType MsgType { get; set; }

        public ResponseMsgBase(ResponseMsgType msgType)
        {
            MsgType = msgType;
        }

        protected XDocument GetXDocument()
        {
            var doc = new XDocument();
            doc.Add(new XElement("xml"));
            doc.Root.Add(new XElement("ToUserName", ToUserName));
            doc.Root.Add(new XElement("FromUserName", FromUserName));
            doc.Root.Add(new XElement("CreateTime", DateTimeHelper.GetWeixinDateTime(CreateTime)));
            doc.Root.Add(new XElement("MsgType", MsgType.ToString()));
            return doc;
        }

        public virtual string ToXml()
        {
            var doc = GetXDocument();
            return doc.ToString();
        }
    }
}

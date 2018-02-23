using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Response
{
    public class ResponseMsgText : ResponseMsgBase
    {
        public string Content { get; set; }

        public ResponseMsgText(string content)
            : base(ResponseMsgType.Text)
        {
            Content = content;
        }

        public override string ToXml()
        {
            var doc = GetXDocument();
            doc.Root.Add(new XElement("Content", Content));

            return doc.ToString();
        }
    }
}

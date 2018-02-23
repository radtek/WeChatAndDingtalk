using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Response
{
    public class ResponseMsgVoice : ResponseMsgBase
    {
        public string MediaId { get; set; }

        public ResponseMsgVoice(string mediaId)
            : base(ResponseMsgType.Voice)
        {
            MediaId = mediaId;
        }

        public override string ToXml()
        {
            var doc = GetXDocument();
            doc.Root.Add(new XElement("MediaId", MediaId));

            return doc.ToString();
        }
    }
}
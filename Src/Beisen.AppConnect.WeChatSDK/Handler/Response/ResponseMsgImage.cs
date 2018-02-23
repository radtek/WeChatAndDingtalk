using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Response
{
    public class ResponseMsgImage : ResponseMsgBase
    {
        public string MediaId { get; set; }

        public ResponseMsgImage(string mediaId)
            : base(ResponseMsgType.Image)
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
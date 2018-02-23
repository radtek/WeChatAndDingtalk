using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Response
{
    public class ResponseMsgVideo : ResponseMsgBase
    {
        public string MediaId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ResponseMsgVideo(string mediaId,string title, string description)
            : base(ResponseMsgType.Video)
        {
            MediaId = mediaId;
            Title = title;
            Description = description;
        }

        public override string ToXml()
        {
            var doc = GetXDocument();
            doc.Root.Add(new XElement("MediaId", MediaId));
            doc.Root.Add(new XElement("Title", Title));
            doc.Root.Add(new XElement("Description", Description));

            return doc.ToString();
        }
    }
}
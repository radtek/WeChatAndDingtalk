using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Response
{
    public class ResponseMsgMusic : ResponseMsgBase
    { 
        public string Title { get; set; }

        public string Description { get; set; }

        public string MusicUrl { get; set; }

        public string HQMusicUrl { get; set; }

        public string ThumbMediaId { get; set; }

        public ResponseMsgMusic(string title, string description,string musicUrl,string hqMusicUrl,string thumbMediaId)
            : base(ResponseMsgType.Music)
        {
            Title = title;
            Description = description;
            MusicUrl = musicUrl;
            HQMusicUrl = hqMusicUrl;
            ThumbMediaId = thumbMediaId;
        }

        public override string ToXml()
        {
            var doc = GetXDocument();
            doc.Root.Add(new XElement("Title", Title));
            doc.Root.Add(new XElement("Description", Description));
            doc.Root.Add(new XElement("MusicUrl", MusicUrl));
            doc.Root.Add(new XElement("HQMusicUrl", HQMusicUrl));
            doc.Root.Add(new XElement("ThumbMediaId", ThumbMediaId));

            return doc.ToString();
        }
    }
}
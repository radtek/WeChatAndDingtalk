using System.Collections.Generic;
using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Response
{
    public class ResponseMsgNews : ResponseMsgBase
    {
        public int ArticleCount { get; set; }

        public List<ResponseMsgNewsItem> Articles { get; set; }

        public ResponseMsgNews(int articleCount, List<ResponseMsgNewsItem> articles)
            : base(ResponseMsgType.News)
        {
            ArticleCount = articleCount;
            Articles = articles;
        }

        public override string ToXml()
        {
            var doc = GetXDocument();
            doc.Root.Add(new XElement("ArticleCount", ArticleCount));

            var atriclesElement = new XElement("Articles");
            foreach (var articale in Articles)
            {
                var item =new XElement("item");
                item.Add(new XElement("Title", articale.Title));
                item.Add(new XElement("Description", articale.Description));
                item.Add(new XElement("PicUrl", articale.PicUrl));
                item.Add(new XElement("Url", articale.Url));

                atriclesElement.Add(item);
            }
            doc.Root.Add(atriclesElement);

            return doc.ToString();
        }
    }

    public class ResponseMsgNewsItem
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string PicUrl { get; set; }

        public string Url { get; set; }
    }
}


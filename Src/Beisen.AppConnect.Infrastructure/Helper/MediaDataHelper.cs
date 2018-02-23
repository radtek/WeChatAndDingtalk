using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Beisen.AppConnect.Infrastructure.Helper
{
    public class MediaDataHelper
    {
#if DEBUG
        private static readonly string scheme = "https";
#else
        private static readonly string scheme =  HttpContext.Current.Request.Url.Scheme;
#endif
        private static readonly string imgUrlFormat = scheme + "://stnew03.beisen.com/ux/beisen-common/upaas-static/upaas/{0}.png";
        public static string GetPicUrlByTag(string tag)
        {
            string picUrl = "";
            if (string.IsNullOrEmpty(tag))
            {
                AppConnectLogHelper.Error("获取消息图片tag为空!");
                return picUrl;
            }

            var tags = tag.Split('_');
            if (tags.Length == 2)
            {
                string imgName = "italent_massage_03";//默认是微信
                if (tags[1] == "21")//如果是钉钉
                {
                    imgName = "italent_massage_04";
                }

                switch (tags[0])
                {
                    //审批中心
                    case "907": picUrl = string.Format(imgUrlFormat, imgName); break;
                    //核心人力
                    case "908": picUrl = string.Format(imgUrlFormat, imgName); break;
                    //假勤
                    case "909": picUrl = string.Format(imgUrlFormat, imgName); break;
                    //薪酬
                    case "960": picUrl = string.Format(imgUrlFormat, imgName); break;
                    default: picUrl = string.Format(imgUrlFormat, imgName); break;
                }
            }
            return picUrl;
        }
        public static string GetPicUrl(string productId, string messageType)
        {
            AppConnectLogHelper.DebugFormat("发消息图片地址：{0}", imgUrlFormat);
            string picUrl = "";
            if (string.IsNullOrEmpty(messageType))
            {
                AppConnectLogHelper.Error("获取消息图片产品Id为空!");
                return picUrl;
            }
            string imgName = "italent_massage_03";//默认是微信
            if (messageType == "21")//如果是钉钉
            {
                imgName = "italent_massage_04";
            }
            switch (productId)
            {
                //审批中心
                case "907": picUrl = string.Format(imgUrlFormat, imgName); break;
                //核心人力
                case "908": picUrl = string.Format(imgUrlFormat, imgName); break;
                //假勤
                case "909": picUrl = string.Format(imgUrlFormat, imgName); break;
                //薪酬
                case "960": picUrl = string.Format(imgUrlFormat, imgName); break;
                default: picUrl = string.Format(imgUrlFormat, imgName); break;
            }
            return picUrl;
        }
    }
}

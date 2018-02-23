using Beisen.AppConnect.Infrastructure.RequestUtility;
using Beisen.AppConnect.WeChatSDK.Helper;
using Beisen.AppConnect.WeChatSDK.Model;
using RestSharp;

namespace Beisen.AppConnect.WeChatSDK.Work
{
    /// <summary>
    /// 企业微信发消息
    /// </summary>
    public static class Message
    {
        /// <summary>
        /// 推送文本卡片消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="toUser"></param>
        /// <param name="agentId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="messageUrl"></param>
        /// <returns></returns>
        public static WorkMessageSendResult SendTextCard(string accessToken, string toUser, int agentId, string title, string description, string messageUrl, string mediaUrl = null)
        {
            var url = string.Format("{0}/cgi-bin/message/send?access_token={1}", HostHelper.HostWork, accessToken);

            var data = new
            {
                touser = toUser,
                toparty = "",
                msgtype = "textcard",
                agentid = agentId,
                textcard = new
                {
                    title = title,
                    description = description,
                    url = messageUrl,
                    picurl = mediaUrl,
                    btntxt = "查看详情"
                }
            };
            return Request.SendRequest<WorkMessageSendResult>(url, Method.POST, data);
        }

        /// <summary>
        /// 推送新闻消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="toUser"></param>
        /// <param name="agentId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="messageUrl"></param>
        /// <returns></returns>
        public static WorkMessageSendResult SendNews(string accessToken, string toUser, int agentId, string title, string description, string messageUrl, string mediaUrl = null)
        {
            var url = string.Format("{0}/cgi-bin/message/send?access_token={1}", HostHelper.HostWork, accessToken);

            var data = new
            {
                touser = toUser,
                toparty = "",
                msgtype = "news",
                agentid = agentId,
                news = new
                {
                    articles = new[]
                    {
                        new
                        {
                            title = title,
                            description = description,
                            url = messageUrl,
                            picurl = mediaUrl,
                            btntxt = ""
                        }
                    }
                }
            };
            return Request.SendRequest<WorkMessageSendResult>(url, Method.POST, data);
        }
    }
}

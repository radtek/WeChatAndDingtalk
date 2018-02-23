using Beisen.AppConnect.Infrastructure.RequestUtility;
using Beisen.AppConnect.WeChatSDK.Helper;
using Beisen.AppConnect.WeChatSDK.Model;
using RestSharp;

namespace Beisen.AppConnect.WeChatSDK.QY
{
    /// <summary>
    /// 企业微信发消息
    /// </summary>
    public static class Message
    {
        /// <summary>
        /// 发消息
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="message">json消息</param>
        /// <returns></returns>
        public static QYMessageSendResult SendForJson(string accessToken, string message)
        {
            var url = string.Format("{0}/cgi-bin/message/send?access_token={1}", HostHelper.HostQY, accessToken);
            return Request.SendRequest<QYMessageSendResult>(url, Method.POST, message);
        }

        /// <summary>
        /// 推送新闻消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <param name="agentId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="messageUrl"></param>
        /// <returns></returns>
        public static QYMessageSendResult SendNews(string accessToken, string userId, int agentId, string title, string description, string messageUrl, string mediaUrl = null)
        {
            var url = string.Format("{0}/cgi-bin/message/send?access_token={1}", HostHelper.HostQY, accessToken);

            var data = new
            {
                touser = userId,
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
                            picurl = mediaUrl
                        }
                    }
                }
            };
            return Request.SendRequest<QYMessageSendResult>(url, Method.POST, data);
        }
    }
}

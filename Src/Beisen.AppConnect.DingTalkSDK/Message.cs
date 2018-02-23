using System;
using Beisen.AppConnect.DingTalkSDK.Helper;
using Beisen.AppConnect.DingTalkSDK.Model;
using Beisen.AppConnect.Infrastructure.RequestUtility;
using RestSharp;

namespace Beisen.AppConnect.DingTalkSDK
{
    public static class Message
    {
        public static MessageSendResult SendLink(string accessToken, string toUser, string agentId, string messageUrl, string title, string text)
        {
            var url = string.Format("{0}/message/send?access_token={1}", HostHelper.HostDingTalk, accessToken);

            var data = new
            {
                touser = toUser,
                toparty = "",
                agentid = agentId,
                msgtype = "link",
                link = new
                {
                    messageUrl = messageUrl,
                    picUrl = "",
                    title = title,
                    text = text
                }
            };

            return Request.SendRequest<MessageSendResult>(url, Method.POST, data);
        }

        public static MessageSendResult SendOA(string accessToken, string toUser, string agentId, string messageUrl, string title, string description, string messagePcUrl = null, string mediaUrl = null)
        {
            var url = string.Format("{0}/message/send?access_token={1}", HostHelper.HostDingTalk, accessToken);

            var data = new
            {
                touser = toUser,
                toparty = "",
                agentid = agentId,
                msgtype = "oa",
                oa = new
                {
                    message_url = messageUrl,
                    pc_message_url = messagePcUrl,
                    //head =new
                    //{
                    //    bgcolor= "FFBBBBBB",
                    //    text=title
                    //},
                    body = new
                    {
                        title = title,
                        content = description,
                        image = mediaUrl
                    }
                }
            };

            return Request.SendRequest<MessageSendResult>(url, Method.POST, data);
        }

        /// <summary>
        /// 只是文本字
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="toUser"></param>
        /// <param name="agentId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="mediaUrl"></param>
        /// <returns></returns>
        public static MessageSendResult SendText(string accessToken, string toUser, string agentId, string title, string description, string mediaUrl = null)
        {
            var url = string.Format("{0}/message/send?access_token={1}", HostHelper.HostDingTalk, accessToken);

            var data = new
            {
                touser = toUser,
                toparty = "",
                agentid = agentId,
                msgtype = "text",
                text = new
                {
                    title = title,
                    content = description,
                    image = mediaUrl
                }
            };

            return Request.SendRequest<MessageSendResult>(url, Method.POST, data);
        }
    }
}

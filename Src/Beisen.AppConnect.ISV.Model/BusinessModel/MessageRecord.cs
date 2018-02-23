using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Model.BusinessModel
{
    public class MessageRecord
    {
        /// <summary>
        /// 发送批次Id
        /// </summary>
        public string BatchId { get; set; }
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 企业CorpId
        /// </summary>
        public string CorpId { get; set; }
        /// <summary>
        /// AgentId
        /// </summary>
        public string AgentId { get; set; }
        /// <summary>
        /// 产品Id
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// 租户Id
        /// </summary>
        public int ISVTenantId { get; set; }

        /// <summary>
        /// 消息发送者
        /// </summary>
        public int FromUser { get; set; }
        /// <summary>
        /// 消息发送者名称
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息接收者（BeisenUserId）
        /// </summary>
        public List<string> ToUserIds { get; set; }
        /// <summary>
        /// 消息接收者（BeisenUserId）
        /// </summary>
        public List<string> ReceiverUserIds { get; set; }
        /// <summary>
        /// 消息接收者（BeisenUserId）
        /// </summary>
        public List<string> NotReceiverUserIds { get; set; }
        /// <summary>
        /// 消息接收者（openId）
        /// </summary>
        public List<string> ToMappingUserId { get; set; }

        /// <summary>
        /// 消息内容标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// 消息内容Url
        /// </summary>
        public string ContentUrl { get; set; }
        /// <summary>
        /// 消息内容Url
        /// </summary>
        public string RedirectUrlMobile { get; set; }

        /// <summary>
        /// 消息内容Url
        /// </summary>
        public string RedirectUrlPC { get; set; }
        /// <summary>
        /// 图片Url
        /// </summary>
        public string ImageUrl { get; set; }

    }
}

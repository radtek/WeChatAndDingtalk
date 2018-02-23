using System.Collections.Generic;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceInterface.Model
{
    public class MessageInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 发送批次Id
        /// </summary>
        public string BatchId { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 消息发送者
        /// </summary>
        public int FromUser { get; set; }

        /// <summary>
        /// 消息接收者（BeisenUserId），多个接收者用|分隔
        /// </summary>
        public string ToUser { get; set; }

        /// <summary>
        /// 消息接收者（openId），多个接收者用|分隔
        /// </summary>
        public string ToOpenId { get; set; }

        /// <summary>
        /// 开放平台账户Id
        /// </summary>
        public string AppAccountId { get; set; }

        /// <summary>
        /// 服务号模板库模板Id
        /// </summary>
        public string TemplateIdShort { get; set; }

        /// <summary>
        /// 服务号模板Id
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public MessageContent Content { get; set; }

        /// <summary>
        /// 消息内容（Json格式）
        /// </summary>
        public string ContentJson { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        public MessageState State { get; set; }

        /// <summary>
        /// 消息Id
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// 发送结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 图片Url
        /// </summary>
        public string MediaUrl { get; set; }

        /// <summary>
        /// 消息类型：微信，钉钉等等
        /// </summary>
        public List<int> MessageTypeList { get; set; }
        /// <summary>
        /// 产品Id
        /// </summary>
        public string ProductId { get; set; }
    }

    /// <summary>
    /// 消息内容
    /// </summary>
    public class MessageContent
    {
        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// PC消息链接
        /// </summary>
        public string DingTalkPCUrl { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息明细
        /// </summary>
        public List<MessageContentDetail> Detail { get; set; }
    }

    /// <summary>
    /// 消息明细
    /// </summary>
    public class MessageContentDetail
    {
        /// <summary>
        /// 服务后模板消息Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Key的明细描述
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 明细内容
        /// </summary>
        public string Value { get; set; }
    }
}

using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.ServiceInterface.Model
{
    /// <summary>
    /// 模板消息
    /// </summary>
    public class WeChatTemplateMessageInfo
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set;}

        /// <summary>
        /// 微信消息Id
        /// </summary>
        public long MsgId { get; set;}

        /// <summary>
        /// 用户OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 公众号原始Id
        /// </summary>
        public string SourceId { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        public TemplateSendStatus Status { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}

using System;

namespace Beisen.AppConnect.WeChatSDK.Enum
{
    /// <summary>
    /// 模板消息发送状态
    /// </summary>
    public enum TemplateSendStatus
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,

        /// <summary>
        /// 成功
        /// </summary>
        Success=1,

        /// <summary>
        /// 用户拒绝
        /// </summary>
        UserBlock=2,

        /// <summary>
        /// 系统错误
        /// </summary>
        SystemFailed=3
    }

    /// <summary>
    /// 模板消息发送状态帮助
    /// </summary>
    public class TemplateSendStatusHelper
    {
        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <param name="status">状态字符串</param>
        /// <returns>枚举</returns>
        public static TemplateSendStatus GetEnum(string status)
        {
            var value = status.ToUpper();
            TemplateSendStatus result;
            switch (value)
            {
                case "SUCCESS":
                    result = TemplateSendStatus.Success;
                    break;
                case "FAILED:USER BLOCK":
                    result = TemplateSendStatus.UserBlock;
                    break;
                case "FAILED:SYSTEM FAILED":
                    result = TemplateSendStatus.SystemFailed;
                    break;
                default:
                    throw new ArgumentException("模板消息发送状态无效：status="+ status);
            }
            return result;
        }
    }
}

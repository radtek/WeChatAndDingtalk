using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Model.BusinessEnum
{
    public class AppconnectEnum
    {
    }
    public enum ActivateStatus
    {
        /// <summary>
        /// 已激活
        /// </summary>
        Activated = 1,
        /// <summary>
        /// 未激活
        /// </summary>
        Inactive = 2,
    }
    public enum UserType
    {
        /// <summary>
        /// 工具导入
        /// </summary>
        Import = 4
    }
    public enum AccountType
    {
        /// <summary>
        /// 登录绑定
        /// </summary>
        Login = 0,

        /// <summary>
        /// 邮件绑定
        /// </summary>
        Email = 1,

        /// <summary>
        /// 手机绑定
        /// </summary>
        Phone = 2
    }
    public enum DingTalkLoginType
    {
        /// <summary>
        /// 移动端
        /// </summary>
        Mobil = 0,
        /// <summary>
        /// PC端
        /// </summary>
        PC = 1,

    }

    public enum CorpType
    {
        ISV = 1,
        Corp = 2
    }

    public enum MappingType
    {
        /// <summary>
        /// 钉钉
        /// </summary>
        DingTalk = 21
    }
    public enum MessageState
    {
        Unsent = 0,

        Sent = 1,

        Success = 2,

        Failed = 3
    }
    /// <summary>
    /// 解决状态
    /// </summary>
    public enum ResolutionState
    {
        /// <summary>
        /// 未解决
        /// </summary>
        Unsolved = 0,
        /// <summary>
        /// 已解决
        /// </summary>
        Resolved = 1,
        /// <summary>
        /// 无需处理
        /// </summary>
        Ignore = 2
    }
    /// <summary>
    /// 是否存在异常
    /// </summary>
    public enum IsException
    {
        Unknown = 0,
        False = 1,
        True = 2
    }

}

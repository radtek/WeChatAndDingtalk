using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceInterface.Model
{
    /// <summary>
    /// 开放平台帐号信息
    /// </summary>
    public class AppAccountInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 多租赁Id
        /// </summary>
        public string AppAccountId { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 账户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 账户类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 原始ID
        /// </summary>
        public string SourceId { get; set; }

        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 企业应用的id
        /// </summary>
        public string AgentId { get; set; }

        /// <summary>
        /// 账户状态
        /// </summary>
        public AppAccountState State { get; set; }

        public string Tag { get; set; }

        public int CreateBy { get; set; }

        public int ModifyBy { get; set; }
    }
}

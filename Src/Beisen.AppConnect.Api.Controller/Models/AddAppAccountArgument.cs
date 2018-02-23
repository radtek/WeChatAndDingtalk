namespace Beisen.AppConnect.Api.Controller.Models
{
    public class AddAppAccountArgument
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public int tenant_id { get; set; }

        /// <summary>
        /// 账户名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 账户类型
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 原始ID
        /// </summary>
        public string source_id { get; set; }

        /// <summary>
        /// AppId
        /// </summary>
        public string app_id { get; set; }

        /// <summary>
        /// AppSecret
        /// </summary>
        public string app_secret { get; set; }

        /// <summary>
        /// 企业应用的id
        /// </summary>
        public string agent_id { get; set; }

        /// <summary>
        /// 账户标签
        /// </summary>
        public string tag { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int create_by { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public int modify_by { get; set; }
    }
}

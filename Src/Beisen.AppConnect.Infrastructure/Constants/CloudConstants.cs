using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Infrastructure.Constants
{
    public static class CloudConstants
    {
        #region 多租赁最大数限制
        public const int MaxQueryRowLimit = 100;
        public const int MaxQueryColumnLimit = 10000;
        #endregion

        #region 多租赁查询默认值
        public const int DefaultQueryRowCount = 10;
        #endregion

        #region 多租赁默认字段
        public const string LogicalDelete = "StdIsDeleted";
        #endregion

    }
    public static class AppUserConstants
    {
        public const string ApplicationName = "AppConnect";
        public const string MetaName = "AppConnect.AppUser";
        public const string AppUser_OpenId = "AppConnect.AppUser.OpenId";
        public const string AppUser_AppId = "AppConnect.AppUser.AppId";
        public const string AppUser_UserId = "AppConnect.AppUser.UserId";
        public const string AppUser_StdIsDelete = "AppConnect.AppUser.StdIsDelete";
    }
    public static class UserInfoMappingConstants
    {
        public const string ApplicationName = "AppConnect";
        public const string MetaName = "AppConnect.UserInfoMapping";
        public const string UserInfo_StaffId = "AppConnect.UserInfoMapping.StaffId";
        public const string UserInfo_SuiteKey = "AppConnect.UserInfoMapping.SuiteKey";
        public const string UserInfo_ISVTenantId = "AppConnect.UserInfoMapping.ISVTenantId";
        public const string UserInfo_Status = "AppConnect.UserInfoMapping.Status";
        public const string UserInfo_CorpId = "AppConnect.UserInfoMapping.CorpId";
        public const string UserInfo_AppId = "AppConnect.UserInfoMapping.AppId";
    }

    public static class AppAccount
    {
        public const string ApplicationName = "AppAccount";
        public const string MetaName = "AppConnect.AppAccount";
        public const string AppAccount_AgentId = "AppConnect.AppAccount.AgentId";
        public const string AppAccount_AppSecret = "AppConnect.AppAccount.AppSecret";
    }
}

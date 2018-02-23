using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Model
{
    public class AppconnectConst
    {
        #region UserInfoMapping
        public const string UserInfoMappingMetaName = "AppConnect.UserInfoMapping";
        public const string UserInfoMapping_SuiteKey = UserInfoMappingMetaName + ".SuiteKey";
        public const string UserInfoMapping_ISVTenantId = UserInfoMappingMetaName + ".ISVTenantId";
        public const string UserInfoMapping_UserId = UserInfoMappingMetaName + ".UserId";
        public const string UserInfoMapping_StaffId = UserInfoMappingMetaName + ".StaffId";
        public const string UserInfoMapping_UserType = UserInfoMappingMetaName + ".UserType";
        public const string UserInfoMapping_DingTalkUserId = UserInfoMappingMetaName + ".MappingUserId";
        public const string UserInfoMapping_CorpId = UserInfoMappingMetaName + ".CorpId";
        public const string UserInfoMapping_Status = UserInfoMappingMetaName + ".Status";
        #endregion

        #region Corp
        public const string CorpMetaName = "AppConnect.Corp";
        public const string CorpSuiteKey = "AppConnect.Corp.SuiteKey";
        public const string CorpCorpId = "AppConnect.Corp.CorpId";
        public const string CorpStdIsDeleted = "AppConnect.Corp.StdIsDeleted";
        #endregion

        #region MessageRecord
        public const string MessageRecordMetaName = "AppConnect.MessageRecord";
        public const string MessageRecordStdIsDeleted = "AppConnect.MessageRecord.StdIsDeleted";
        public const string MessageRecordSuiteKey = "AppConnect.MessageRecord.SuiteKey";
        public const string MessageRecordCorpId = "AppConnect.MessageRecord.CorpId";
        #endregion

        #region AppAccount
        public const string AppAccountMetaName = "AppConnect.AppAccount";
        public const string AppAccountId = "AppConnect.AppAccount.AppAccountId";
        public const string Type = "AppConnect.AppAccount.Type";
        #endregion
    }
}

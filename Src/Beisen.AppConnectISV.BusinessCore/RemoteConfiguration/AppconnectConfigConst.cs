using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore
{
    public static class AppconnectConfigConst
    {
        #region NodeType
        public const string Redis = "Redis";
        public const string ISV = "ISV";
        public const string DingDingApi = "DingDingApi";
        public const string TitaApi = "TitaApi";
        public const string RedisKey = "RedisKey";
        public const string Domain = "Domain";

        #endregion

        #region NodeType Include Key

        public const string Redis_KeySpace = "KeySpace";
        public const string Redis_ISVTenantId = "ISV_TenantId";
        public const string Redis_Expires = "Expires";

        public const string ISV_SystemTenantId = "ISVSystemTenantId";
        public const string ISV_SystemUserId = "ISVSystemUserId";
        public const string ISV_Token = "Token";
        public const string ISV_SuitSecret = "SuitSecret";
        public const string ISV_SuiteKey = "SuiteKey";
        public const string ISV_EncodingAesKey = "EncodingAesKey";
        public const string ISV_AuthUrl = "AuthUrl";


        public const string DingDingApi_SuiteAccessToken = "SuiteAccessToken";
        public const string DingDingApi_PermanentCode = "Permanent_Code";
        public const string DingDingApi_ActivateSuite = "Activate_Suite";
        public const string DingDingApi_CorpToken = "Corp_Token";
        public const string DingDingApi_JsApiTicket = "JsApiTicket";
        public const string DingDingApi_AuthCorpInfo = "AuthCorpInfo";
        public const string DingDingApi_UserInfo = "UserInfo";
        public const string DingDingApi_Message = "Message";
        public const string DingDingApi_AsyncMessage = "AsyncMessage";

        public const string TitaApi_TitaAuth = "TitaAuth";
        public const string TitaApi_TitaAuthPC = "TitaAuthPC";


        public const string RedisKey_SuiteTicket = "Suite_Ticket";
        public const string RedisKey_SuiteAccessToken = "Suite_SuiteAccessToken";
        public const string RedisKey_SuiteTmpAuthCode = "Suite_TmpAuthCode";
        public const string RedisKey_CorpPermanentCode = "Corp_PermanentCode";
        public const string RedisKey_CorpToken = "Corp_CorpToken";
        public const string RedisKey_CorpJsApiTicket = "Corp_JsApiTicket";
        public const string RedisKey_AuthCorpInfo = "Corp_AuthCorpInfo";

        public const string Domain_Cloud = "Cloud";
        public const string Domain_Appconnect = "Appconnect";

        #endregion


    }
}

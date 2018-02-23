using Beisen.AppConnectISV.BusinessCore;
using Beisen.AppConnectISV.BusinessCore.DingDing;
using Beisen.AppConnectISV.BusinessCore.RemoteConfiguration;
using Beisen.AppConnectISV.Infrastructure;
using Beisen.AppConnectISV.Model;
using Beisen.AppConnectISV.Model.BusinessModel;
using Beisen.AppConnectISV.Model.HttpModel;
using Beisen.AppConnectISV.ServiceImp.CoreProvider;
using Beisen.MultiTenant.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.ServiceImp
{
    public class ISVLoginProvider
    {
        #region Singleton 
        static readonly ISVLoginProvider _Instance = new ISVLoginProvider();
        public static ISVLoginProvider Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion
        public string GetTitaSign(int tenantId, int userId, int appId, string appKey)
        {
            var titaSgin = TitaSign.Instance.GetSgin(tenantId, userId, appId, appKey);
            return titaSgin;
        }
        public SignResult GetTitaSsoSignV2(int tenantId, int userId, int titaAppId)
        {
            SignResult signResult = new SignResult();
            try
            {
                var siginType = 1;
                var timeStamp = DateTimeCryptography.ConvertToUnixTimeStamp(DateTime.Now);
                var sign = TitaSign.Instance.GetSginV2(tenantId, userId, titaAppId, timeStamp, siginType);
                var ssoSign = string.Format("tenant_id={0}&user_id={1}&app_id={2}&time_stamp={3}&sign={4}&sign_type={5}", tenantId, userId, titaAppId, timeStamp, sign, siginType);
                signResult.SsoSign = ssoSign;
            }
            catch (Exception ex)
            {
                signResult.SsoSign = string.Empty;
                signResult.ErrorMsg = ex.Message;
            }
            return signResult;
        }
        public bool VerifyIsActivate()
        {
            return ISVLoginCoreProvider.Instance.VerifyIsActivate();
        }
        public JsApiInfo GetJsApiInfo(string corpId, string appId, string currentUrl)
        {
            var jsApiInfo = ISVLoginCoreProvider.Instance.GetJsApiInfo(corpId, appId, currentUrl);
            return jsApiInfo;
        }
        public AccountErrorInfo Login(string userName, string password)
        {
            var info = ISVLoginCoreProvider.Instance.Login(userName, password);
            return info;
        }
        public void GetDingTaklUserInfo(string corpId, string code)
        {
            ISVLoginCoreProvider.Instance.GetDingTaklUserInfo(corpId, code);
        }
        public string GetTitaAuthUrl()
        {
            return TitaApiUrl.TitaAuth;
        }
        public string GetTitaAuthPCUrl()
        {
            return TitaApiUrl.TitaAuthPC;
        }
        public void SaveUserInfo(AccountLoginInfo userInfoCookie)
        {
            ISVLoginCoreProvider.Instance.SaveUserInfo(userInfoCookie);

        }
        public ObjectData CurrentUserInfo(AccountLoginInfo accountLoginInfo, DingTalkUserInfo dingTalkUserInfoCookie)
        {
            return ISVLoginCoreProvider.Instance.CurrentUserInfo(accountLoginInfo, dingTalkUserInfoCookie);
        }
        public ObjectData GetSystemUserInfo(string corpId, string mappingUserId)
        {
            return ISVLoginCoreProvider.Instance.GetSystemUserInfo(corpId, mappingUserId);
        }
        public List<ObjectData> GetSystemUserInfos(int tenantId, string[] userIds)
        {
            return ISVLoginCoreProvider.Instance.GetSystemUserInfos(tenantId, userIds);
        }
        public List<ObjectData> GetSystemUserInfosForDefault(int tenantId, string[] userIds)
        {
            return ISVLoginCoreProvider.Instance.GetSystemUserInfosOrDefault(tenantId, userIds);
        }
        public string GetCloudDomain()
        {
            return DomainInfo.Instance.CloudHomePage;
        }
    }
}

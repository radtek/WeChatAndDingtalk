using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore
{
    public class ISVInfo
    {
        #region Singleton 
        static readonly ISVInfo _Instance = new ISVInfo();
        public static ISVInfo Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion

        public static int ISVSystemTenantId
        {
            get
            {
                var tenantId = AppconnectConfig.GetAppconnectConfigInfo<int>(AppconnectConfigConst.ISV, AppconnectConfigConst.ISV_SystemTenantId);
                if (tenantId <= 0)
                {
                    throw new Exception(string.Format("未在远程配置中获取ISVSystemTenantId{0}", tenantId));
                }
                return tenantId;
            }
        }
        public static int ISVSystemUserId
        {
            get
            {
                var tenantId = AppconnectConfig.GetAppconnectConfigInfo<int>(AppconnectConfigConst.ISV, AppconnectConfigConst.ISV_SystemUserId);
                if (tenantId <= 0)
                {
                    throw new Exception(string.Format("未在远程配置中获取ISVSystemUserId{0}", tenantId));
                }
                return tenantId;
            }
        }
        public static string Token
        {
            get
            {
                string token = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.ISV, AppconnectConfigConst.ISV_Token);
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new Exception(string.Format("未在远程配置中获取Token{0}", token));
                }
                return token;
            }
        }
        public static string SuiteKey
        {
            get
            {
                string suiteKey = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.ISV, AppconnectConfigConst.ISV_SuiteKey);
                if (string.IsNullOrWhiteSpace(suiteKey))
                {
                    throw new Exception(string.Format("未在远程配置中获取SuiteKey{0}", suiteKey));
                }
                return suiteKey;
            }
        }
        public static string SuitSecret
        {
            get
            {
                string suitSecret = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.ISV, AppconnectConfigConst.ISV_SuitSecret);
                if (string.IsNullOrWhiteSpace(suitSecret))
                {
                    throw new Exception(string.Format("未在远程配置中获取suitSecret{0}", suitSecret));
                }
                return suitSecret;
            }
        }
        public static string EncodingAesKey
        {
            get
            {
                string encodingAesKey = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.ISV, AppconnectConfigConst.ISV_EncodingAesKey);
                if (string.IsNullOrWhiteSpace(encodingAesKey))
                {
                    throw new Exception(string.Format("未在远程配置中获取EncodingAesKey{0}", encodingAesKey));
                }
                return encodingAesKey;
            }
        }


        private static string AuthUrl
        {
            get
            {
                var authRedirectUrl = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.ISV, AppconnectConfigConst.ISV_AuthUrl);
                if (string.IsNullOrWhiteSpace(authRedirectUrl))
                {
                    throw new Exception(string.Format("未在远程配置中获取redirectUrl_Mobil{0}", authRedirectUrl));
                }
                return authRedirectUrl;
            }
        }
        public static string GetBusinessRedirectUrl(string corpId, string appId, int logType, string businessRedirectUrl, string titaAppId)
        {
            var redirectUrl = string.Format(AuthUrl, corpId, appId, logType, businessRedirectUrl, titaAppId);
            return redirectUrl;
        }
    }
}

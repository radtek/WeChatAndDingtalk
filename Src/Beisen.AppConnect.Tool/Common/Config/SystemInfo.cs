using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Tool.Common.Config
{
    public class SystemInfo
    {
        #region Singleton 
        static readonly SystemInfo _Instance = new SystemInfo();
        public static SystemInfo Instance
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
                var tenantId = AppconnectConfig.GetAppconnectConfigInfo<int>("ISV", "ISVSystemTenantId");
                if (tenantId <= 0)
                {
                    throw new Exception(string.Format("未在远程配置中获取ISVSystemTenantId{0}", tenantId));
                }
                return tenantId;
            }
        }

        public static string SuiteKey
        {
            get
            {
                string suiteKey = AppconnectConfig.GetAppconnectConfigInfo<string>("ISV", "SuiteKey");
                if (string.IsNullOrWhiteSpace(suiteKey))
                {
                    throw new Exception(string.Format("未在远程配置中获取SuiteKey{0}", suiteKey));
                }
                return suiteKey;
            }
        }

        public static string AppId
        {
            get
            {
                string appId = AppconnectConfig.GetAppconnectConfigInfo<string>("DingTalkApplication", "AppId");
                if (string.IsNullOrWhiteSpace(appId))
                {
                    throw new Exception(string.Format("未在远程配置中获取SuiteKey{0}", appId));
                }
                return appId;
            }
        }
    }
}

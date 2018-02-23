using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore.RemoteConfiguration
{
    public class DomainInfo
    {
        #region Singleton 
        static readonly DomainInfo _Instance = new DomainInfo();
        public static DomainInfo Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion

        public string CloudHomePage
        {
            get
            {
                string domain_Cloud = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.Domain, AppconnectConfigConst.Domain_Cloud);
                if (string.IsNullOrWhiteSpace(domain_Cloud))
                {
                    throw new Exception(string.Format("未在远程配置中获取Domain_Cloud{0}", domain_Cloud));
                }
                return domain_Cloud;
            }
        }
        public string AppconnectHomePage
        {
            get
            {
                string domain_Cloud = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.Domain, AppconnectConfigConst.Domain_Appconnect);
                if (string.IsNullOrWhiteSpace(domain_Cloud))
                {
                    throw new Exception(string.Format("未在远程配置中获取Domain_Cloud{0}", domain_Cloud));
                }
                return domain_Cloud;
            }
        }

    }
}

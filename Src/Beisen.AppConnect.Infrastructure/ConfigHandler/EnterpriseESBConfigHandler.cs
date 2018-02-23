using Beisen.AppConnect.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Infrastructure.ConfigHandler
{
    public class EnterpriseESBConfigHandler
    {
        static readonly int defaultThreshold = 1000;
        #region Singleton 
        static readonly EnterpriseESBConfigHandler _Instance = new EnterpriseESBConfigHandler();
        public static EnterpriseESBConfigHandler Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion

        public int ImportAsyncThreshold
        {
            get
            {
                var threshold = AppconnectConfig.GetAppconnectConfigInfo<int>("EnterpriseESB", "ImportAsyncThreshold");
                if (threshold <= 0)
                {
                    AppConnectLogHelper.Error(string.Format("未在远程配置EnterpriseESB Type下获取ImportAsyncThreshold{0}", threshold));
                    return defaultThreshold;
                }
                return threshold;
            }
        }
    }
}

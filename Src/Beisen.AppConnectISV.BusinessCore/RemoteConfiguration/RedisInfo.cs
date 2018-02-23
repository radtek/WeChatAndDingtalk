using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore
{
    public class RedisInfo
    {
        #region Singleton 
        static readonly RedisInfo _Instance = new RedisInfo();
        public static RedisInfo Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion
        public static int Expires
        {
            get
            {
                var expires = AppconnectConfig.GetAppconnectConfigInfo<int>(AppconnectConfigConst.Redis, AppconnectConfigConst.Redis_Expires);
                if (expires <= 0)
                {
                    throw new Exception(string.Format("未在远程配置中获取Expires{0}", expires));
                }
                return expires;
            }
        }
        public static string KeySpace
        {
            get
            {
                var keySpace = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.Redis, AppconnectConfigConst.Redis_KeySpace);
                if (string.IsNullOrWhiteSpace(keySpace))
                {
                    throw new Exception(string.Format("未在远程配置中获取KeySpace{0}", keySpace));
                }
                return keySpace;
            }
        }
        public static int ISVTenantId
        {
            get
            {
                var isvTenantId = AppconnectConfig.GetAppconnectConfigInfo<int>(AppconnectConfigConst.Redis, AppconnectConfigConst.Redis_ISVTenantId);
                if (isvTenantId <= 0)
                {
                    throw new Exception(string.Format("未在远程配置中获取ISVTenantId{0}", isvTenantId));
                }
                return isvTenantId;
            }
        }
    }
}

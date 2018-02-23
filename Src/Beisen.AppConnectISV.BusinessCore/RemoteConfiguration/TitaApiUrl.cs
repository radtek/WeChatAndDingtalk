using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore
{
    public class TitaApiUrl
    {
        #region Singleton 
        static readonly TitaApiUrl _Instance = new TitaApiUrl();
        public static TitaApiUrl Instance
        {
            get
            {
                return _Instance;
            }
        }
        #endregion
        public static string TitaAuth
        {
            get
            {
                var titaAuthUrl = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.TitaApi, AppconnectConfigConst.TitaApi_TitaAuth);
                if (string.IsNullOrWhiteSpace(titaAuthUrl))
                {
                    throw new Exception(string.Format("未在远程配置中获取TitaApiUrl{0}", titaAuthUrl));
                }
                return titaAuthUrl;
            }
        }
        public static string TitaAuthPC
        {
            get
            {
                var titaAuthUrl = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.TitaApi, AppconnectConfigConst.TitaApi_TitaAuthPC);
                if (string.IsNullOrWhiteSpace(titaAuthUrl))
                {
                    throw new Exception(string.Format("未在远程配置中获取TitaAuthPCUrl{0}", titaAuthUrl));
                }
                return titaAuthUrl;
            }
        }
    }
}

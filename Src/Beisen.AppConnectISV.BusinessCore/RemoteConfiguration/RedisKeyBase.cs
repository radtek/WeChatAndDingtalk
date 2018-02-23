using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore
{
    public class RedisKeyBase
    {
        public static string _SuiteKey = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.ISV, AppconnectConfigConst.ISV_SuiteKey);
    }
}

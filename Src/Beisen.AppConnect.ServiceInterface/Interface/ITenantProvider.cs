using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface ITenantProvider
    {
        /// <summary>
        /// 获取租户logo
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        string GetTenantLogo(int tenantId, string isvId = "10002");
    }
}

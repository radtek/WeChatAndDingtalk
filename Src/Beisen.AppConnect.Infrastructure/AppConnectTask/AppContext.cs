using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.Common.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Infrastructure.AppConnectTask
{
    public class AppContext
    {
        public static void Set(int tenantId, int userId, string app = "AppConnect")
        {
            if (tenantId < 0)
            {
                AppConnectLogHelper.Error("tenantId 必须是正数");
                return;
            }
            if (userId < 0)
            {
                AppConnectLogHelper.Error("userId 必须是正数");
                return;
            }
            if (string.IsNullOrEmpty(app))
            {
                AppConnectLogHelper.Error("appName 不能为空");
                return;
            }
            ApplicationContext.Current.TenantId = tenantId;
            ApplicationContext.Current.UserId = userId;
            ApplicationContext.Current.ApplicationName = app;
        }
    }
}

using System.Web;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;

namespace Beisen.AppConnect.Web.WebSite
{
    public static class RuntimeContext
    {
        public static string AppAccountId
        {
            get
            {
                if (!HttpContext.Current.Items.Contains("AppAccountId"))
                {
                    var appAccountId = HttpContext.Current.Request["appaccount_id"];
                    var tenantId= HttpContext.Current.Request["tenant_id"];
                    var tag = HttpContext.Current.Request["tag"];

                    HttpContext.Current.Items["AppAccountId"] = ProviderGateway.AppAccountProvider.GetAppAccountId(ConvertHelper.ToInt(tenantId), appAccountId, tag);
                }
                return HttpContext.Current.Items["AppAccountId"].ToString();
            }
        }
    }
}
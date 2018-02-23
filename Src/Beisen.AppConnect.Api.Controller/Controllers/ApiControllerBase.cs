using System.Web;
using System.Web.Http;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.Logging;

namespace Beisen.AppConnect.Api.Controller.Controllers
{
    public class ApiControllerBase : ApiController
    {
        protected static string _AppAccountId
        {
            get
            {
                var appaccount_id = HttpContext.Current.Request["appaccount_id"];
                var tenantId = HttpContext.Current.Request["tenant_id"];
                var tag = HttpContext.Current.Request["tag"];
                if (!HttpContext.Current.Items.Contains("AppAccountId"))
                {
                    var appAccountId = ProviderGateway.AppAccountProvider.GetAppAccountId(ConvertHelper.ToInt(tenantId), appaccount_id, tag);
                    HttpContext.Current.Items["AppAccountId"] = appAccountId;
                }


                if (HttpContext.Current.Items["AppAccountId"] != null && !string.IsNullOrWhiteSpace(HttpContext.Current.Items["AppAccountId"].ToString()))
                {
                    return HttpContext.Current.Items["AppAccountId"].ToString();
                }
                else
                {
                    AppConnectLogHelper.DebugFormat("ApiControllerBase获取AppAccountId为空! 参数appaccount_id:{0},tenantId:{1}：tag:{2};", appaccount_id, tenantId, tag);
                    return null;
                }
            }
        }
    }
}

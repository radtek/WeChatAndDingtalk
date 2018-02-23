using System.Web.Http;
using Beisen.AppConnect.Api.Controller.Models;
using Beisen.AppConnect.ServiceInterface;
using Beisen.Logging;
using Newtonsoft.Json;
using Beisen.AppConnect.Infrastructure.Helper;

namespace Beisen.AppConnect.Api.Controller.Controllers
{
    public class TokenController : ApiControllerBase
    {
        [HttpGet]
        public ApiResult Get(int tenant_id, string appaccount_id = "", string tag = "")
        {
            try
            {
                var appAccountId = _AppAccountId;
                var token = ProviderGateway.TokenProvider.GetToken(tenant_id, appAccountId);
                return new TokenResult
                {
                    AccessToken = token
                };
            }
            catch (System.Exception ex)
            {
                AppConnectLogHelper.Debug("进入TokenController异常方法" + JsonConvert.SerializeObject(ex));
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Web.Http;
using Beisen.AppConnect.Api.Controller.Models;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;

namespace Beisen.AppConnect.Api.Controller.Controllers
{
    public class QrCodeController:ApiControllerBase
    {
        [HttpPost]
        public ApiResult Create([FromUri]int tenant_id = 0, [FromUri]string appaccount_id = "", [FromUri]int type = 12, [FromUri]int app_id = 100, [FromUri]int size= 320)
        {
            AppConnectLogHelper.DebugFormat("调用方法--tenant_id:{0},appaccount_id:{1},type:{2},app_id:{3},size:{4}", tenant_id, appaccount_id, type, app_id, size);
            var code = ProviderGateway.QrCodeLoginProvider.GenerateQrCode(app_id);
            if (string.IsNullOrWhiteSpace(code))
            {
                return new ApiResult
                {
                    ErrCode = 2001,
                    ErrMsg = "未开通应用"
                };
            }

            var parameters = new Dictionary<string, string>();
            parameters.Add("code", code);
            parameters.Add("size", size.ToString());
            parameters.Add("tenant_id", tenant_id.ToString());
            if (!string.IsNullOrWhiteSpace(_AppAccountId))
            {
                parameters.Add("appaccount_id", _AppAccountId);
            }
            parameters.Add("type", type.ToString());
            AppConnectLogHelper.DebugFormat("二维码parameters:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(parameters));
            var imageUrl = Infrastructure.Helper.UrlHelper.AddParameter(AppConnectHostConfig.Cache[0].Remove(0,5) + HostConst.Image, parameters);
            AppConnectLogHelper.DebugFormat("二维码imageUrl:{0}", imageUrl);
            var result = new QrCodeImageResult
             {
                 QrCode = code,
                 ImageUrl = imageUrl
             };
            AppConnectLogHelper.DebugFormat("二维码返回数据:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(result));
            return result;
        }

        [HttpGet]
        public ApiResult State(string code, int tenant_id = 0)
        {
            var qrCodeLoginInfo = ProviderGateway.QrCodeLoginProvider.GetAndUpdateByCode(code);

            return new QrCodeStateResult
            {
                State = (int) qrCodeLoginInfo.State,
                TenantId = qrCodeLoginInfo.TenantId,
                UserId = qrCodeLoginInfo.UserId,
            };
        }
    }
}

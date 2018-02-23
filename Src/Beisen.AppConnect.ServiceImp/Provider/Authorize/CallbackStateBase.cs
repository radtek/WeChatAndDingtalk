using System.Collections.Generic;
using Beisen.AppConnect.Infrastructure.Exceptions;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    internal abstract class CallbackStateBase
    {
        internal abstract string GetRedirectUrl(CallbackContentInfo contentInfo, string code, string state);

        protected string GetOpenId(AppAccountInfo appAccount, string code)
        {
            var extend = new Dictionary<string, string>
            {
                {TemplateConst.ExtendCode, code},
                {TemplateConst.ExtendToken, ProviderGateway.TokenProvider.GetToken(appAccount)}
            };

            var requestTemplate = new DefaultApiTemplate(appAccount, TemplateConst.GetOpenId, extend);
            //TODO:这块会出现空引用需要加一下判断
            var openId = requestTemplate.GetResponse()[TemplateConst.OpenId];

            if (string.IsNullOrWhiteSpace(openId))
            {
                var message = string.Format("未获取到OpenId：tenantId={0},appAccountId={1},code={2}", appAccount.TenantId, appAccount.Id, code);
                AppConnectLogHelper.Error(message);
                throw new SDKResultException(message);
            }

            var cookie = CookieHelper.GetCookie();
            if (cookie.OpenIds == null)
            {
                cookie.OpenIds = new Dictionary<string, string>();
            }
            cookie.OpenIds.Add(appAccount.AppId, openId);
            CookieHelper.SetCookie(cookie);

            return openId;
        }

        protected void SetNextState(string batchId, CallbackContentState state)
        {
            ProviderGateway.CallbackContentProvider.UpdateState(batchId, state);
        }
    }
}

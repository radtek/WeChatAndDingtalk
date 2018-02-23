using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.ServiceInterface;

namespace Beisen.AppConnect.Web.WebSite.Controllers
{
    public class MenuController : BaseController
    {
        // GET: Menu
        public ActionResult Custom(string id)
        {
            var menu = ProviderGateway.MenuProvider.Get(id);

            //// 单独处理钉钉
            //var appAccount = ProviderGateway.AppAccountProvider.Get(menu.AppAccountId);
            //if (appAccount.Type == 21)
            //{
            //    var cookie = CookieHelper.GetCookie();
            //    var currentUrl = Request.Url.AbsoluteUri;
            //    if (string.IsNullOrWhiteSpace(CookieHelper.GetOpenId(cookie, appAccount.AppId)))
            //    {
            //        var batchId = Guid.NewGuid().ToString();
            //        var callbackContent = new CallbackContentInfo
            //        {
            //            BatchId = batchId,
            //            TenantId = appAccount.TenantId,
            //            AppAccountPublic = appAccount.AppAccountId,
            //            AppAccountPrivate = appAccount.AppAccountId,
            //            Content = currentUrl,
            //            State = CallbackContentState.Private
            //        };
            //        ProviderGateway.CallbackContentProvider.Add(callbackContent);

            //        var state = Guid.NewGuid().ToString("N");
            //        CookieHelper.SetState(state);

            //        var jsApiConfig = ProviderGateway.JsApiProvider.GetJsApiConfig(batchId, currentUrl);
            //        jsApiConfig.State = state;
            //        jsApiConfig.RedirectUri = AppConnectHostConfig.Cache[0] + HostConst.AuthorizeCallback + "?batch=" + batchId;

            //        return View("~/Views/Authorize/DingTalk.cshtml", jsApiConfig);
            //    }

            //    var rUrl = ProviderGateway.AuthorizeProvider.GetIdentityUrl(appAccount.AppAccountId, currentUrl, 0, cookie);
            //    if (string.IsNullOrWhiteSpace(rUrl))
            //    {
            //        if (!ProviderGateway.AuthorizeProvider.CheckApp(menu.BeisenAppId, menu.Url))
            //        {
            //            return Redirect(AppConnectHostConfig.Cache[0] + HostConst.Error + "?message=请开通应用");
            //        }

            //        var signQuery = ProviderGateway.AppUserAccountProvider.GetSginQuery(cookie.AccountAppId, cookie.AccountOpenId, menu.BeisenAppId);

            //        return Redirect(Infrastructure.Helper.UrlHelper.AddQuery(menu.Url, signQuery));
            //    }
            //    else
            //    {
            //        return Redirect(rUrl);
            //    }
            //}

            var args = new Dictionary<string, string>();
            args.Add("tenant_id", menu.TenantId.ToString());
            args.Add("appaccount_id", menu.AppAccountId);
            args.Add("app_id", menu.BeisenAppId.ToString());
            args.Add("redirect_url", HttpUtility.UrlEncode(menu.Url));

            var url = Infrastructure.Helper.UrlHelper.AddParameter(AppConnectHostConfig.Cache[0] + HostConst.UserAuthorize, args);
            return Redirect(url);
        }
    }
}
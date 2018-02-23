using System.Web;
using System.Web.Mvc;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.Common.HelperObjects;
using Beisen.Logging;

namespace Beisen.AppConnect.Web.WebSite.Controllers
{
    public class AuthorizeController : BaseController
    {
        // GET: Authorize
        public ActionResult Callback(string code, string state, string batch)
        {



            ArgumentHelper.AssertIsTrue(!string.IsNullOrWhiteSpace(code), "code is null or empty");
            ArgumentHelper.AssertIsTrue(!string.IsNullOrWhiteSpace(state), "state is null or empty");
            ArgumentHelper.AssertIsTrue(!string.IsNullOrWhiteSpace(batch), "batch is null or empty");

            var cookieState = CookieHelper.GetState();


            //Log 信息
            try
            {
                var logMsg = string.Format("Callback参数:Code:{0},Batch:{1},State:{2}; --------CookieState{3};;;;", code, batch, state, cookieState);
                AppConnectLogHelper.Debug(logMsg);
            }
            catch (System.Exception ex)
            {

            }


            //if (cookieState != state)
            //{
            //    return Redirect(AppConnectHostConfig.Cache[0] + HostConst.Error + "?message=参数错误");
            //}

            var callbackContent = ProviderGateway.CallbackContentProvider.GetByBatchId(batch);
            if (callbackContent.State == CallbackContentState.Finish)
            {
                return Redirect(AppConnectHostConfig.Cache[0] + callbackContent.Content);
            }

            var redirectUrl = ProviderGateway.AuthorizeProvider.GetRedirectUrl(code, state, batch);

            return Redirect(redirectUrl);
        }

        public ActionResult DingTalk(string batch, string state, string redirect_url)
        {
            var jsApiConfig = ProviderGateway.JsApiProvider.GetJsApiConfig(batch, HttpUtility.UrlDecode(Request.Url.AbsoluteUri));
            jsApiConfig.State = state;
            jsApiConfig.RedirectUri = HttpUtility.UrlDecode(redirect_url);

            return View(jsApiConfig);
        }

        /// <summary>
        /// 钉钉PC登录校验
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="state"></param>
        /// <param name="redirect_url"></param>
        /// <returns></returns>
        public ActionResult DingTalkPC(string batch, string state, string redirect_url)
        {
            var jsApiConfig = ProviderGateway.JsApiProvider.GetJsApiConfig(batch, HttpUtility.UrlDecode(Request.Url.AbsoluteUri));
            jsApiConfig.State = state;
            jsApiConfig.RedirectUri = HttpUtility.UrlDecode(redirect_url);

            return View(jsApiConfig);
        }
    }
}
using System;
using System.Web.Mvc;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Attribute;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.Web.WebSite.Models;
using Beisen.Logging;
using Newtonsoft.Json;
namespace Beisen.AppConnect.Web.WebSite.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        [AccountIdentity]
        public ActionResult Info(int tenant_id = 0, string appaccount_id = "", int type = 12, int app_id = 100)
        {
            InitIdentity();

            //if (string.IsNullOrWhiteSpace(redirect_url))
            //{
            var email = ProviderGateway.AppUserAccountProvider.GetUserEmail(_AccountAppId, _AccountOpenId);
            ViewData["Email"] = email;
            ViewData["AppAccountTypeName"] = ProviderGateway.AppAccountProvider.GetAppAccountTypeName(RuntimeContext.AppAccountId);

            return View();
            //}

            //if (!ProviderGateway.AuthorizeProvider.CheckApp(app_id, redirect_url))
            //{
            //    return Redirect(AppConnectHostConfig.Cache[0] + HostConst.Error + "?message=请开通应用");
            //}

            //var signQuery = ProviderGateway.AppUserAccountProvider.GetSginQuery(_AccountAppId, _AccountOpenId, app_id);

            //return Redirect(Infrastructure.Helper.UrlHelper.AddQuery(redirect_url, signQuery));
        }

        public ActionResult Bind(string batch, int type = 0)
        {
            var bindingInfo = ProviderGateway.AppUserAccountProvider.GetBindingInfo(CookieHelper.GetCookie().OpenIds, batch);

            //返回null表示已经绑定，跳转到用户信息
            if (bindingInfo == null)
            {
                return Redirect(AppConnectHostConfig.Cache[0] + HostConst.UserInfo);
            }

            ViewData["Batch"] = batch;
            ViewData["Type"] = type == 0 ? bindingInfo.Type : type;
            ViewData["Email"] = bindingInfo.Email;
            ViewData["SendInterval"] = bindingInfo.SendInterval;

            return View();
        }

        public ActionResult NoAccount(string batch, int type)
        {
            ViewData["Batch"] = batch;
            ViewData["Type"] = type;
            return View();
        }

        public ActionResult HasBind(string batch, int type)
        {
            ViewData["Batch"] = batch;
            ViewData["Type"] = type;

            var batchInfo = ProviderGateway.CallbackContentProvider.GetByBatchId(batch);
            ViewData["AppAccountTypeName"] = ProviderGateway.AppAccountProvider.GetAppAccountTypeName(batchInfo.AppAccountPrivate);
            return View();
        }

        public ActionResult Activate(string code)
        {
            var result = ProviderGateway.AppUserAccountProvider.Activate(code);
            if (!result)
            {
                return Redirect(AppConnectHostConfig.Cache[0] + HostConst.Error + "?message=链接已失效");
            }

            return View();
        }

        public FileResult GetCaptchaImage(string batch)
        {
            var data = ProviderGateway.AppUserAccountProvider.GetCaptchaImage(batch);

            return File(data, "image/png");
        }

        public JsonResult _Bind(string batch, int type, string username, string password = "", string captcha = "")
        {
            try
            {
                var result = ProviderGateway.AppUserAccountProvider.Bind(CookieHelper.GetCookie().OpenIds, batch, (ServiceInterface.Model.Enum.AppUserAccountType)type, username, password, captcha);
                AppConnectLogHelper.Debug("Bind返回值DEBUG" + JsonConvert.SerializeObject(result));
                return Json(new WebApiResult<WebApiBindResult>
                {
                    Code = result.Result,
                    Message = result.Message,
                    Data = new WebApiBindResult
                    {
                        NeedCaptcha = result.NeedCaptcha,
                        RedirectUrl = result.RedirectUrl
                    }
                });
            }
            catch (System.Exception ex)
            {
                AppConnectLogHelper.Error("_Bind异常信息" + ex.Message + "EX:", ex);

                return Json(new WebApiResult<WebApiBindResult>
                {
                    Code = 417,
                    Message = ex.Message,
                    Data = new WebApiBindResult()
                });
            }
        }

        public JsonResult _UnBind()
        {
            var result = InitApiIdentity();
            if (result.Code == 0)
            {
                ProviderGateway.AppUserAccountProvider.UnBind(_AccountAppId, _AccountOpenId);
            }

            return Json(result);
        }

        [AccountIdentity]
        public ActionResult Authorize(string redirect_url, int tenant_id = 0, string appaccount_id = "", int type = 12, int app_id = 100)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(redirect_url))
                {
                    return Redirect(AppConnectHostConfig.Cache[0] + HostConst.Error + "?message=回调地址无效");
                }

                InitIdentity();

                if (!ProviderGateway.AuthorizeProvider.CheckApp(app_id, redirect_url))
                {
                    return Redirect(AppConnectHostConfig.Cache[0] + HostConst.Error + "?message=请开通应用");
                }
                //获取sign
                var signQuery = ProviderGateway.AppUserAccountProvider.GetSginQuery(_AccountAppId, _AccountOpenId, app_id);

                var url = Infrastructure.Helper.UrlHelper.AddQuery(redirect_url, signQuery);
                AppConnectLogHelper.DebugFormat("Authorize---最终跳转URL:{0},_AccountAppId{1},_AccountOpenId{2},app_id{3}", url, _AccountAppId, _AccountOpenId, app_id);
                return Redirect(url);
                //  return Redirect(Infrastructure.Helper.UrlHelper.AddQuery(redirect_url, signQuery));
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.ErrorFormat("Authorize异常:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                throw;
            }
        }

        [AccountIdentity]
        public ActionResult AuthorizeForMsg(string redirect_url, int tenant_id = 0, string appaccount_id = "", int type = 12, int app_id = 100)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(redirect_url))
                {
                    return Redirect(AppConnectHostConfig.Cache[0] + HostConst.Error + "?message=回调地址无效");
                }

                InitIdentity();

                //获取sign
                var signQuery = ProviderGateway.AppUserAccountProvider.GetSginQuery(_AccountAppId, _AccountOpenId, app_id);

                var url = Infrastructure.Helper.UrlHelper.AddQuery(redirect_url, signQuery);
                AppConnectLogHelper.DebugFormat("AuthorizeAuthorizeForMsg---最终跳转URL:{0},_AccountAppId{1},_AccountOpenId{2},app_id{3}", url, _AccountAppId, _AccountOpenId, app_id);
                return Redirect(url);
                //  return Redirect(Infrastructure.Helper.UrlHelper.AddQuery(redirect_url, signQuery));
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.ErrorFormat("AuthorizeForMsg异常:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                throw;
            }
        }
    }
}
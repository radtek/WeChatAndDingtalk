using System;
using System.Web.Mvc;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.Web.WebSite;
using Beisen.Logging;
using Newtonsoft.Json;

namespace Beisen.AppConnect.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomActionFilter());
        }
    }

    public class CustomActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Log 信息

            try
            {
                var tenant_id = filterContext.ActionParameters["tenant_id"];
                var appaccount_id = filterContext.ActionParameters["appaccount_id"];
                var app_id = filterContext.ActionParameters["app_id"];
                var redirect_url = filterContext.ActionParameters["redirect_url"];
                var logMsg = string.Format("TenantId:{0},Appaccont_Id:{1},App_Id:{2},Redirect_Url:{3};", tenant_id, appaccount_id, app_id, redirect_url);
                AppConnectLogHelper.Debug(logMsg);

                var cookie = CookieHelper.GetCookie();
                if (cookie != null)
                {
                    var jsonstr = SerializeHelper.Serialize(cookie);
                    var logMsg1 = string.Format("TenantId:{0},Appaccont_Id:{1},App_Id:{2},Redirect_Url:{3};Cookie{4}", tenant_id, appaccount_id, app_id, redirect_url, jsonstr);
                    AppConnectLogHelper.Debug(logMsg1);
                }
                else
                {
                    var logMsg2 = string.Format("Cookie为空： TenantId:{0},Appaccont_Id:{1},App_Id:{2},Redirect_Url:{3};", tenant_id, appaccount_id, app_id, redirect_url);
                    AppConnectLogHelper.Debug(logMsg2);
                }
            }
            catch (System.Exception ex)
            {
            }

            try
            {
                // 临时处理北森cloud应用身份失效情况，暂时直接跳错误页，后续再寻找解决方案
                var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
                var actionName = filterContext.ActionDescriptor.ActionName.ToLower();
                var appType = filterContext.HttpContext.Request["app_type"];
                if (controllerName == "user" && actionName == "authorize" && !string.IsNullOrWhiteSpace(appType))
                {
                    filterContext.Result = new RedirectResult(AppConnectHostConfig.Cache[0] + HostConst.Error + "?message=您的身份已失效，请重新进入应用");
                    return;
                }

                var attributes = filterContext.ActionDescriptor.GetCustomAttributes(false);
                bool isVerifyAccount = false;

                foreach (var attribute in attributes)
                {
                    var attributeFullName = attribute.ToString();
                    if (attributeFullName == "Beisen.AppConnect.Infrastructure.Attribute.AccountIdentityAttribute")
                    {
                        isVerifyAccount = true;
                    }
                }
                if (isVerifyAccount)
                {
                    //用于开发调试
                    //var appId = "wxdcfd8d067c353bed";
                    //var openId = "oeJYSwIG_y0WoXyPiGMSC66B-Gd4";
                    //var cookieDev = CookieHelper.GetCookie();
                    //if (cookieDev.OpenIds == null || !cookieDev.OpenIds.ContainsKey(appId))
                    //{
                    //    if (cookieDev.OpenIds == null)
                    //    {
                    //        cookieDev.OpenIds = new System.Collections.Generic.Dictionary<string, string>();
                    //    }
                    //    cookieDev.OpenIds.Add(appId, openId);
                    //    CookieHelper.SetCookie(cookieDev);
                    //    filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.RawUrl);
                    //    return;
                    //}

                    //var tenantId = ConvertToInt(filterContext.HttpContext.Request["tenant_id"]);
                    //var appAccountId = filterContext.HttpContext.Request["appaccount_id"];
                    var type = ConvertHelper.ToInt(filterContext.HttpContext.Request["type"]);
                    var cookie = CookieHelper.GetCookie();

                    string redirectUrl = null;
                    //获取登录状态，当有值且为0时是PC登录
                    var loginType = filterContext.HttpContext.Request.QueryString["login_type"];
                    AppConnectLogHelper.DebugFormat("loginType:{0},url:{1}", loginType, filterContext.HttpContext.Request.RawUrl);
                    if (!string.IsNullOrEmpty(loginType) && loginType == "0")//PC登录
                    {
                        redirectUrl = ProviderGateway.AuthorizeProvider.GetAuthorizeUrl(RuntimeContext.AppAccountId, filterContext.HttpContext.Request.RawUrl, type == 0 ? 12 : type, cookie, loginType);
                    }
                    else
                    {
                        redirectUrl = ProviderGateway.AuthorizeProvider.GetAuthorizeUrl(RuntimeContext.AppAccountId, filterContext.HttpContext.Request.RawUrl, type == 0 ? 12 : type, cookie);//移动端
                    }

                    if (redirectUrl != null)
                    {
                        filterContext.Result = new RedirectResult(redirectUrl);
                        return;
                    }

                    if (controllerName == "account" && actionName == "home")
                    {
                        //var tenantId = filterContext.HttpContext.Request["tenant_id"];
                        //var inviteUserId = filterContext.HttpContext.Request["inviteuser_id"];
                        //var isvId = filterContext.HttpContext.Request["isv_id"];
                        //var url = filterContext.HttpContext.Request["redirect_url"];
                        redirectUrl = ProviderGateway.AuthorizeProvider.GetIdentityUrl(RuntimeContext.AppAccountId, filterContext.HttpContext.Request.RawUrl, type == 0 ? 12 : type, cookie, ServiceInterface.Model.Enum.CallbackContentState.RegisterBind);
                    }
                    else
                    {
                        redirectUrl = ProviderGateway.AuthorizeProvider.GetIdentityUrl(RuntimeContext.AppAccountId, filterContext.HttpContext.Request.RawUrl, type == 0 ? 12 : type, cookie);
                    }
                    if (redirectUrl != null)
                    {
                        filterContext.Result = new RedirectResult(redirectUrl);
                        return;
                    }
                    CookieHelper.SetCookie(cookie);
                }
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.ErrorFormat("Filter执行异常:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                throw;
            }
        }

    }
}
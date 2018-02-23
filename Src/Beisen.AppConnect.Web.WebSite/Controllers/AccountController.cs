using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Attribute;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.AppConnect.Web.WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beisen.AppConnect.Web.WebSite.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        [AccountIdentity]
        public ActionResult Home(int inviteuser_id, string isv_id, string redirect_url, int tenant_id = 0, string appaccount_id = "", int type = 12)
        {
            if (tenant_id == 0 && string.IsNullOrEmpty(appaccount_id))
            {
                return Redirect(AppConnectHostConfig.Cache[0] + HostConst.Error + "?message=参数错误，tenant_id与appaccount_id不能同时为空");
            }

            InitIdentity();
            var logo = ProviderGateway.TenantProvider.GetTenantLogo(tenant_id);
            var appUserInfo = ProviderGateway.AppUserAccountProvider.GetByOpenId(_AccountAppId, _AccountOpenId);
            var email = appUserInfo.BeisenAccount;
            //var email = ProviderGateway.AppUserAccountProvider.GetUserEmail(_AccountAppId, _AccountOpenId);

            ViewData["TenantId"] = tenant_id;
            ViewData["InviteUserId"] = inviteuser_id;
            ViewData["IsvId"] = isv_id;
            ViewData["RedirectUrl"] = redirect_url;
            ViewData["Email"] = email;
            ViewData["AccountAppId"] = _AccountAppId;
            ViewData["Type"] = type;
            ViewData["Logo"] = logo;
            ViewData["Host"] = AppConnectHostConfig.Cache[0];

            return View();
        }

        public ActionResult Bind(int tenant_id, int inviteuser_id, string isv_id, string redirect_url, string appaccount_id = "", int type = 12, string batch = "")
        {
            if (tenant_id == 0 && string.IsNullOrEmpty(appaccount_id))
            {
                return Redirect(AppConnectHostConfig.Cache[0] + HostConst.Error + "?message=参数错误，tenant_id与appaccount_id不能同时为空");
            }

            if (string.IsNullOrEmpty(batch))
            {
                batch = ProviderGateway.RegisterProvider.GetBindBatchId(tenant_id, appaccount_id, type, redirect_url);
                if (string.IsNullOrEmpty(batch))
                {
                    return Redirect(AppConnectHostConfig.Cache[0] + HostConst.Error + "?message=参数错误，tenant_id不匹配");
                }
            }

            var logo = ProviderGateway.TenantProvider.GetTenantLogo(tenant_id);

            ViewData["TenantId"] = tenant_id;
            ViewData["InviteUserId"] = inviteuser_id;
            ViewData["IsvId"] = isv_id;
            ViewData["RedirectUrl"] = redirect_url;
            ViewData["Batch"] = batch;
            ViewData["Host"] = AppConnectHostConfig.Cache[0];
            ViewData["Logo"] = logo;

            return View();
        }

        [HttpPost]
        public JsonResult CheckMobile(string mobile)
        {
            if (mobile.Length == 11)
            {
                var result = ProviderGateway.StaffProvider.CheckUserNameExist(mobile);
                var message = result ? "该手机号已被注册" : "";

                return Json(new WebApiResult<bool>()
                {
                    Code = result ? 0 : 1,
                    Data = !result,
                    Message = message
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new WebApiResult<bool>()
            {
                Code = 0,
                Data = false,
                Message = "手机号格式不正确"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult VerifyCode(string mobile, int code, int type = 0)
        {
            int codestate = 0;
            var jsonResult = new WebApiResult<bool>()
            {
                Data = false,
            };

            if (mobile.Length == 11 && !string.IsNullOrEmpty(mobile))
            {
                if (code < 0 || type < 0)
                {
                    jsonResult.Data = false;
                    jsonResult.Message = "验证码错误，请重新输入";
                }
                if (ProviderGateway.MobileVerificationProvider.VerifyCode(mobile, code, type, out codestate))
                {
                    jsonResult.Code = 1;
                    jsonResult.Data = true;
                }
                else
                {
                    jsonResult.Data = false;
                    jsonResult.Message = codestate == 2 ? "验证码已失效，请重新获取" : "验证码错误，请重新输入";
                }
            }
            else
            {
                jsonResult.Data = false;
                jsonResult.Message = "手机号格式不正确";
            }

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Register(int tenant_id, string mobile, int code, string username, string password, int inviteuser_id, string redirect_url, string batch, int code_type = 0)
        {
            var jsonResult = new WebApiResult<bool>()
            {
                Data = false,
            };
            var message = string.Empty;
            var result = ProviderGateway.RegisterProvider.Register(tenant_id, mobile, code, code_type, username, password, inviteuser_id, out message);
            if (!result)
            {
                jsonResult.Message = message;
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
                //var errorPage = string.Format("{0}{1}?message={2}", AppConnectHostConfig.Cache[0], HostConst.Error, message);
                //return Redirect(errorPage);
            }

            //绑定
            var bindResult = ProviderGateway.AppUserAccountProvider.Bind(Infrastructure.Helper.CookieHelper.GetCookie().OpenIds, 
                batch, AppUserAccountType.Phone, mobile, password, string.Empty);

            if (bindResult.Result != 0)
            {
                jsonResult.Code = 1;
                jsonResult.Message = bindResult.Message;
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }

            jsonResult.Code = 1;
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendISVMobileValCode(int tenant_id, string mobile, string isv_id, int type = 0)
        {
            var jsonResult = new WebApiResult<bool>();

            bool result = false;
            var message = string.Empty;
            if (string.IsNullOrEmpty(mobile) || type < 0)
            {
                jsonResult.Data = false;
                jsonResult.Message = "参数错误";

                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(isv_id))
            {
                //to do sth.是否添加默认的isv模板
                jsonResult.Data = false;
                jsonResult.Message = "参数错误";

                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = ProviderGateway.MobileVerificationProvider.SendISVMessage(tenant_id, isv_id, mobile, out message, type);
            }

            jsonResult.Code = 1;
            jsonResult.Data = result;
            jsonResult.Message = message;

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
    }
}
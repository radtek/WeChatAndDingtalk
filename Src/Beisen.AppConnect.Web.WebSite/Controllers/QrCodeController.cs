using System.Web;
using System.Web.Mvc;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Attribute;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.AppConnect.Web.WebSite.Models;
using Beisen.Common.HelperObjects;

namespace Beisen.AppConnect.Web.WebSite.Controllers
{
    public class QrCodeController : BaseController
    {
        // GET: QrLogin
        public ActionResult Index(string redirect_url, int tenant_id = 0, string appaccount_id = "", int type = 12, int app_id = 100)
        {
            ArgumentHelper.AssertNotNullOrEmpty(redirect_url, "redirectUrl is null or empty");

            var url = HttpUtility.UrlDecode(redirect_url);
            if (!ProviderGateway.AuthorizeProvider.CheckApp(app_id,url))
            {
                return Redirect(AppConnectHostConfig.Cache[0] + HostConst.Error + "?message=请开通应用");
            }

            var code = ProviderGateway.QrCodeLoginProvider.GenerateQrCode(app_id);
            ViewData["Code"] = code;
            ViewData["TenantId"] = tenant_id;
            ViewData["AppAccountId"] = RuntimeContext.AppAccountId;
            ViewData["Type"] = type;
            ViewData["RedirectUrl"] = url;

            return View();
        }

        public ActionResult Image(string code, int size = 320, int tenant_id = 0, string appaccount_id = "", int type = 12)
        {
            ArgumentHelper.AssertNotNullOrEmpty(code, "code is null or empty");
            AppConnectLogHelper.DebugFormat("调用Image方法:code:{0}", code);
            var file = ProviderGateway.QrCodeLoginProvider.GenerateQrCodePicture(tenant_id, appaccount_id, type, code, size);
            if (file == null || file.Length <= 0)
            {
                AppConnectLogHelper.Error("file stream is null");
            }
            return File(file, "image/png");
        }

        [AccountIdentity]
        public ActionResult Scan(string code, int tenant_id = 0, string appaccount_id = "", int type = 12)
        {
            ArgumentHelper.AssertNotNullOrEmpty(code, "code is null or empty");

            InitIdentity();

            var result = ProviderGateway.QrCodeLoginProvider.Scan(code);

            if (result == false)
            {
                return Redirect(AppConnectHostConfig.Cache[0] + HostConst.Error + "?message=二维码已失效");
            }
            var email = ProviderGateway.AppUserAccountProvider.GetUserEmail(_AccountAppId, _AccountOpenId);
            ViewData["Email"] = email;
            ViewData["Code"] = code;

            return View();
        }

        public JsonResult _GetState(string code)
        {
            var qrCodeLoginInfo = ProviderGateway.QrCodeLoginProvider.GetAndUpdateByCode(code);
            var signQuery = "";
            if (qrCodeLoginInfo.State == QrCodeLoginState.Login)
            {
                signQuery = ItalentOAuthHelper.GetSginQuery(qrCodeLoginInfo.TenantId, qrCodeLoginInfo.UserId, qrCodeLoginInfo.TitaAppId);
            }

            return Json(new WebApiResult<QrCodeStateResult>
            {
                Data = new QrCodeStateResult
                {
                    State = (int) qrCodeLoginInfo.State,
                    TenantId = qrCodeLoginInfo.TenantId,
                    UserId = qrCodeLoginInfo.UserId,
                    SignQuery = signQuery
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type">2：登录 3：取消</param>
        /// <returns></returns>
        public JsonResult _Submit(string code, int type)
        {
            var result = InitApiIdentity<bool>();
            if (result.Code == 0)
            {
                result.Data = ProviderGateway.QrCodeLoginProvider.Submit(code, (QrCodeLoginState)type, _AccountAppId, _AccountOpenId);
            }

            return Json(result);
        }
    }
}
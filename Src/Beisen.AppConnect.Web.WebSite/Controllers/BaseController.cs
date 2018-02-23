using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.Web.WebSite.Models;

namespace Beisen.AppConnect.Web.WebSite.Controllers
{
    public class BaseController : System.Web.Mvc.Controller
    {
        /// <summary>
        /// 账户AppId
        /// </summary>
        protected string _AccountAppId;
        /// <summary>
        /// 账户OpenId
        /// </summary>
        protected string _AccountOpenId;

        /// <summary>
        /// 初始化用户身份
        /// </summary>
        protected void InitIdentity()
        {
            var cookie = CookieHelper.GetCookie();
            _AccountAppId = cookie.AccountAppId;
            _AccountOpenId = cookie.AccountOpenId;
        }

        protected WebApiResult InitApiIdentity()
        {
            var result = new WebApiResult();
            var cookie = CookieHelper.GetCookie();
            var verifyResult = ProviderGateway.AuthorizeProvider.VerifyUser(cookie);
            if (!verifyResult)
            {
                result.Code = 1;
                result.Message = "未获取到用户身份";
            }
            else
            {
                _AccountAppId = cookie.AccountAppId;
                _AccountOpenId = cookie.AccountOpenId;
            }

            return result;
        }
        /// <summary>
        /// 初始化用户身份
        /// </summary>
        protected WebApiResult<T> InitApiIdentity<T>()
        {
            var result = new WebApiResult<T>();
            var cookie = CookieHelper.GetCookie();
            var verifyResult = ProviderGateway.AuthorizeProvider.VerifyUser(cookie);
            if (!verifyResult)
            {
                result.Code = 1;
                result.Message = "未获取到用户身份";
            }
            else
            {
                _AccountAppId = cookie.AccountAppId;
                _AccountOpenId = cookie.AccountOpenId;
            }

            return result;
        }
    }
}
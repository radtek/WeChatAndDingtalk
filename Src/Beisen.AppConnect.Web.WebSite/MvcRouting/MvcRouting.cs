using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Web.WebSite
{
    public class MvcRouting
    {
        #region Singleton 
        static readonly MvcRouting _Instance = new MvcRouting();
        public static MvcRouting Instance
        {
            get
            {
                return _Instance;
            }
        }
        #endregion

        public const string LoginPage = "/ISV/LoginPage";
        public const string DingTalkAuthorize = "/ISV/DingTalkAuthorize";
        public const string DingTalkAuthorizePC = "/ISV/DingTalkAuthorizePC";
        private const string Error = "/Common/Error";
        private const string ISVLoginAuthorize = "/ISV/ISVLoginAuthorize";
        public string GetErrorInfoUrl(string message)
        {
            var errorInfoPageUrl = string.Format(Error + "?message={0}", message);
            return errorInfoPageUrl;
        }
        public string GetISVLoginAuthorize(string corpId, string appId, int loginType)
        {
            var ISVLoginAuthorizeUrl = string.Format(ISVLoginAuthorize + "?corpId={0}&appId={1}&loginType={2}", corpId, appId, loginType);
            return ISVLoginAuthorizeUrl;
        }
    }
}

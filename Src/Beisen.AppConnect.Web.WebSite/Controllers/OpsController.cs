using System;
using System.Web.Mvc;
using Beisen.AppConnect.ServiceInterface;

namespace Beisen.AppConnect.Web.WebSite.Controllers
{
    public class OpsController : Controller
    {
        private const string _Key = "9FF8AC375B827F4DDF92C8BDE29C8CFD";

        private void VerifyKey(string key)
        {
            if (Beisen.Common.Encrypt.Cryption.EncryptStrMD5(key) != _Key)
            {
                throw new Exception("非法请求~~");
            }
        }

        public ActionResult MenuList(string key ,int tenant_id)
        {
            VerifyKey(key);

            var result = ProviderGateway.MenuProvider.GetList(tenant_id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AppUserInfo(string key,int tenant_id, int id)
        {
            VerifyKey(key);

            var result = ProviderGateway.AppUserAccountProvider.GetById(tenant_id, id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UnBind(string key, string app_id,string open_id)
        {
            VerifyKey(key);

            ProviderGateway.AppUserAccountProvider.UnBind(app_id, open_id);

            return Json(true);
        }

        public ActionResult GetToken(string key, string appaccount_id, bool get_new = false)
        {
            VerifyKey(key);

            var result = ProviderGateway.TokenProvider.GetToken(0, appaccount_id, get_new);

            return Json(result);
        }

        public ActionResult GetTicket(string key, string appaccount_id, string ticket_type, bool get_new = false)
        {
            VerifyKey(key);

            var result = ProviderGateway.JsApiProvider.GetTicket(appaccount_id, ticket_type,get_new);

            return Json(result);
        }

        public ActionResult DeleteTemplateMapping(string key, int user_id, int id)
        {
            VerifyKey(key);

            ProviderGateway.TemplateMappingProvider.Delete(user_id, id);

            return Json(true);
        }

        public ActionResult InitAppAccount(string key, int tenant_id, string appaccount_id)
        {
            VerifyKey(key);

            ProviderGateway.AppAccountProvider.UpdateMultiTenant(tenant_id, appaccount_id);

            return Json(true);
        }

        public ActionResult ResendMessage(string key, int tenant_id, int id)
        {
            VerifyKey(key);

            var batchId = ProviderGateway.MessageProvider.Resend(tenant_id, id);

            return Json(batchId);
        }

        public ActionResult TestTitaMessage(string key, int tenant_id, int user_id, string to_userid)
        {
            var url = string.Format("http://titaapi.beisencorp.com/api/v1/{0}/{1}/message", tenant_id, user_id);

            var json = "{\"app_id\":0,\"app_type\":0,\"tenant_id\":"+ tenant_id + ",\"from_user_id\": " + user_id + ",\"to_user_ids\": \"" + to_userid + "\",\"template_id\": \"78\",\"is_sign\": true,\"template_title\": \"string\",\"values\": [{\"key\": \"to_openid\",\"value\": \"\"},{\"key\": \"appaccount_id\",\"value\": \"\"},{\"key\": \"url\",\"value\": \"http://www.baidu.com\"},{\"key\": \"first\",\"value\": \"您好，邀请您参加面试\"},{\"key\": \"keyword1\",\"value\": \"开发工程师\"},{\"key\": \"keyword2\",\"value\": \"面试时间\"},{\"key\": \"keyword3\",\"value\": \"面试地点\"},{\"key\": \"keyword4\",\"value\": \"面试说明\"},{\"key\": \"keyword5\",\"value\": \"通知人\"},{\"key\": \"remark\",\"value\": \"点击详情，答复是否参加\"}]}";

            var result = Beisen.AppConnect.Infrastructure.RequestUtility.Request.SendRequestForJson(url, RestSharp.Method.POST, json);
            return Json(result);
        }
    }
}
using Beisen.AppConnectISV.Infrastructure;
using Beisen.AppConnectISV.Model.HttpModel;
using Beisen.AppConnectISV.ServiceImp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Beisen.AppConnect.Web.WebSite.Controllers
{

    public class ISVApiController : ApiController
    {
        /// <summary>
        /// 测试API是否通
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string ApiTest()
        {

            return "value";
        }
        /// <summary>
        /// 公司数据源触发器
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CorpId()
        {
            LogHelper.Instance.Dump("进入CorpId_GetObjectListDataSource");
            var corpLists = DataSourceProvider.Instance.GetObjectListDataSource();
            return corpLists;
        }


        /// <summary>
        /// DingTalk CallBack
        /// </summary>
        /// <param name="bodyMsg"></param>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DingTalkCallBack([FromBody] DingTalkCallBack bodyMsg, string signature, string timestamp, string nonce)
        {
            var checkCode = ISVCallbackProvider.Instance.ISVCallback(bodyMsg.encrypt, signature, timestamp, nonce);
            return checkCode;
        }

        [HttpPost]
        public ApiResult GetUserInfo([FromUri]int tenantId, [FromBody]UserInfoArgument userInfoArgument)
        {
            var userInfoList = UserInfoMappingProvider.Instance.GetUserInfoMapping(tenantId, userInfoArgument.userIds, userInfoArgument.appAccountId);
            return userInfoList;
        }

        [HttpPost]
        public ApiResult SendMessage([FromUri]int tenant_id, [FromUri]int user_id, [FromBody]MessageInfoArgument message)
        {
            LogHelper.Instance.Dump(string.Format("SendMessage Start: TenantId:{0},UserId:{1};MessageInfoArgument:{2}", tenant_id, user_id, JsonConvert.SerializeObject(message)));
            var apiResult = ISVSendMessageProvider.Instance.SendMessageDingTalk(tenant_id, user_id, message);
            LogHelper.Instance.Dump(string.Format("SendMessage Stop : ApiResult:{0}", JsonConvert.SerializeObject(apiResult)));
            return apiResult;
        }
    }
}
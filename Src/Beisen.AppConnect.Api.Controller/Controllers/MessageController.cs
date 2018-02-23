using System.Collections.Generic;
using System.Web.Http;
using Beisen.AppConnect.Api.Controller.Models;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using System;
using Beisen.Common.HelperObjects;
using System.Linq;
using Beisen.AppConnect.Infrastructure.Configuration;
using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Controllers
{
    public class MessageController : ApiControllerBase
    {
        [HttpPost]
        public ApiResult Send([FromUri]int tenant_id, [FromUri]int user_id, [FromBody]MessageArgument message)
        {
            var messageInfo = new MessageInfo();
            messageInfo.TenantId = tenant_id;
            messageInfo.FromUser = user_id;
            messageInfo.ToUser = message.to_user;
            messageInfo.ToOpenId = message.to_openid;
            messageInfo.AppAccountId = ProviderGateway.AppAccountProvider.GetAppAccountId(tenant_id, message.appaccount_id, message.tag);
            messageInfo.TemplateIdShort = message.template_id_short;
            messageInfo.TemplateId = message.template_id;
            messageInfo.Content = new MessageContent();
            messageInfo.Content.Url = message.content.url;
            messageInfo.Content.Title = message.content.title;
            messageInfo.Content.Detail = new List<MessageContentDetail>();
            foreach (var detail in message.content.detail)
            {
                messageInfo.Content.Detail.Add(new MessageContentDetail { Key = detail.key, Text = detail.text, Value = detail.value });
            }
            messageInfo.ContentJson = SerializeHelper.Serialize(messageInfo.Content);
            AppConnectLogHelper.Debug("Send接收到租户：" + tenant_id + "开始发送消息,消息内容:" + Newtonsoft.Json.JsonConvert.SerializeObject(messageInfo));
            var batchId = ProviderGateway.MessageProvider.Send(messageInfo);

            return new Models.MessageSendResult { BatchId = batchId };
        }

        [HttpPost]
        public ApiResult SendMessage([FromUri]int tenant_id, [FromUri]int user_id, [FromBody]MessageArgumentForTita message)
        {
            Models.MessageSendResult resultMessage = new Models.MessageSendResult();
            var messageInfo = new MessageInfo();
            try
            {
                VaildMessageType(tenant_id, user_id, message);
                messageInfo.TenantId = tenant_id;
                messageInfo.FromUser = user_id;
                messageInfo.ToUser = message.receiverId;
                messageInfo.ProductId = message.productId;
                messageInfo.MessageTypeList = SpliceMessageType(message.messageType);
                messageInfo.Content = new MessageContent();
                messageInfo.Content.Url = message.content.url;
                messageInfo.Content.Title = message.content.title;
                messageInfo.Content.Detail = new List<MessageContentDetail>();
                foreach (var detail in message.content.detail)
                {
                    messageInfo.Content.Detail.Add(new MessageContentDetail { Key = detail.key, Text = detail.text, Value = detail.value });
                }
                messageInfo.ContentJson = SerializeHelper.Serialize(messageInfo.Content);
                AppConnectLogHelper.Debug("Send接收到租户：" + tenant_id + "开始发送消息,消息内容:" + Newtonsoft.Json.JsonConvert.SerializeObject(messageInfo));
                var batchId = ProviderGateway.MessageProvider.Send(messageInfo);

                resultMessage.BatchId = batchId;
                resultMessage.ErrCode = 0;
                resultMessage.ErrMsg = "企业消息发送成功!";
                AppConnectLogHelper.DebugFormat("租户:{0},接收人:{1},产品:{2},消息:{3}发送成功！", messageInfo.TenantId, messageInfo.ToUser, messageInfo.ProductId, JsonConvert.SerializeObject(messageInfo.Content));

            }
            catch (System.Exception ex)
            {
                resultMessage.ErrCode = 500;
                resultMessage.ErrMsg = string.Format("调用企业发送消息失败:{0}", ex.Message);
                AppConnectLogHelper.ErrorFormat("企业消息发送消息异常:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(ex));
            }

            //调用ISV消息发送
            try
            {
                AppConnectLogHelper.Debug("开始发送ISV消息");
                var requestUrl = string.Format("{0}/Api/ISVApi/SendMessage?tenant_id={1}&user_id={2}", AppConnectHostConfig.Cache[0], tenant_id, user_id);
                string resultStr = HttpClientHelper.HttpPost(requestUrl, message);
                var resultModel = JsonConvert.DeserializeObject<ApiResultModel>(resultStr);
                if (resultModel.Code != 0)
                {
                    resultMessage.ErrCode = 500;
                    resultMessage.ErrMsg = "ISV消息发送失败!";
                    AppConnectLogHelper.Error("调用ISV 发送消息api失败");
                }
                AppConnectLogHelper.Debug("调用ISV 发送消息api成功");
            }
            catch (Exception ex)
            {
                resultMessage.ErrCode = 500;
                resultMessage.ErrMsg = string.Format("调用ISV发送消息失败:{0}", ex.Message);
                AppConnectLogHelper.ErrorFormat("发送消息异常:{0}", JsonConvert.SerializeObject(ex));
            }
            AppConnectLogHelper.DebugFormat("SendMessageResult:{0}", JsonConvert.SerializeObject(resultMessage));
            return resultMessage;
        }

        private void VaildMessageType(int tenant_id, int user_id, MessageArgumentForTita message)
        {
            ArgumentHelper.AssertIsTrue(tenant_id > 0, "current tenant_id is 0");
            ArgumentHelper.AssertIsTrue(user_id > 0, "current user_id is 0");
            AppConnectLogHelper.DebugFormat("开始调用消息接口,租户：{0},发送者:{1}", tenant_id, user_id);
            if (string.IsNullOrWhiteSpace(message.productId))
            {
                throw new Exception("ProductId is null or empty!");
            }
            if (string.IsNullOrWhiteSpace(message.messageType))
            {
                throw new Exception("MessageType is null or empty!");
            }
            if (string.IsNullOrWhiteSpace(message.receiverId))
            {
                throw new Exception("ReceiverId is null or empty!!");
            }
        }
        /// <summary>
        /// 拼接Tag,如果是0则获取所有Tag进行拼接
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        private List<int> SpliceMessageType(string messageType)
        {
            List<int> tagList = new List<int>();
            if (messageType == "0")
            {
                var appAccountTypes = ProviderGateway.AppAccountProvider.GetAppAccountTypes();
                foreach (var appAccountType in appAccountTypes)
                {
                    tagList.Add(appAccountType.Key);
                }
            }
            else
            {
                List<string> messageTypeList = new List<string>(messageType.Split('|').Distinct());
                messageTypeList.ForEach(currentObj =>
                {
                    tagList.Add(Convert.ToInt32(currentObj));
                });
            }
            return tagList;
        }
    }
}

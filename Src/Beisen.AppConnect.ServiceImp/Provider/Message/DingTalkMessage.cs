using System.Collections.Generic;
using System.Text;
using System.Web;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Newtonsoft.Json;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    internal class DingTalkMessage : IMessage
    {
        #region 单例

        protected static readonly IMessage _instance = new DingTalkMessage();

        public static IMessage Intance
        {
            get { return _instance; }
        }

        private DingTalkMessage()
        {
        }

        #endregion

        public List<MessageInfo> Build(AppAccountInfo appAccount, List<int> userIds, MessageInfo message)
        {
            var result = new List<MessageInfo>();
            result.Add(new MessageInfo
            {
                Id = message.Id,
                BatchId = message.BatchId,
                TenantId = message.TenantId,
                FromUser = message.FromUser,
                ToUser = message.ToUser,
                ToOpenId = message.ToOpenId,
                AppAccountId = appAccount.AppAccountId,
                TemplateIdShort = message.TemplateIdShort,
                TemplateId = message.TemplateId,
                Content = message.Content,
                ContentJson = message.ContentJson,
                State = message.State,
                MessageId = message.MessageId,
                Result = message.Result
            });

            return result;
        }

        public MessageSendResult Send(AppAccountInfo appAccount, MessageInfo message)
        {
            var result = new MessageSendResult();
            if (appAccount == null)
            {
                appAccount = ProviderGateway.AppAccountProvider.Get(message.AppAccountId);
            }
            var token = ProviderGateway.TokenProvider.GetToken(appAccount);

            if (string.IsNullOrWhiteSpace(message.ToOpenId))
            {
                var appUsers = ProviderGateway.AppUserAccountProvider.GetListByUserId(message.TenantId, message.ToUser.Split('|'), appAccount.AppId);
                var toOpenIds = new List<string>();
                foreach (var appUser in appUsers)
                {
                    toOpenIds.Add(appUser.OpenId);
                }
                message.ToOpenId = string.Join("|", toOpenIds);
            }

            var description = new StringBuilder();
            foreach (var detail in message.Content.Detail)
            {
                if (string.IsNullOrWhiteSpace(detail.Text))
                {
                    description.AppendFormat("{0}\n", detail.Value);
                }
                else
                {
                    description.AppendFormat("{0}：{1}\n", detail.Text, detail.Value);
                }
            }
           ;
            var picUrl = MediaDataHelper.GetPicUrl(message.ProductId, appAccount.Type.ToString());
            AppConnectLogHelper.DebugFormat("调用钉钉API发送消息:Token{0};ToOpenId{1}; AgentId{2} ContentUrl{3}, ContentTitle{4}, description:{5}, pic:{6},tenantId:{7}", token, message.ToOpenId, appAccount.AgentId, message.Content.Url, message.Content.Title, description.ToString(), picUrl, appAccount.TenantId);
            Beisen.AppConnect.DingTalkSDK.Model.MessageSendResult sendResult = new Beisen.AppConnect.DingTalkSDK.Model.MessageSendResult();
            if (string.IsNullOrEmpty(message.Content.Url))
            {
                message.Content.Url = AppConnectHostConfig.Cache[0] + string.Format("/Common/Error?title={0}&message={1}", HttpUtility.UrlEncode("友情提醒"), HttpUtility.UrlEncode("抱歉，此消息暂不支持在此查看，请在电脑浏览器中查看"));
            }
            if (string.IsNullOrEmpty(message.Content.DingTalkPCUrl))
            {
                message.Content.DingTalkPCUrl = AppConnectHostConfig.Cache[0] + string.Format("/Common/Error?title={0}&message={1}", HttpUtility.UrlEncode("友情提醒"), HttpUtility.UrlEncode("抱歉，此消息暂不支持在此查看，请在电脑浏览器中查看"));
            }
            sendResult = DingTalkSDK.Message.SendOA(token, message.ToOpenId, appAccount.AgentId, message.Content.Url, message.Content.Title, description.ToString(), message.Content.DingTalkPCUrl, picUrl);

            if (sendResult.ErrCode == 0)
            {
                result.State = MessageState.Success;
                AppConnectLogHelper.DebugFormat("钉钉发送消息成功,TenantId：{0},返回值：{1}", appAccount.TenantId, JsonConvert.SerializeObject(sendResult));
            }
            else
            {
                result.State = MessageState.Failed;
                AppConnectLogHelper.ErrorFormat("钉钉发送消息失败,TenantId：{0}，userId:{1},errorCode:{2},errorMsg:{3}", appAccount.TenantId, message.ToOpenId, sendResult.ErrCode, sendResult.ErrMsg);
            }
            result.ErrMsg = SerializeHelper.Serialize(sendResult);
            result.MessageId = sendResult.MessageId;
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.AppConnect.WeChatSDK.Model;
using Newtonsoft.Json;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    internal class WeChatWorkMessage : IMessage
    {
        #region 单例

        protected static readonly IMessage _instance = new WeChatWorkMessage();

        public static IMessage Intance
        {
            get { return _instance; }
        }

        private WeChatWorkMessage()
        {
        }

        #endregion

        public List<MessageInfo> Build(AppAccountInfo appAccount, List<int> userIds, MessageInfo message)
        {
            var result = new List<MessageInfo>();

            if (string.IsNullOrWhiteSpace(message.ToOpenId))
            {
                if (userIds == null)
                {
                    userIds = message.ToUser.Split('|').Select(i => Convert.ToInt32(i)).ToList();
                }
                if (userIds.Count <= 1000)
                {
                    result.Add(new MessageInfo
                    {
                        Id = message.Id,
                        BatchId = message.BatchId,
                        TenantId = message.TenantId,
                        FromUser = message.FromUser,
                        ToUser = message.ToUser,
                        AppAccountId = appAccount.AppAccountId,
                        TemplateIdShort = message.TemplateIdShort,
                        TemplateId = message.TemplateId,
                        Content = message.Content,
                        ContentJson = message.ContentJson,
                        State = message.State,
                        MessageId = message.MessageId,
                        Result = message.Result
                    });
                }
                else
                {
                    var toUserList = new List<string>();
                    var toUser = new List<int>();
                    for (var i = 1; i <= userIds.Count; i++)
                    {
                        toUser.Add(userIds[i]);
                        if (i % 1000 == 0)
                        {
                            toUserList.Add(string.Join("|", toUser));
                            toUser = new List<int>();
                        }
                    }

                    foreach (var user in toUserList)
                    {
                        result.Add(new MessageInfo
                        {
                            Id = message.Id,
                            BatchId = message.BatchId,
                            TenantId = message.TenantId,
                            FromUser = message.FromUser,
                            ToUser = user,
                            AppAccountId = appAccount.AppAccountId,
                            TemplateIdShort = message.TemplateIdShort,
                            TemplateId = message.TemplateId,
                            Content = message.Content,
                            ContentJson = message.ContentJson,
                            State = message.State,
                            MessageId = message.MessageId,
                            Result = message.Result
                        });
                    }
                }
            }
            else
            {
                var messageOpenIdList = message.ToOpenId.Split('|');
                if (messageOpenIdList.Length <= 1000)
                {
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
                }
                else
                {
                    var toOpenIdList = new List<string>();
                    var toOpenId = new List<string>();
                    for (var i = 1; i <= messageOpenIdList.Length; i++)
                    {
                        toOpenId.Add(messageOpenIdList[i]);
                        if (i % 1000 == 0)
                        {
                            toOpenIdList.Add(string.Join("|", toOpenId));
                            toOpenId = new List<string>();
                        }
                    }

                    foreach (var openId in toOpenIdList)
                    {
                        result.Add(new MessageInfo
                        {
                            Id = message.Id,
                            BatchId = message.BatchId,
                            TenantId = message.TenantId,
                            FromUser = message.FromUser,
                            ToUser = message.ToUser,
                            ToOpenId = openId,
                            AppAccountId = appAccount.AppAccountId,
                            TemplateIdShort = message.TemplateIdShort,
                            TemplateId = message.TemplateId,
                            Content = message.Content,
                            ContentJson = message.ContentJson,
                            State = message.State,
                            MessageId = message.MessageId,
                            Result = message.Result
                        });
                    }
                }
            }

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
            var picUrl = MediaDataHelper.GetPicUrl(message.ProductId, appAccount.Type.ToString());
            AppConnectLogHelper.DebugFormat("调用企业微信发送消息:Token{0};ToOpenId{1}; AgentId{2} ContentUrl{3}, ContentTitle{4}, description:{5}, pic:{6},tenantId:{7}", token, message.ToOpenId, appAccount.AgentId, message.Content.Url, message.Content.Title, description.ToString(), picUrl, appAccount.TenantId);

            if (string.IsNullOrEmpty(message.Content.Url))
            {
                message.Content.Url = AppConnectHostConfig.Cache[0] + string.Format("/Common/Error?title={0}&message={1}", HttpUtility.UrlEncode("友情提醒"), HttpUtility.UrlEncode("抱歉，此消息暂不支持在此查看，请在电脑浏览器中查看"));
            }
            WorkMessageSendResult sendResult = new WorkMessageSendResult();
            sendResult = WeChatSDK.Work.Message.SendNews(token, message.ToOpenId, Convert.ToInt32(appAccount.AgentId), message.Content.Title, description.ToString(), message.Content.Url, picUrl);
            if (sendResult.ErrCode == 0)
            {
                result.State = MessageState.Success;
                AppConnectLogHelper.DebugFormat("企业微信发送消息成功{0}", JsonConvert.SerializeObject(sendResult));
            }
            else
            {
                result.State = MessageState.Failed;
                AppConnectLogHelper.ErrorFormat("企业微信发送消息失败,errorCode:{0},errorMsg:{1},tenantId:{2},userId:{3}", sendResult.ErrCode, sendResult.ErrMsg, appAccount.TenantId, message.ToOpenId);
            }
            
            result.ErrMsg = SerializeHelper.Serialize(sendResult);
            return result;
        }
    }
}

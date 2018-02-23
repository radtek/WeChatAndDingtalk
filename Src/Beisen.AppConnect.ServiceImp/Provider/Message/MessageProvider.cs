using System;
using System.Collections.Generic;
using Beisen.AppConnect.ServiceImp.Persistance;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.Common.HelperObjects;
using Beisen.AppConnect.Infrastructure.Helper;
using System.Text.RegularExpressions;
using System.Linq;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.Logging;
using System.Web;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    public class MessageProvider : IMessageProvider
    {
        #region 单例

        private static readonly IMessageProvider _instance = new MessageProvider();
        public static IMessageProvider Instance
        {
            get { return _instance; }
        }

        private MessageProvider()
        {
        }
        #endregion

        public string Send(MessageInfo message)
        {
            var messageBuildList = new List<MessageInfo>();

            if (string.IsNullOrWhiteSpace(message.BatchId))
            {
                message.BatchId = Guid.NewGuid().ToString();
            }
            if (message.MessageTypeList.Count > 0)
            {
                var appUsers = ProviderGateway.AppUserAccountProvider.GetListByUserId(message.TenantId, message.ToUser.Split('|'));
                var appIds = new List<string>();
                var userDic = new Dictionary<string, List<int>>();
                foreach (var appUser in appUsers)
                {
                    if (!userDic.ContainsKey(appUser.AppId))
                    {
                        userDic.Add(appUser.AppId, new List<int> { appUser.UserId });
                        appIds.Add(appUser.AppId);
                    }
                    else
                    {
                        userDic[appUser.AppId].Add(appUser.UserId);
                    }
                }
                if (appIds.Count <= 0)
                {
                    AppConnectLogHelper.DebugFormat("消息接收者{0}没有绑定钉钉微信相关产品!消息内容是:{1}", message.ToUser, Newtonsoft.Json.JsonConvert.SerializeObject(message));
                    throw new Exception("接收人未注册钉钉,微信等相关产品!");
                }
                var appAccounts = ProviderGateway.AppAccountProvider.GetListByAppId(message.TenantId, appIds);
                var messageUrl = message.Content.Url;

                IEnumerable<AppAccountInfo> appAccountDistinct = new List<AppAccountInfo>();
                //去除重复应用
                Dictionary<string, AppAccountInfo> appAccountDistinctAgent = new Dictionary<string, AppAccountInfo>();
                foreach (var appAccount in appAccounts)
                {
                    if (string.IsNullOrEmpty(appAccount.AppId) || string.IsNullOrEmpty(appAccount.AgentId))
                    {
                        continue;
                    }
                    var key = string.Format("{0}_{1}",appAccount.TenantId, appAccount.AppId);
                    if (!appAccountDistinctAgent.ContainsKey(key))
                    {
                        appAccountDistinctAgent.Add(key, appAccount);
                    }
                }
                appAccountDistinct = appAccountDistinctAgent.Select(t => t.Value);
                AppConnectLogHelper.DebugFormat("appAccount个数:{0}", appAccountDistinct.Count());
                foreach (var appAccount in appAccountDistinct)
                {
                    if (message.MessageTypeList.Contains(appAccount.Type))
                    {
                        var messageModel = BuildNewModel(message);
                        messageModel.AppAccountId = appAccount.AppAccountId;
                        messageModel.Content.Url = GetUrl(messageModel.TenantId, messageModel.AppAccountId, messageModel.ProductId, messageUrl);
                        if (appAccount.Type == 21)//只有钉钉的时候
                        {
                            messageModel.Content.DingTalkPCUrl = GetDingTalkPCUrl(messageModel.TenantId, messageModel.AppAccountId, messageModel.ProductId, messageUrl);
                        }
                        messageModel.ContentJson = SerializeHelper.Serialize(messageModel.Content);
                        var instance = MessageFactory.GetIntance(appAccount);
                        var buildList = instance.Build(appAccount, userDic[appAccount.AppId], messageModel);
                        messageBuildList.AddRange(buildList);
                        AppConnectLogHelper.DebugFormat("message model:{0},messageType:{1}", Newtonsoft.Json.JsonConvert.SerializeObject(messageModel), appAccount.Type);
                    }
                }
            }
            AppConnectLogHelper.DebugFormat("组装发送消息的个数:{0}", messageBuildList.Count());
            foreach (var messageBuild in messageBuildList)
            {
                var appAccount = ProviderGateway.AppAccountProvider.Get(messageBuild.AppAccountId);
                if (messageBuild.Id == 0)
                {
                    messageBuild.State = MessageState.Unsent;
                    messageBuild.Id = Add(messageBuild);
                }
                var instance = MessageFactory.GetIntance(appAccount);
                var sendResult = new MessageSendResult();
                try
                {
                    messageBuild.ProductId = message.ProductId;
                    sendResult = instance.Send(appAccount, messageBuild);
                }
                catch (Exception ex)
                {
                    sendResult.State = MessageState.Failed;
                    sendResult.ErrMsg = ex.Message;
                }
                finally
                {
                    //更新msgid、发送结果
                    UpdateSendResult(messageBuild.TenantId, messageBuild.Id, sendResult.MessageId, sendResult.State, sendResult.ErrMsg);
                }
            }

            return message.BatchId;
        }

        private MessageInfo BuildNewModel(MessageInfo message)
        {
            MessageInfo messageNew = new MessageInfo()
            {
                Id = message.Id,
                BatchId = message.BatchId,
                TenantId = message.TenantId,
                FromUser = message.FromUser,
                ToUser = message.ToUser,
                ToOpenId = message.ToOpenId,
                AppAccountId = message.AppAccountId,
                TemplateIdShort = message.TemplateIdShort,
                TemplateId = message.TemplateId,
                Content = message.Content,
                ContentJson = message.ContentJson,
                State = message.State,
                MessageId = message.MessageId,
                Result = message.Result,
                MediaUrl = message.MediaUrl,
                MessageTypeList = message.MessageTypeList,
                ProductId = message.ProductId
            };

            MessageContent contentNew = new MessageContent();
            contentNew.Url = message.Content.Url;
            contentNew.Detail = message.Content.Detail;
            contentNew.Title = message.Content.Title;
            messageNew.Content = contentNew;
            messageNew.ContentJson = SerializeHelper.Serialize(messageNew.Content);
            return messageNew;
        }
        #region 构造URL
        private string GetUrl(int tenantId, string appaccount_id, string app_id, string return_url)
        {
            ArgumentHelper.AssertPositive(tenantId, "GetUrl tenantId is not positive");
            ArgumentHelper.AssertNotNull(appaccount_id, "GetUrl appaccount_id is null");
            ArgumentHelper.AssertNotNull(app_id, "GetUrl app_id is null");
            if (string.IsNullOrEmpty(return_url))
            {
                return null;
            }
            var encodeRedirectUrl = HttpUtility.UrlEncode(string.Format("{0}?return_url={1}", AppConnectHostConfig.Cache[2], HttpUtility.UrlEncode(return_url)));
            string redirectUrl = string.Format("{0}/user/authorizeformsg?tenant_id={1}&appaccount_id={2}&app_id={3}&redirect_url={4}", AppConnectHostConfig.Cache[0], tenantId, appaccount_id, app_id, encodeRedirectUrl);
            AppConnectLogHelper.DebugFormat("移动端拼接后详情页RedirectUrl:{0}", redirectUrl);
            return redirectUrl;
        }
        private string GetDingTalkPCUrl(int tenantId, string appaccount_id, string app_id, string return_url)
        {
            ArgumentHelper.AssertPositive(tenantId, "GetDingTalkPCUrl tenantId is not positive");
            ArgumentHelper.AssertNotNull(appaccount_id, "GetDingTalkPCUrl appaccount_id is null");
            ArgumentHelper.AssertNotNull(app_id, "GetDingTalkPCUrl app_id is null");
            if (string.IsNullOrEmpty(return_url))
            {
                return null;
            }
            var encodeRedirectUrl = HttpUtility.UrlEncode(string.Format("{0}?return_url={1}", AppConnectHostConfig.Cache[2], HttpUtility.UrlEncode(return_url)));
            string redirectUrl = string.Format("{0}/user/authorizeformsg?tenant_id={1}&appaccount_id={2}&app_id={3}&login_type=0&redirect_url={4}", AppConnectHostConfig.Cache[0], tenantId, appaccount_id, app_id, encodeRedirectUrl);
            AppConnectLogHelper.DebugFormat("PC端拼接后详情页RedirectUrl:{0}", redirectUrl);
            return redirectUrl;
        }
        #endregion


        public int Add(MessageInfo info)
        {
            ArgumentHelper.AssertIsTrue(info != null, "MessageInfo is null");
            ArgumentHelper.AssertNotNullOrEmpty(info.BatchId, "MessageInfo.BatchId is null or empty");
            ArgumentHelper.AssertIsTrue(info.TenantId > 0, "MessageInfo.TenantId is 0");
            ArgumentHelper.AssertNotNullOrEmpty(info.AppAccountId, "MessageInfo.AppAccountId is null or empty");
            ArgumentHelper.AssertNotNullOrEmpty(info.ContentJson, "MessageInfo.ContentJson is null or empty");

            return MessageDao.Insert(info);
        }

        public void UpdateSendResult(int tenantId, int id, string messageId, MessageState state, string result)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertIsTrue(id > 0, "id is 0");

            MessageDao.UpdateSendResult(tenantId, id, messageId, state, result);
        }

        public void UpdateSendResultForWeChatService(string appAccountId, int userId, string messageId, MessageState state, string result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public MessageInfo Get(int tenantId, int id)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertIsTrue(id > 0, "id is 0");

            return MessageDao.Get(tenantId, id);
        }

        /// <summary>
        /// 重发消息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Resend(int tenantId, int id)
        {
            var messageInfo = Get(tenantId, id);
            var appAccount = ProviderGateway.AppAccountProvider.Get(messageInfo.AppAccountId);
            var instance = MessageFactory.GetIntance(appAccount);
            var sendResult = new MessageSendResult();
            try
            {
                sendResult = instance.Send(appAccount, messageInfo);
            }
            catch (Exception ex)
            {
                sendResult.State = MessageState.Failed;
                sendResult.ErrMsg = ex.Message;
            }
            finally
            {
                //更新msgid、发送结果
                UpdateSendResult(messageInfo.TenantId, messageInfo.Id, sendResult.MessageId, sendResult.State, sendResult.ErrMsg);
            }
            return messageInfo.BatchId;
        }
    }
}

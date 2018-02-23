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
using Beisen.AppConnect.WeChatSDK.MP;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    internal class WeChatServiceMessage:IMessage
    {
        #region 单例

        protected static readonly IMessage _instance = new WeChatServiceMessage();

        public static IMessage Intance
        {
            get { return _instance; }
        }

        private WeChatServiceMessage()
        {
        }

        #endregion

        public List<MessageInfo> Build(AppAccountInfo appAccount,List<int> userIds,MessageInfo message)
        {
            var result = new List<MessageInfo>();
            if (string.IsNullOrWhiteSpace(message.ToOpenId))
            {
                if (userIds == null)
                {
                    userIds = message.ToUser.Split('|').Select(i => Convert.ToInt32(i)).ToList();
                }
                foreach (var userId in userIds)
                {
                    result.Add(new MessageInfo
                    {
                        Id = message.Id,
                        BatchId = message.BatchId,
                        TenantId = message.TenantId,
                        FromUser = message.FromUser,
                        ToUser = userId.ToString(),
                        ToOpenId=message.ToOpenId,
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
            else
            {
                var openIds = message.ToOpenId.Split('|');
                foreach (var openId in openIds)
                {
                    result.Add(new MessageInfo
                    {
                        Id = message.Id,
                        BatchId = message.BatchId,
                        TenantId = message.TenantId,
                        FromUser = message.FromUser,
                        ToUser = message.ToUser,
                        ToOpenId=openId,
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
            return result;
        }

        public MessageSendResult Send(AppAccountInfo appAccount,MessageInfo message)
        {
            var result = new MessageSendResult();
            if (appAccount == null)
            {
                appAccount = ProviderGateway.AppAccountProvider.Get(message.AppAccountId);
            }

            var token = ProviderGateway.TokenProvider.GetToken(appAccount);

            if (string.IsNullOrWhiteSpace(message.TemplateId))
            {
                if (string.IsNullOrWhiteSpace(message.TemplateIdShort))
                {
                    result.State = MessageState.Failed;
                    result.ErrMsg = "模板为空";
                    return result;
                }
                //跟据数据库中对应关系获取id
                var templateId = ProviderGateway.TemplateMappingProvider.GetTemplateId(appAccount.AppId, message.TemplateIdShort);
                //不存在增加模板
                if (string.IsNullOrWhiteSpace(templateId))
                {
                    var template = Template.AddTemplate(token, message.TemplateIdShort);
                    if (template.ErrCode != 0)
                    {
                        result.State = MessageState.Failed;
                        result.ErrMsg = SerializeHelper.Serialize(template);
                        return result;
                    }
                    message.TemplateId = template.TemplateId;
                    ProviderGateway.TemplateMappingProvider.Add(
                        new TemplateMappingInfo
                        {
                            AppId = appAccount.AppId,
                            TemplateIdShort = message.TemplateIdShort,
                            TemplateId = template.TemplateId,
                            CreateBy = message.FromUser
                        });
                }
                else
                {
                    message.TemplateId = templateId;
                }
            }

            if (string.IsNullOrWhiteSpace(message.ToOpenId))
            {
                var appUser = ProviderGateway.AppUserAccountProvider.GetByUserId(message.TenantId, Convert.ToInt32(message.ToUser), appAccount.AppId);
                if (appUser == null)
                {
                    result.State = MessageState.Failed;
                    result.ErrMsg = "用户未绑定";
                    return result;
                }
                message.ToOpenId = appUser.OpenId;
            }

            var jsondata = new StringBuilder();
            jsondata.Append("{");
            foreach (var detail in message.Content.Detail)
            {
                jsondata.Append("\"" + detail.Key + "\":{\"value\":\"" + detail.Value + "\"},");
            }
            jsondata.Remove(jsondata.Length - 1, 1);
            jsondata.Append("}");

            var picUrl = MediaDataHelper.GetPicUrl(message.ProductId, appAccount.Type.ToString());
            AppConnectLogHelper.DebugFormat("调用微信服务号发送消息:Token{0};ToOpenId{1}; AgentId{2} ContentUrl{3}, ContentTitle{4}, description:{5}, pic:{6}", token, message.ToOpenId, appAccount.AgentId, message.Content.Url, message.Content.Title, jsondata.ToString(), picUrl);
            if (string.IsNullOrEmpty(message.Content.Url))
            {
                message.Content.Url = AppConnectHostConfig.Cache[0] + string.Format("/Common/Error?title={0}&message={1}", HttpUtility.UrlEncode("友情提醒"), HttpUtility.UrlEncode("抱歉，此消息暂不支持在此查看，请在电脑浏览器中查看"));
            }
            var sendResult = Template.SendTemplateMessageForJson(token, message.ToOpenId, message.TemplateId, "", message.Content.Url, jsondata.ToString());
            
            if (sendResult.ErrCode != 0)
            {
                result.State = MessageState.Failed;
                result.ErrMsg = SerializeHelper.Serialize(sendResult);
                AppConnectLogHelper.ErrorFormat("企业微信发送消息失败,errorCode:{0},errorMsg:{1}", sendResult.ErrCode, sendResult.ErrMsg);
                return result;
            }

            result.State = MessageState.Success;
            result.MessageId = sendResult.MsgId.ToString();
            result.ErrMsg = SerializeHelper.Serialize(sendResult);
            return result;

        }
    }
}

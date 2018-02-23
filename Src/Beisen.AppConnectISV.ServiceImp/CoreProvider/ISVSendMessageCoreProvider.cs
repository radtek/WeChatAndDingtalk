using Beisen.AppConnectISV.BusinessCore;
using Beisen.AppConnectISV.BusinessCore.DingDing;
using Beisen.AppConnectISV.BusinessCore.RemoteConfiguration;
using Beisen.AppConnectISV.Infrastructure;
using Beisen.AppConnectISV.Model.BusinessEnum;
using Beisen.AppConnectISV.Model.BusinessModel;
using Beisen.AppConnectISV.Model.HttpModel;
using Beisen.MultiTenant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Beisen.Common.Context;
using Beisen.AppConnectISV.Model;

namespace Beisen.AppConnectISV.ServiceImp.CoreProvider
{
    public class ISVSendMessageCoreProvider
    {
        #region Singleton 
        static readonly ISVSendMessageCoreProvider _Instance = new ISVSendMessageCoreProvider();
        public static ISVSendMessageCoreProvider Instance
        {
            get
            {
                return _Instance;
            }
        }
        #endregion

        public SendMessage_Result SendMessageDingTalk(int tenantId, int userId, MessageInfoArgument messageInfoArgument)
        {
            SendMessage_Result sendMessage_Result = new SendMessage_Result();
            MessageRecord messageRecord = new MessageRecord();

            VaildMessageType(tenantId, userId, messageInfoArgument);
            BuildModelForMessageInfoArgument(tenantId, userId, messageRecord, messageInfoArgument);
            var systemUserInfos = ISVLoginProvider.Instance.GetSystemUserInfosForDefault(tenantId, messageRecord.ToUserIds.ToArray());
            if (systemUserInfos != null && systemUserInfos.Count > 0)
            {
                //获取未登录的人员ID
                var portionUserIds = GetNotLoginUser(messageRecord.ToUserIds, systemUserInfos);
                if (portionUserIds.Any())
                {
                    messageRecord.NotReceiverUserIds = portionUserIds;
                }
                BuildModelForUserInfoMapping(messageRecord, systemUserInfos);
                LogHelper.Instance.Dump(string.Format("ISV SendMessage MessageRecord：{0}", JsonConvert.SerializeObject(messageRecord)));
                sendMessage_Result = DingDingMethod.Instance.SendOAMessage(messageRecord.CorpId, messageRecord.AgentId, messageRecord.FromUserName, string.Join("|", messageRecord.ToMappingUserId), messageRecord.RedirectUrlMobile, messageRecord.RedirectUrlPC, messageRecord.Title, messageRecord.Content, messageRecord.ImageUrl);
            }
            else
            {
                messageRecord.NotReceiverUserIds = messageRecord.ToUserIds;
            }
            LogHelper.Instance.Dump(string.Format("ISV SetMessageRecord MessageRecord：{0}", JsonConvert.SerializeObject(messageRecord)));
            SetMessageRecord(messageRecord, sendMessage_Result);
            return sendMessage_Result;
        }
        #region Vail
        private void VaildMessageType(int tenant_id, int user_id, MessageInfoArgument message)
        {
            if (message == null)
            {
                throw new Exception("MessageInfoArgument is null or empty!");
            }
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
        #endregion

        #region  BuildModel
        private void BuildModelForMessageInfoArgument(int tenantId, int userId, MessageRecord messageRecord, MessageInfoArgument messageInfoArgument)
        {
            messageRecord.BatchId = Guid.NewGuid().ToString();
            messageRecord.ToUserIds = messageInfoArgument.receiverId.Split('|').ToList();
            messageRecord.ProductId = messageInfoArgument.productId;
            messageRecord.Title = messageInfoArgument.content.title;
            messageRecord.Content = MessageContent(messageInfoArgument);
            messageRecord.ContentUrl = messageInfoArgument.content.url;
            messageRecord.ImageUrl = Helper.GetImageUrl(messageInfoArgument.productId);
            messageRecord.ISVTenantId = tenantId;
            messageRecord.FromUser = userId;
            messageRecord.FromUserName = Account.Instance.GetStaffNameById(tenantId, userId);

        }
        private void BuildModelForUserInfoMapping(MessageRecord messageRecord, List<ObjectData> userInfoMappings)
        {
            var UserInfoMapping = userInfoMappings.First();
            messageRecord.AppId = Convert.ToString(UserInfoMapping["AppId"]);
            messageRecord.CorpId = Convert.ToString(UserInfoMapping["CorpId"]);
            messageRecord.AgentId = DingDingMethod.Instance.GetAgentId(messageRecord.CorpId, messageRecord.AppId);
            messageRecord.ReceiverUserIds = userInfoMappings.Select(s => Convert.ToString(s["StaffId"])).ToList();
            messageRecord.ToMappingUserId = userInfoMappings.Select(s => Convert.ToString(s["MappingUserId"])).ToList();
            messageRecord.RedirectUrlMobile = GetRedirectUrl(messageRecord.CorpId, messageRecord.AppId, messageRecord.ContentUrl, (int)DingTalkLoginType.Mobil, messageRecord.ProductId);
            messageRecord.RedirectUrlPC = GetRedirectUrl(messageRecord.CorpId, messageRecord.AppId, messageRecord.ContentUrl, (int)DingTalkLoginType.PC, messageRecord.ProductId);
        }
        private string MessageContent(MessageInfoArgument messageInfoArgument)
        {
            var description = new StringBuilder();
            foreach (var detail in messageInfoArgument.content.detail)
            {
                if (string.IsNullOrWhiteSpace(detail.text))
                {
                    description.AppendFormat("{0}\n", detail.value);
                }
                else
                {
                    description.AppendFormat("{0}：{1}\n", detail.text, detail.value);
                }
            }
            return description.ToString();
        }
        private string GetRedirectUrl(string corpId, string appId, string businessUrl, int loginType, string titaAppId)
        {
            if (string.IsNullOrWhiteSpace(businessUrl))
            {
                //  message.Content.Url = AppConnectHostConfig.Cache[0] + string.Format("/Common/Error?title={0}&message={1}", "跳转错误", "该消息不支持移动端查看,请在电脑端处理");
                businessUrl = DomainInfo.Instance.AppconnectHomePage + string.Format("/Common/Error?title={0}&message={1}", HttpUtility.UrlEncode("友情提醒"), HttpUtility.UrlEncode("抱歉，此消息暂不支持在此查看，请在电脑浏览器中查看"));
            }
            string redirectUrl = ISVInfo.GetBusinessRedirectUrl(corpId, appId, loginType, HttpUtility.UrlEncode(businessUrl), titaAppId);
            return redirectUrl;
        }
        #endregion

        #region GetNotLoginUser
        private List<string> GetNotLoginUser(List<string> receiverIds, List<ObjectData> userMappings)
        {
            List<string> notLoginUserIds = new List<string>();
            if (userMappings == null || userMappings.Count <= 0)
            {
                notLoginUserIds.AddRange(receiverIds);
            }
            else
            {
                receiverIds.ForEach(receiverId =>
                {
                    var userMapping = userMappings.Where(w => Convert.ToString(w["StaffId"]) == receiverId).FirstOrDefault();
                    if (userMapping == null)
                    {
                        notLoginUserIds.Add(receiverId);
                    }

                });
                //userMappings.ForEach(userMapping =>
                //{
                //    var userId = Convert.ToString(userMapping["StaffId"]);
                //    if (!receiverIds.Contains(userId))
                //    {
                //        notLoginUserIds.Add(userId);
                //    }
                //});
            }
            return notLoginUserIds;
        }
        #endregion

        #region SetMessageRecord
        private void SetMessageRecord(MessageRecord messageRecord, SendMessage_Result sendMessage_Result)
        {
            var tenantId = ISVInfo.ISVSystemTenantId;
            var userId = ISVInfo.ISVSystemUserId;
            List<ObjectData> objectDatas = new List<ObjectData>();
            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = tenantId;
            ApplicationContext.Current.UserId = userId;
            var notReceiverUserIds = string.Empty;
            if (messageRecord.NotReceiverUserIds != null && messageRecord.NotReceiverUserIds.Count > 0)
            {
                notReceiverUserIds = string.Join(",", messageRecord.NotReceiverUserIds);
            }
            var metaObject = CloudData.GetMetaObject(tenantId, AppconnectConst.MessageRecordMetaName);
            ObjectData objectData = new ObjectData(metaObject);
            objectData.CreatedBy = userId;
            objectData.CreatedTime = DateTime.Now;
            objectData.ID = Guid.NewGuid();
            objectData.Owner = userId;
            objectData["SuiteKey"] = ISVInfo.SuiteKey;
            objectData["StdIsDeleted"] = false;
            objectData["BatchId"] = messageRecord.BatchId;
            objectData["AppId"] = messageRecord.AppId;
            objectData["CorpId"] = messageRecord.CorpId;
            objectData["AgentId"] = messageRecord.AgentId;
            objectData["ProductId"] = messageRecord.ProductId;
            objectData["ISVTenantId"] = messageRecord.ISVTenantId;
            objectData["FromUserId"] = messageRecord.FromUser;
            if (messageRecord.ToUserIds != null && messageRecord.ToUserIds.Count > 0)
            {
                objectData["ToUserId"] = string.Join(",", messageRecord.ToUserIds);
            }
            if (messageRecord.ReceiverUserIds != null && messageRecord.ReceiverUserIds.Count > 0)
            {
                objectData["ReceiverUserIds"] = string.Join(",", messageRecord.ReceiverUserIds);
            }
            objectData["NotReceiverUserIds"] = notReceiverUserIds;
            if (messageRecord.ToMappingUserId != null && messageRecord.ToMappingUserId.Count > 0)
            {
                objectData["ToMappingUserId"] = string.Join(",", messageRecord.ToMappingUserId);
            }
            objectData["Title"] = messageRecord.Title;
            objectData["Content"] = messageRecord.Content;
            objectData["RedirectUrlMobile"] = messageRecord.RedirectUrlMobile;
            objectData["RedirectUrlPC"] = messageRecord.RedirectUrlPC;
            objectData["ImageUrl"] = messageRecord.ImageUrl;
            objectData["ErrCode"] = sendMessage_Result.ErrCode;
            objectData["ErrMsg"] = sendMessage_Result.ErrMsg;
            objectData["MessageId"] = sendMessage_Result.MessageId;
            objectData["InvalidUser"] = sendMessage_Result.InvalidUser;
            objectData["InvalidParty"] = sendMessage_Result.InvalidParty;
            objectData["ForbiddenUserId"] = sendMessage_Result.ForbiddenUserId;
            objectData["MessageType"] = (int)MappingType.DingTalk;
            if (sendMessage_Result.ErrCode == 0 && string.IsNullOrWhiteSpace(sendMessage_Result.InvalidParty) && string.IsNullOrWhiteSpace(sendMessage_Result.InvalidUser) && string.IsNullOrWhiteSpace(sendMessage_Result.ForbiddenUserId) && string.IsNullOrWhiteSpace(notReceiverUserIds))
            {
                objectData["IsException"] = (int)IsException.False;
                objectData["ResolutionState"] = (int)ResolutionState.Ignore;
            }
            else
            {
                objectData["IsException"] = (int)IsException.True;
                objectData["ResolutionState"] = (int)ResolutionState.Unsolved;
            }
            objectDatas.Add(objectData);
            CloudData.Add(metaObject, objectDatas);
            LogHelper.Instance.Dump(string.Format("EnterInto SetMessageRecord Success BatchId:{0}", messageRecord.BatchId));
        }
        #endregion

    }
}

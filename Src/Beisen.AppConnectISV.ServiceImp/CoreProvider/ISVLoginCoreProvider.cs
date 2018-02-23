using Beisen.AppConnectISV.BusinessCore;
using Beisen.AppConnectISV.BusinessCore.DingDing;
using Beisen.AppConnectISV.Infrastructure;
using Beisen.AppConnectISV.Model;
using Beisen.Common.Context;
using Beisen.MultiTenant.Model;
using Beisen.SearchV3.DSL.Filters;
using Beisen.SearchV3.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Beisen.AppConnectISV.Model.BusinessEnum;

namespace Beisen.AppConnectISV.ServiceImp.CoreProvider
{
    public class ISVLoginCoreProvider
    {
        #region Singleton 
        static readonly ISVLoginCoreProvider _Instance = new ISVLoginCoreProvider();
        public static ISVLoginCoreProvider Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion

        public JsApiInfo GetJsApiInfo(string corpId, string appId, string currentUrl)
        {
            var corpAccessToken = DingDingMethod.Instance.GetCorpAccessToken(corpId);
            var jsApiTicket = DingDingMethod.Instance.GetJsApiTicket(corpId, corpAccessToken);
            //获取AgentId
            var agentId = DingDingMethod.Instance.GetAgentId(corpId, appId);
            //获取生成Sign信息
            var nonce = DingDingMethod.Instance.Nonce();
            var timeStamp = DingDingMethod.Instance.DingTalkTimeStamp();
            var sign = DingDingMethod.Instance.GetSign(jsApiTicket, nonce, timeStamp, currentUrl);
            var jsApiInfo = new JsApiInfo
            {
                Signature = sign,
                Nonce = nonce,
                Url = currentUrl,
                TimeStamp = timeStamp,
                CorpId = corpId,
                AgentId = agentId,
            };
            return jsApiInfo;
        }
        public AccountErrorInfo Login(string userName, string password)
        {
            AccountErrorInfo errorInfo = new AccountErrorInfo();
            var loginInfo = Account.Instance.Login(userName, password, out errorInfo);
            if (loginInfo != null)
            {
                SaveUserInfo(loginInfo);
            }
            return errorInfo;
        }

        public void GetDingTaklUserInfo(string corpId, string code)
        {
            DingDingMethod.Instance.GetDingTaklUserInfo(corpId, code);
        }
        #region Save UserInfo
        public void SaveUserInfo(AccountLoginInfo accountLoginInfo)
        {
            var tenantId = accountLoginInfo.TenantId;
            var userId = accountLoginInfo.UserId;

            var dingTalkUserInfo = Cookie.GetDingTalkUserInfo();
            var isvAuthInfo = Cookie.GetISVAuthInfo();
            //双写为了夸租户查询
            var objectId = SavaUserInfoMapping(tenantId, accountLoginInfo.UserId, accountLoginInfo, dingTalkUserInfo, isvAuthInfo);
            var systemObjectId = SavaUserInfoMapping(ISVInfo.ISVSystemTenantId, ISVInfo.ISVSystemUserId, accountLoginInfo, dingTalkUserInfo, isvAuthInfo);
            //写入 ISVAuthInfo
            isvAuthInfo.ISLogin = true;
            Cookie.SetISVAuthInfo(isvAuthInfo);
            //写入UserInfo
            ISVLoginInfo userCookieInfo = new ISVLoginInfo();
            userCookieInfo.TenantId = accountLoginInfo.TenantId;
            userCookieInfo.UserId = accountLoginInfo.UserId;
            userCookieInfo.UserType = accountLoginInfo.UserType;
            userCookieInfo.SystemObjectId = systemObjectId;
            Cookie.SetISVLoginInfo(userCookieInfo);

            LogHelper.Instance.Dump(string.Format("SaveUserInfo: GetISVAuthInfo{0};ISVLoginInfo:{1}", JsonConvert.SerializeObject(Cookie.GetISVAuthInfo()), JsonConvert.SerializeObject(Cookie.GetISVLoginInfo())));
        }
        private string SavaUserInfoMapping(int tenantId, int userId, AccountLoginInfo accountLoginInfo, DingTalkUserInfo dingTalkUserInfo, ISVAuthInfo isvAuthInfo)
        {
            LogHelper.Instance.Dump(string.Format("SavaUserInfoMapping：tenantId:{0},userId{1},AccountLoginInfo{2},DingTalkUserInfoCookie{3}，ISVAuthInfo{4}", tenantId, userId, JsonConvert.SerializeObject(accountLoginInfo), JsonConvert.SerializeObject(dingTalkUserInfo), JsonConvert.SerializeObject(isvAuthInfo)));

            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = tenantId;
            ApplicationContext.Current.UserId = userId;

            List<ObjectData> objectDatas = new List<ObjectData>();
            var metaObject = CloudData.GetMetaObject(tenantId, AppconnectConst.UserInfoMappingMetaName);
            ObjectData objectData = new ObjectData(metaObject);
            objectData.CreatedBy = accountLoginInfo.UserId;
            objectData.CreatedTime = DateTime.Now;
            objectData.ID = Guid.NewGuid();
            objectData.Owner = accountLoginInfo.UserId;
            objectData["StdIsDeleted"] = false;
            objectData["SuiteKey"] = ISVInfo.SuiteKey;
            objectData["ISVTenantId"] = accountLoginInfo.TenantId;
            objectData["StaffId"] = accountLoginInfo.UserId;
            objectData["UserType"] = accountLoginInfo.UserType;
            objectData["MappingUserId"] = dingTalkUserInfo.DingTalkUserId;
            objectData["AppId"] = isvAuthInfo.AppId;
            objectData["CorpId"] = isvAuthInfo.CorpId;
            objectData["MappingType"] = (int)MappingType.DingTalk;
            objectData["Status"] = (int)ActivateStatus.Activated;

            objectDatas.Add(objectData);
            CloudData.Add(metaObject, objectDatas);

            return objectData.ID.ToString();
        }
        #endregion

        #region  CurrentUserInfo
        public ObjectData CurrentUserInfo(AccountLoginInfo accountLoginInfo, DingTalkUserInfo dingTalkUserInfoCookie)
        {
            var tenantId = accountLoginInfo.TenantId;
            var userId = accountLoginInfo.UserId;
            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = tenantId;
            ApplicationContext.Current.UserId = userId;
            var filter = new BooleanFilter()
             .Must(new TermsFilter(AppconnectConst.UserInfoMapping_ISVTenantId, tenantId))
             .Must(new TermsFilter(AppconnectConst.UserInfoMapping_StaffId, userId))
             .Must(new TermsFilter(AppconnectConst.UserInfoMapping_CorpId, dingTalkUserInfoCookie.CorpId))
             .Must(new TermsFilter(AppconnectConst.UserInfoMapping_DingTalkUserId, dingTalkUserInfoCookie.DingTalkUserId))
             .Must(new TermsFilter(AppconnectConst.UserInfoMapping_SuiteKey, ISVInfo.SuiteKey));
            var data = CloudData.GetEntityAllList(AppconnectConst.UserInfoMappingMetaName, tenantId, filter).FirstOrDefault();
            return data;
        }
        #endregion

        #region  根据MappingId和CorpId查找Italent人员信息
        /// <summary>
        /// 系统级租户映射表,已绑定人员
        /// </summary>
        /// <param name="corpId"></param>
        /// <param name="mappingUserId"></param>
        /// <returns></returns>
        public ObjectData GetSystemUserInfo(string corpId, string mappingUserId)
        {
            var tenantId = ISVInfo.ISVSystemTenantId;
            var userId = ISVInfo.ISVSystemUserId;
            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = tenantId;
            ApplicationContext.Current.UserId = userId;
            var filter = new BooleanFilter()
             .Must(new TermsFilter(AppconnectConst.UserInfoMapping_CorpId, corpId))
              .Must(new TermsFilter(AppconnectConst.UserInfoMapping_Status, (int)ActivateStatus.Activated))
             .Must(new TermsFilter(AppconnectConst.UserInfoMapping_DingTalkUserId, mappingUserId))
             .Must(new TermsFilter(AppconnectConst.UserInfoMapping_SuiteKey, ISVInfo.SuiteKey));
            var data = CloudData.GetEntityAllList(AppconnectConst.UserInfoMappingMetaName, tenantId, filter).FirstOrDefault();
            return data;
        }
        public ObjectData GetSystemUserInfo(string objectId)
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new Exception("GetSystemUserInfo Methoad ObejectId Is null");
            }
            return CloudData.GetEntityListForIds(AppconnectConst.UserInfoMappingMetaName, ISVInfo.ISVSystemTenantId, objectId.Split(',')).FirstOrDefault();
        }
        #endregion
        #region
        public List<ObjectData> GetSystemUserInfos(int tenantId, string[] userIds)
        {
            var currentTenantId = ISVInfo.ISVSystemTenantId;
            var currentUserId = ISVInfo.ISVSystemUserId;
            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = currentTenantId;
            ApplicationContext.Current.UserId = currentUserId;

            var filter = new BooleanFilter()
               //.Must(new TermsFilter(AppconnectConst.UserInfoMapping_CorpId, corpId))
               .Must(new TermsFilter(AppconnectConst.UserInfoMapping_Status, (int)ActivateStatus.Activated))
               .Must(new TermsFilter(AppconnectConst.UserInfoMapping_StaffId, userIds))
               .Must(new TermsFilter(AppconnectConst.UserInfoMapping_SuiteKey, ISVInfo.SuiteKey));
            var data = CloudData.GetEntityAllList(AppconnectConst.UserInfoMappingMetaName, tenantId, filter).ToList();
            if (!data.Any())
            {
                throw new Exception("未在人员映射表中,找到消息接收人,请先登录!");
            }
            return data;

        }
        public List<ObjectData> GetSystemUserInfosOrDefault(int tenantId, string[] userIds)
        {
            var currentTenantId = ISVInfo.ISVSystemTenantId;
            var currentUserId = ISVInfo.ISVSystemUserId;
            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = currentTenantId;
            ApplicationContext.Current.UserId = currentUserId;

            var filter = new BooleanFilter()
               .Must(new TermsFilter(AppconnectConst.UserInfoMapping_Status, (int)ActivateStatus.Activated))
               .Must(new TermsFilter(AppconnectConst.UserInfoMapping_StaffId, userIds))
               .Must(new TermsFilter(AppconnectConst.UserInfoMapping_SuiteKey, ISVInfo.SuiteKey));
            var data = CloudData.GetEntityAllList(AppconnectConst.UserInfoMappingMetaName, tenantId, filter).ToList();
            return data;
        }
        #endregion
        public bool VerifyIsActivate()
        {
            var flag = false;
            var isvAuthInfo = Cookie.GetISVAuthInfo();
            var dingTalkUserInfo = Cookie.GetDingTalkUserInfo();
            LogHelper.Instance.Dump(string.Format("VerifyIsActivate:DingTalkUserInfoCookie：{0}； ISVAuthInfoCookie{1}", JsonConvert.SerializeObject(dingTalkUserInfo), JsonConvert.SerializeObject(isvAuthInfo)), LogType.Debug);

            if (isvAuthInfo != null && isvAuthInfo.ISLogin) // 查询Cookie校验,仅适用第一次登录
            {
                flag = true;
                isvAuthInfo.ISLogin = false;
                Cookie.SetISVAuthInfo(isvAuthInfo);
                LogHelper.Instance.Dump(string.Format("清除后LoginInfo：{0}", JsonConvert.SerializeObject(Cookie.GetISVAuthInfo())), LogType.Debug);
            }
            else // 查询多租赁校验
            {
                string corpId = dingTalkUserInfo.CorpId;
                string mappingUserId = dingTalkUserInfo.DingTalkUserId;
                var userInfo = GetSystemUserInfo(corpId, mappingUserId);
                if (userInfo != null)
                {
                    LogHelper.Instance.Dump(string.Format("VerifyIsActivate:ISV_CurrentUserInfo：{0}", JsonConvert.SerializeObject(userInfo)), LogType.Debug);
                    var status = Convert.ToInt32(userInfo["Status"]);
                    if (status == (int)ActivateStatus.Activated)
                    {
                        ISVLoginInfo userCookieInfo = new ISVLoginInfo();
                        userCookieInfo.TenantId = Convert.ToInt32(userInfo["ISVTenantId"]);
                        userCookieInfo.UserId = Convert.ToInt32(userInfo["StaffId"]);
                        userCookieInfo.UserType = Convert.ToInt32(userInfo["UserType"]);
                        userCookieInfo.SystemObjectId = userInfo.ID.ToString();
                        Cookie.SetISVLoginInfo(userCookieInfo);
                        flag = true;
                    }
                }
            }
            return flag;
        }
    }
}

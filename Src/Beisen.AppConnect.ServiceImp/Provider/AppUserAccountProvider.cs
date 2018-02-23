using System;
using System.Collections.Generic;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceImp.Persistance;
using Beisen.AppConnect.ServiceImp.Provider.Proxy;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.BeisenUser.Model;
using Beisen.Common.HelperObjects;
using System.Linq;
using Newtonsoft.Json;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    public class AppUserAccountProvider : IAppUserAccountProvider
    {
        #region 单例

        private static readonly IAppUserAccountProvider _instance = new AppUserAccountProvider();

        public static IAppUserAccountProvider Instance
        {
            get { return _instance; }
        }

        private AppUserAccountProvider()
        {
        }

        #endregion
        public static string TitaApiUrl = Configuration.ConfigManager.AppSettings["TitaApiUrl"];
        /// <summary>
        /// 增加用户账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="appUserAccountInfo"></param>
        public int AddOrUpdate(int tenantId, AppUserAccountInfo appUserAccountInfo)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertNotNull(appUserAccountInfo, "appUserAccountInfo is null");
            ArgumentHelper.AssertNotNullOrEmpty(appUserAccountInfo.AppId, "appUserAccountInfo.appId is null");
            ArgumentHelper.AssertNotNullOrEmpty(appUserAccountInfo.OpenId, "appUserAccountInfo.openId is null");
            ArgumentHelper.AssertIsTrue(appUserAccountInfo.TenantId > 0, "appUserAccountInfo.TenantId is 0");
            ArgumentHelper.AssertIsTrue(appUserAccountInfo.UserId > 0, "appUserAccountInfo.UserId is 0");
            var sqlId = AppUserAccountDao.InsertOrUpdate(tenantId, appUserAccountInfo);
            AppUserAccountDao.AddOrUpdateCLoud(tenantId, sqlId, appUserAccountInfo);
            return sqlId;

        }
        /// <summary>
        /// 根据Id获取用户账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public AppUserAccountInfo GetById(int tenantId, int id)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertIsTrue(id > 0, "id is 0");

            return AppUserAccountDao.GetById(tenantId, id);
        }

        /// <summary>
        /// 根据OpenId获取用户
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public AppUserAccountInfo GetByOpenId(string appId, string openId)
        {
            ArgumentHelper.AssertNotNullOrEmpty(appId, "appId is null");
            ArgumentHelper.AssertNotNullOrEmpty(openId, "openId is null");

            return AppUserAccountDao.GetByOpenId(appId, openId);
        }

        /// <summary>
        /// 根据UserId获取用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public AppUserAccountInfo GetByUserId(int tenantId, int userId, string appId)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertIsTrue(userId > 0, "userId is 0");
            ArgumentHelper.AssertNotNullOrEmpty(appId, "appId is null");

            return AppUserAccountDao.GetByUserId(tenantId, userId, appId);
        }

        /// <summary>
        /// 根据TenantId和状态获取用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="appId"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        public List<AppUserAccountInfo> GetByTenantId(int tenantId, string appId, string states)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertNotNullOrEmpty(appId, "appId is null");
            ArgumentHelper.AssertNotNullOrEmpty(states, "states is null");

            return AppUserAccountDao.GetByTenantId(tenantId, appId, states);
        }

        /// <summary>
        /// 更新用户账户状态
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <param name="state"></param>
        public void UpdateState(int tenantId, int id, AppUserAccountState state)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertIsTrue(id > 0, "id is 0");

            AppUserAccountDao.UpdateState(tenantId, id, state);
        }

        /// <summary>
        /// 解绑账户
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        public void UnBind(string appId, string openId)
        {
            ArgumentHelper.AssertNotNullOrEmpty(appId, "appId is null");
            ArgumentHelper.AssertNotNullOrEmpty(openId, "openId is null");

            AppUserAccountDao.UnBind(appId, openId);
        }

        /// <summary>
        /// 解绑账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="ids"></param>
        public void UnBind(int tenantId, int userId, string ids)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertIsTrue(userId > 0, "userId is 0");
            ArgumentHelper.AssertNotNullOrEmpty(ids, "ids is null");
            var appUsers = AppUserAccountDao.GetAppUserList(tenantId, ids);
            if (appUsers != null && appUsers.Count > 0)
            {
                var sqlIds = appUsers.Select(s => s["SqlId"] == null ? "" : s["SqlId"].ToString()).ToList();
                if (sqlIds != null && sqlIds.Count > 0)
                {
                    var convertSqlIds = string.Join<string>(",", sqlIds);
                    AppUserAccountDao.UnbindCloud(tenantId, userId, ids);
                    AppUserAccountDao.UnBind(tenantId, userId, convertSqlIds);
                }

            }
        }

        /// <summary>
        /// 获取用户邮箱
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public string GetUserEmail(string appId, string openId)
        {
            var appUserAccount = GetByOpenId(appId, openId);

            var email = "";
            if (appUserAccount != null && appUserAccount.UserId != 0)
            {
                email = Proxy.BeisenUserProxy.GetUserEmail(appUserAccount.UserId);
            }

            return email;
        }

        /// <summary>
        /// 激活绑定
        /// </summary>
        /// <param name="code"></param>
        public bool Activate(string code)
        {
            ArgumentHelper.AssertNotNullOrEmpty(code, "code is null");

            var args = CryptographyHelper.AESDecrypt(code).Split('_');
            var tenantId = Convert.ToInt32(args[0]);
            var bindBatchId = Convert.ToInt32(args[1]);

            var bindBatch = ProviderGateway.BindBatchProvider.Get(tenantId, bindBatchId);
            if (bindBatch == null || bindBatch.State != BindBatchState.UnUse)
            {
                return false;
            }
            var appUserAccount = GetById(tenantId, bindBatch.AppUserAccountId);
            if (appUserAccount == null || appUserAccount.State != AppUserAccountState.Inactive || appUserAccount.BeisenAccount != bindBatch.BeisenAccount)
            {
                return false;
            }

            UpdateState(tenantId, bindBatch.AppUserAccountId, AppUserAccountState.Activated);
            ProviderGateway.BindBatchProvider.UpdateState(tenantId, bindBatchId, BindBatchState.Used);

            return true;
        }

        /// <summary>
        /// 用户绑定
        /// </summary>
        /// <param name="openIds"></param>
        /// <param name="batch"></param>
        /// <param name="type"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="captcha"></param>
        /// <returns></returns>
        public BindResult Bind(Dictionary<string, string> openIds, string batch, AppUserAccountType type, string userName, string password, string captcha)
        {
            if (openIds == null && openIds.Count <= 0)
            {
                throw new Exception("openIds is empty");
            }
            if (string.IsNullOrWhiteSpace(batch))
            {
                throw new Exception("batch is null or empty");
            }
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new Exception("userName is null or empty");
            }

            userName = userName.Replace(" ", "");
            var handler = BindHandlerFactory.GetHandler(type);
            return handler.Bind(openIds, batch, userName, password, captcha);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        public byte[] GetCaptchaImage(string batch)
        {
            ArgumentHelper.AssertNotNullOrEmpty(batch, "batch is null");

            string captcha;
            var data = CaptchaImageHelper.CreatBitmap(out captcha);
            RedisHelper.SetRedis(RedisConstName.Captcha + batch, captcha, 1800);

            return data;
        }

        /// <summary>
        /// 获取绑定中的状态信息
        /// </summary>
        /// <param name="openIds"></param>
        /// <param name="batch"></param>
        /// <returns>null:表示已经绑定</returns>
        public BindingInfoResult GetBindingInfo(Dictionary<string, string> openIds, string batch)
        {
            ArgumentHelper.AssertIsTrue(openIds != null && openIds.Count > 0, "openIds is empty");
            ArgumentHelper.AssertNotNullOrEmpty(batch, "batch is null");

            var result = new BindingInfoResult
            {
                Type = 0,
                Email = "",
                SendInterval = 0
            };

            var callback = ProviderGateway.CallbackContentProvider.GetByBatchId(batch);
            var appAccountPublic = ProviderGateway.AppAccountProvider.Get(callback.AppAccountPublic);
            var appUserAccount = ProviderGateway.AppUserAccountProvider.GetByOpenId(appAccountPublic.AppId, openIds[appAccountPublic.AppId]);
            if (appUserAccount == null || appUserAccount.State == AppUserAccountState.Unbind)
            {
                return result;
            }

            if (appUserAccount.State == AppUserAccountState.Activated)
            {
                return null;
            }

            result.Type = (int)appUserAccount.Type;
            if (appUserAccount.Type == AppUserAccountType.Email)
            {
                result.Email = appUserAccount.BeisenAccount;
                var lastSendTime = RedisHelper.GetRedis(RedisConstName.SendCheck + appUserAccount.BeisenAccount);
                if (!string.IsNullOrWhiteSpace(lastSendTime))
                {
                    var interval = DateTime.Now.Ticks - Convert.ToInt64(lastSendTime);
                    result.SendInterval = 180 - (int)(interval / 10000000L);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        /// <param name="titaAppId"></param>
        /// <returns></returns>
        public string GetSginQuery(string appId, string openId, int titaAppId)
        {
            var appUserAccount = GetByOpenId(appId, openId);
            var signQuery = ItalentOAuthHelper.GetSginQuery(appUserAccount.TenantId, appUserAccount.UserId, titaAppId);
            return signQuery;
        }
        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public string GetSginQueryV2(string appId, string openId, int titaAppId)
        {
            var appUserAccount = GetByOpenId(appId, openId);
            //调用tita获取sign信息
            string requestUrl = string.Format("{0}/api/v1/{1}/{2}/sign?app_id={3}&sign_type=1", TitaApiUrl, appUserAccount.TenantId, appUserAccount.UserId, titaAppId);
            string resultStr = HttpClientHelper.HttpGet(requestUrl);
            return resultStr;
        }

        /// <summary>
        /// 更新主账户ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="masterAccountId"></param>
        public void UpdateMasterAccountId(int id, int masterAccountId)
        {
            ArgumentHelper.AssertIsTrue(id > 0, "id is 0");
            ArgumentHelper.AssertIsTrue(masterAccountId > 0, "tenantId is 0");

            AppUserAccountDao.UpdateMasterAccountId(id, masterAccountId);
        }

        public List<AppUserForCloud> GetCloudList(int tenantId, string appAccountId, int pageSize, int pageNum, out int total)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertNotNullOrEmpty(appAccountId, "appAccountId is null");

            var result = new List<AppUserForCloud>();
            total = 0;
            var appAccount = ProviderGateway.AppAccountProvider.Get(appAccountId);
            if (appAccount != null)
            {
                var appUserList = AppUserAccountDao.List(tenantId, appAccount.AppId, pageSize, pageNum, out total);

                var userIds = new List<int>();
                foreach (var appUser in appUserList)
                {
                    userIds.Add(appUser.UserId);
                }
                var userLites = BeisenUserProxy.GetUserLiteByUserIds(tenantId, userIds);
                var userDic = new Dictionary<int, UserLite>();
                foreach (var userLite in userLites)
                {
                    if (!userDic.ContainsKey(userLite.UserID))
                    {
                        userDic.Add(userLite.UserID, userLite);
                    }
                }
                foreach (var appUser in appUserList)
                {
                    var userLite = userDic.ContainsKey(appUser.UserId) ? userDic[appUser.UserId] : null;
                    var appUserForCloud = new AppUserForCloud();
                    appUserForCloud.Id = appUser.Id;
                    appUserForCloud.UserName = userLite == null ? "" : userLite.Name;
                    appUserForCloud.UserEmail = userLite == null ? "" : userLite.Email;
                    appUserForCloud.State = appUser.State;
                    appUserForCloud.CreateTime = appUser.CreateTime;
                    appUserForCloud.ActivateTime = appUser.Type == AppUserAccountType.Login ? appUser.CreateTime : appUser.ActivateTime;
                    appUserForCloud.UnbindTime = appUser.UnbindTime;
                    result.Add(appUserForCloud);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取账户状态描述
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public string GetStateName(AppUserAccountState state)
        {
            switch (state)
            {
                case AppUserAccountState.Inactive:
                    return "未激活";
                case AppUserAccountState.Activated:
                    return "已激活";
                case AppUserAccountState.Unbind:
                    return "已解绑";
                default:
                    throw new ArgumentException("未找对应状态");

            }
        }

        public List<AppUserAccountInfo> GetListByUserId(int tenantId, string[] userIds)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertIsTrue(userIds != null && userIds.Length > 0, "userIds is null or empty");

            var resultTemp = new List<AppUserAccountInfo>();
            if (userIds.Length > 100)
            {
                var userIdGroup = new List<string>();
                var group = new List<string>();
                for (var i = 1; i <= userIds.Length; i++)
                {
                    group.Add(userIds[i]);
                    if (i % 100 == 0)
                    {
                        userIdGroup.Add(string.Join("|", group));
                        group = new List<string>();
                    }
                }

                foreach (var ug in userIdGroup)
                {
                    resultTemp.AddRange(AppUserAccountDao.GetListByUserId(tenantId, ug));
                }
            }
            else
            {
                resultTemp.AddRange(AppUserAccountDao.GetListByUserId(tenantId, string.Join(",", userIds)));
            }
            //过滤公共绑定，去除重复
            var appAccountPublic = ProviderGateway.AppAccountProvider.GetPublicByType(12);
            var result = new List<AppUserAccountInfo>();
            foreach (var user in resultTemp)
            {
                if (user.AppId != appAccountPublic.AppId)
                {
                    result.Add(user);
                }
            }

            return result;
        }

        public List<AppUserAccountInfo> GetListByUserId(int tenantId, string[] userIds, string appId)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertIsTrue(userIds != null && userIds.Length > 0, "userIds is null or empty");
            ArgumentHelper.AssertNotNullOrEmpty(appId, "appId is null");

            var result = new List<AppUserAccountInfo>();
            if (userIds.Length > 100)
            {
                var userIdGroup = new List<string>();
                var group = new List<string>();
                for (var i = 1; i <= userIds.Length; i++)
                {
                    group.Add(userIds[i]);
                    if (i % 100 == 0)
                    {
                        userIdGroup.Add(string.Join("|", group));
                        group = new List<string>();
                    }
                }

                foreach (var ug in userIdGroup)
                {
                    result.AddRange(AppUserAccountDao.GetListByUserId(tenantId, ug, appId));
                }
            }
            else
            {
                result.AddRange(AppUserAccountDao.GetListByUserId(tenantId, string.Join(",", userIds), appId));
            }

            return result;
        }

        public List<AppUserAccountInfo> ConvertToUserId(string appAccountId, List<string> openIds)
        {
            ArgumentHelper.AssertNotNullOrEmpty(appAccountId, "appId is null");
            ArgumentHelper.AssertIsTrue(openIds != null && openIds.Count > 0, "openId is null");

            var appAccount = ProviderGateway.AppAccountProvider.Get(appAccountId);
            return AppUserAccountDao.ConvertToUserId(appAccount.TenantId, appAccount.AppId, string.Join(",", openIds));

        }

        public bool DeleteAppUser(int tenantId, int userId)
        {
            ArgumentHelper.AssertPositive(tenantId, "DeleteAppUser tenantId is not positive");
            ArgumentHelper.AssertPositive(userId, "DeleteAppUser userId is not positive");
            int sqlCount = AppUserAccountDao.DeleteAppUser(tenantId, userId);
            if (sqlCount > 0)
            {
                AppUserAccountDao.DeleteCLoud(tenantId, userId);
                return true;
            }
            return false;
        }
    }
}
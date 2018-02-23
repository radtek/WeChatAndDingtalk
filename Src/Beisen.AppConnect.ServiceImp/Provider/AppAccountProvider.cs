using System;
using System.Collections.Generic;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.Common.HelperObjects;
using Beisen.AppConnect.ServiceImp.Persistance;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.AppConnect.Infrastructure;
using Beisen.SearchV3.DSL.Filters;
using Beisen.AppConnect.Infrastructure.Constants;
using System.Linq;
using Beisen.MultiTenant.Data;
using Beisen.DynamicScript.SDK;
using Beisen.MultiTenant.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Interface;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    /// <summary>
    /// 第三方账户信息实现
    /// </summary>
    public class AppAccountProvider : IAppAccountProvider
    {
        #region 单例

        private static readonly IAppAccountProvider _instance = new AppAccountProvider();
        public static IAppAccountProvider Instance
        {
            get { return _instance; }
        }

        private AppAccountProvider()
        {
        }
        #endregion       
        /// <summary>
        /// 增加开放平台帐号信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="info"></param>
        /// <param name="isSaveMultiTenant"></param>
        public string Add(int tenantId, AppAccountInfo info, bool isSaveMultiTenant = true)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "TenantId is 0");
            ArgumentHelper.AssertIsTrue(info != null, "AppAccountInfo is null");
            ArgumentHelper.AssertIsTrue(info.TenantId > 0, "AppAccountInfo.TenantId is 0");
            ArgumentHelper.AssertNotNullOrEmpty(info.Name, "AppAccountInfo.AppAccountName is null or empty");
            ArgumentHelper.AssertNotNullOrEmpty(info.AppId, "AppAccountInfo.AppId is null or empty");
            ArgumentHelper.AssertNotNullOrEmpty(info.AppSecret, "AppAccountInfo.AppSecret is null or empty");

            AppAccountDao.Insert(info);

            if (isSaveMultiTenant && info.TenantId > 1)
            {
                try
                {
                    AppAccountDao.SaveMultiTenant(info);
                }
                catch (Exception ex)
                {
                    AppConnectLogHelper.Error("保存多租赁信息出错，AppAccountId=" + info.AppAccountId, ex);
                }
            }

            return info.AppAccountId;
        }

        /// <summary>
        /// 更新开放平台帐号信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="info"></param>
        /// <param name="isSaveMultiTenant"></param>
        public void Update(int tenantId, AppAccountInfo info, bool isSaveMultiTenant = true)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "TenantId is 0");
            ArgumentHelper.AssertIsTrue(info != null, "AppAccountInfo is null");
            ArgumentHelper.AssertIsTrue(info.TenantId > 0, "AppAccountInfo.TenantId is 0");
            ArgumentHelper.AssertIsTrue(!string.IsNullOrWhiteSpace(info.AppAccountId), "AppAccountInfo.appAccountId is null or empty");
            ArgumentHelper.AssertNotNullOrEmpty(info.Name, "AppAccountInfo.AppAccountName is null or empty");
            ArgumentHelper.AssertNotNullOrEmpty(info.AppId, "AppAccountInfo.AppId is null or empty");
            ArgumentHelper.AssertNotNullOrEmpty(info.AppSecret, "AppAccountInfo.AppSecret is null or empty");

            AppAccountDao.Update(info);

            if (isSaveMultiTenant && info.TenantId > 1)
            {
                try
                {
                    AppAccountDao.SaveMultiTenant(info);
                }
                catch (Exception ex)
                {
                    AppConnectLogHelper.Error("保存多租赁信息出错，AppAccountId=" + info.AppAccountId, ex);
                }
            }
        }
        /// <summary>
        /// 增加或更新开放平台账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="info"></param>
        /// <param name="isSaveMultiTenant"></param>
        public string AddOrUpdate(int tenantId, AppAccountInfo info, bool isSaveMultiTenant = true)
        {
            if (string.IsNullOrEmpty(info.AppAccountId))
            {
                return Add(info.TenantId, info, isSaveMultiTenant);
            }
            else
            {
                var existAppAccount = Get(info.AppAccountId);
                if (existAppAccount == null)
                {
                    return Add(info.TenantId, info, isSaveMultiTenant);
                }
                else
                {
                    Update(info.TenantId, info, isSaveMultiTenant);
                    return existAppAccount.AppAccountId;
                }
            }
        }

        /// <summary>
        /// 获取开放平台帐号信息
        /// </summary>
        /// <param name="appAccountId"></param>
        /// <returns></returns>
        public AppAccountInfo Get(string appAccountId)
        {
            ArgumentHelper.AssertIsTrue(!string.IsNullOrWhiteSpace(appAccountId), "appAccountId is null or empty");

            return AppAccountDao.Get(appAccountId);
        }

        /// <summary>
        /// 获取开放平台帐号信息（从多租赁）
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="appAccountId"></param>
        /// <returns></returns>
        public AppAccountInfo GetMultiTenant(int tenantId, string appAccountId)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "TenantId is 0");
            ArgumentHelper.AssertIsTrue(!string.IsNullOrWhiteSpace(appAccountId), "appAccountId is null or empty");

            return AppAccountDao.GetMultiTenant(tenantId, appAccountId);
        }

        /// <summary>
        /// 更新开放平台帐号信息（多租赁）
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="appAccountId"></param>
        /// <returns></returns>
        public void UpdateMultiTenant(int tenantId, string appAccountId)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "TenantId is 0");
            ArgumentHelper.AssertIsTrue(!string.IsNullOrWhiteSpace(appAccountId), "appAccountId is null or empty");

            var appAccount = Get(appAccountId);
            AppAccountDao.SaveMultiTenant(appAccount);
        }

        /// <summary>
        /// 根据类型获取公共开放平台帐号
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AppAccountInfo GetPublicByType(int type)
        {
            //公共开放平台帐号
            ArgumentHelper.AssertIsTrue(type > 0, "type is null or empty");

            switch (type)
            {
                case 11:
                case 13:
                    type = 12;
                    break;
            }

            return AppAccountDao.GetPubilicByType(type);
        }

        /// <summary>
        /// 更新开放平台账户状态
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="appAccountId"></param>
        /// <param name="state"></param>
        public void UpdateState(int tenantId, int userId, string appAccountId, AppAccountState state)
        {
            ArgumentHelper.AssertIsTrue(!string.IsNullOrWhiteSpace(appAccountId), "id is null or empty");

            AppAccountDao.UpdateState(tenantId, userId, appAccountId, state);
        }

        /// <summary>
        /// 获取账户类型
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAppAccountTypes()
        {
            return AppConnectAppAccountTypeConfig.Cache;
        }

        public AppAccountInfo GetByTag(int tenantId, string tag)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "TenantId is 0");
            ArgumentHelper.AssertIsTrue(!string.IsNullOrWhiteSpace(tag), "tag is null or empty");

            return AppAccountDao.GetByTag(tenantId, tag);
        }

        public string GetAppAccountId(int tenantId, string appAccountId, string tag)
        {
            if (!string.IsNullOrWhiteSpace(appAccountId))
            {
                return appAccountId;
            }
            if (!string.IsNullOrWhiteSpace(tag))
            {
                var appAccountInfo = GetByTag(tenantId, tag);
                if (appAccountInfo != null)
                {
                    return appAccountInfo.AppAccountId;
                }
            }
            return "";
        }

        /// <summary>
        /// 获取账户类型名称
        /// </summary>
        /// <param name="appAccountId"></param>
        /// <returns></returns>
        public string GetAppAccountTypeName(string appAccountId)
        {
            if (string.IsNullOrWhiteSpace(appAccountId))
            {
                return "微信";
            }

            var appAccount = Get(appAccountId);
            if (appAccount.Type == 11 || appAccount.Type == 12)
            {
                return "微信";
            }
            return AppConnectAppAccountTypeConfig.Cache[appAccount.Type];
        }

        public List<AppAccountInfo> GetListByAppId(int tenantId, List<string> appIds)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "TenantId is 0");
            ArgumentHelper.AssertIsTrue(appIds != null && appIds.Count > 0, "appIds is null or empty");

            return AppAccountDao.GetListByAppId(tenantId, string.Join(",", appIds));
        }

        public List<AppAccountInfo> GetListByTenantId(int tenantId)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "TenantId is 0");

            return AppAccountDao.GetListByTenantId(tenantId);
        }
    }
}

using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    internal class MobileBindHandler : IBindHandler
    {
        #region 单例

        protected static readonly IBindHandler _instance = new MobileBindHandler();

        public static IBindHandler Instance
        {
            get { return _instance; }
        }

        private MobileBindHandler()
        {
        }

        #endregion

        private AppUserAccountType _type = AppUserAccountType.Phone;

        public BindResult Bind(Dictionary<string, string> openIds, string batch, string userName, string password, string captcha)
        {
            var result = new BindResult
            {
                NeedCaptcha = false
            };

            var callback = ProviderGateway.CallbackContentProvider.GetByBatchId(batch);
            var appAccountPublic = ProviderGateway.AppAccountProvider.Get(callback.AppAccountPublic);
            if (!openIds.ContainsKey(appAccountPublic.AppId))
            {
                result.Result = 2;
                result.Message = "用户公有OpenId无效";
                return result;
            }
            AppAccountInfo appAccountPrivate = null;
            if (callback.TenantId != 0 && !string.IsNullOrEmpty(callback.AppAccountPrivate))
            {
                appAccountPrivate = ProviderGateway.AppAccountProvider.Get(callback.AppAccountPrivate);
                if (!openIds.ContainsKey(appAccountPrivate.AppId))
                {
                    result.Result = 2;
                    result.Message = "用户私有OpenId无效";
                    return result;
                }
            }
            var openIdPublic = openIds[appAccountPublic.AppId];

            var currentAppUserAccount = ProviderGateway.AppUserAccountProvider.GetByOpenId(appAccountPublic.AppId, openIdPublic);
            if (currentAppUserAccount != null && currentAppUserAccount.State == AppUserAccountState.Activated)
            {
                result.Result = 8;
                result.Message = "您已经绑定过了，无需重复绑定";
                return result;
            }

            //验证绑定时间
            //if (RedisHelper.IsExist(RedisConstName.MobileCheck + userName))
            //{
            //    result.Result = 6;
            //    result.Message = "绑定过于频繁";
            //    return result;
            //}

            var users = ProviderGateway.StaffProvider.GetByUserName(userName);
            if (users == null || users.Count <= 0)
            {
                result.Result = 5;
                result.Message = "未找到对应账户";
                return result;
            }

            var userId = users.First().Key;
            var tenantId = Proxy.BeisenUserProxy.GetTenantId(userId);
            //判断用户是否已经绑定
            var oldAppUserAccount = ProviderGateway.AppUserAccountProvider.GetByUserId(tenantId, userId, appAccountPublic.AppId);
            if (oldAppUserAccount != null && oldAppUserAccount.State == AppUserAccountState.Activated)
            {
                result.Result = 7;
                result.Message = "帐号已绑定";
                return result;
            }

            var newAppUserAccountMaster = new AppUserAccountInfo
            {
                AppId = appAccountPublic.AppId,
                OpenId = openIdPublic,
                TenantId = tenantId,
                UserId = userId,
                BeisenAccount = userName,
                Type = _type,
                State = AppUserAccountState.Activated,
                MasterAccountId = 0
            };
            var masterAccountId = ProviderGateway.AppUserAccountProvider.AddOrUpdate(tenantId, newAppUserAccountMaster);

            //增加私有关联关系
            if ((appAccountPublic.Type == 11 || appAccountPublic.Type == 12) && callback.TenantId != 0 && masterAccountId != 0 && !string.IsNullOrEmpty(callback.AppAccountPrivate))
            {
                var newAppAccountUser = new AppUserAccountInfo
                {
                    AppId = appAccountPrivate.AppId,
                    OpenId = openIds[appAccountPrivate.AppId],
                    TenantId = tenantId,
                    UserId = userId,
                    BeisenAccount = userName,
                    Type = _type,
                    State = AppUserAccountState.Activated,
                    MasterAccountId = masterAccountId
                };
                ProviderGateway.AppUserAccountProvider.AddOrUpdate(tenantId, newAppAccountUser);
            }

            result.RedirectUrl = callback.Content;
            //RedisHelper.SetRedis(RedisConstName.MobileCheck + userName, DateTime.Now.Ticks.ToString(), 300);
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.AppConnect.ServiceInterface.Model.WebModel;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    internal class LoginBindHandler : IBindHandler
    {
        #region 单例

        protected static readonly IBindHandler _instance = new LoginBindHandler();

        public static IBindHandler Instance
        {
            get { return _instance; }
        }

        private LoginBindHandler()
        {
        }

        #endregion

        private AppUserAccountType _type = AppUserAccountType.Login;

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
            if (callback.TenantId != 0)
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

            //判断错误次数
            var errorCount = RedisHelper.GetRedis(RedisConstName.LoginErrorCount + userName);
            var errorCountInt = string.IsNullOrWhiteSpace(errorCount) ? 0 : Convert.ToInt32(errorCount);
            if (errorCountInt > 3)
            {
                //校验验证码
                var code = RedisHelper.GetRedis(RedisConstName.Captcha + batch);
                if (!string.IsNullOrWhiteSpace(code) && string.Compare(code, captcha, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    result.Result = 3;
                    result.Message = "验证码错误";
                    return result;
                }
            }
            //判断用户名密码
            //  var userId = Proxy.BeisenUserProxy.ValidateByEmail(userName, password);
            ResultModel resultModel = new ResultModel();
            var authentication = ProviderGateway.StaffProvider.Login(userName, password, out resultModel);
            if (authentication == null)
            {
                result.Result = resultModel.ErrCode;
                result.Message = resultModel.ErrMsg;
                return result;
            }
            var userId = authentication.UserId;
            if (userId <= 0)
            {
                var count = errorCountInt + 1;
                RedisHelper.SetRedis(RedisConstName.LoginErrorCount + userName, count.ToString(), 1800);

                if (count > 3)
                {
                    result.NeedCaptcha = true;
                    RedisHelper.SetRedis(RedisConstName.Captcha + batch, Guid.NewGuid().ToString(), 1800);
                }
                result.Result = 4;
                result.Message = "用户名密码错误";
                return result;
            }
            //把UserId写入到Cookie中
            var cookie = CookieHelper.GetCookie();
            cookie.AccountUserId = userId;
            CookieHelper.SetCookie(cookie);


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
            if ((appAccountPublic.Type == 11 || appAccountPublic.Type == 12) && callback.TenantId != 0 && masterAccountId != 0)
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

            return result;
        }
    }
}

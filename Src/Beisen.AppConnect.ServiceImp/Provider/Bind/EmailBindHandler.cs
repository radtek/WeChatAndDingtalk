using System;
using System.Collections.Generic;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceImp.Provider.Proxy;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    internal class EmailBindHandler:IBindHandler
    {
        #region 单例

        protected static readonly IBindHandler _instance = new EmailBindHandler();
        public static IBindHandler Instance
        {
            get { return _instance; }
        }

        private EmailBindHandler()
        {
        }

        #endregion

        private AppUserAccountType _type = AppUserAccountType.Email;

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

            //验证发送时间
            if (RedisHelper.IsExist(RedisConstName.SendCheck + userName))
            {
                result.Result = 6;
                result.Message = "绑定过于频繁";
                return result;
            }

            // 校验邮箱是否存在
            var userId = Proxy.BeisenUserProxy.GetSecurityByUserName(userName);
            if (userId <= 0)
            {
                result.Result = 5;
                result.Message = "未找到对应账户";
                return result;
            }
            var tenantId = Proxy.BeisenUserProxy.GetTenantId(userId);

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
                State = AppUserAccountState.Inactive,
                MasterAccountId=0
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
                    State = AppUserAccountState.Inactive,
                    MasterAccountId = masterAccountId
                };
                ProviderGateway.AppUserAccountProvider.AddOrUpdate(tenantId, newAppAccountUser);
            }
            // 记录发邮件记录
            var bindBatch = new BindBatchInfo
            {
                TenantId = tenantId,
                AppUserAccountId = masterAccountId,
                BeisenAccount = userName,
                Type = _type,
                State = BindBatchState.UnUse
            };
            var bindBatchId = ProviderGateway.BindBatchProvider.Add(tenantId, bindBatch);

            // 发送激活邮件
            string activateCode = CryptographyHelper.AESEncrypt(tenantId + "_" + bindBatchId);
            string emailBody = GetEmailBody(userName, AppConnectHostConfig.Cache[0] + HostConst.UserActivate + "?code=" + activateCode);
            var emailBatch = EmailProxy.SendEmail(0, "iTalent登录邮件验证", "", userName, emailBody);
            RedisHelper.SetRedis(RedisConstName.SendCheck + userName, DateTime.Now.Ticks.ToString(), 180);
            ProviderGateway.BindBatchProvider.UpdateBatchId(tenantId, bindBatchId, emailBatch);

            return result;
        }

        private string GetEmailBody(string userName, string url)
        {
            return string.Format(@"<table cellpadding='0' cellspacing='0' border='0' style='padding-left: 32px;'>
                                   <tr><td border='0' height='30px' style='border:0px;'></td></tr>
                                   <tr><td border='0' height='30px'  style='border:0px;'><span>{0}，您好！</span></td></tr>
                                   <tr style='line-height:36px;'><td border='0' height='30px' style='border:0px;'><span>您正在使用iTalent登录，为验证此电子邮件地址属于您，请点击下方链接：</span></td></tr>
                                   <tr style='line-height:36px;'><td border='0' height='30px' style='border:0px;'><a href='{1}'>{1}</a></td></tr>
                                   <tr style='line-height:36px;'><td  border='0' height='30px' style='border:0px;'>如果以上链接无法访问，请将该网址复制并粘贴至新的浏览器窗口中。</td></tr>
                                   <tr style='line-height:36px;'><td  border='0' height='30px' style='border:0px;'>如非本人操作，请忽略。</td></tr>
                                   <tr style='line-height:36px;'><td  border='0' height='30px' style='border:0px;'>此致</td></tr>
                                   <tr style='line-height:36px;'><td  border='0' height='30px' style='border:0px;'>iTalent团队</td></tr>
                                   <tr style='line-height:36px;'><td  border='0' height='30px' style='border:0px;color:gray;'>本邮件由系统自动发出，请勿回复。</td></tr>
                                   </table>", userName, url);
        }

    }
}

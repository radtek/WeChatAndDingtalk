using System;
using System.Collections.Generic;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceImp.Provider.Proxy;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.Common.HelperObjects;
using Beisen.Logging;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    public class AuthorizeProvider : IAuthorizeProvider
    {
        #region 单例

        private static readonly IAuthorizeProvider _instance = new AuthorizeProvider();
        public static IAuthorizeProvider Instance
        {
            get { return _instance; }
        }

        private AuthorizeProvider()
        {
        }

        #endregion

        /// <summary>
        /// 获取开放平台授权URL
        /// </summary>
        /// <param name="appAccountId"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="type">公共服务号类型。12:微信服务号，21：钉钉</param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public string GetAuthorizeUrl(string appAccountId, string redirectUrl, int type, UserCookie cookie, string loginType = null)
        {
            //Log 信息
            //  AppConnectLogHelper.ErrorFormat("进入GetAuthorizeUrl方法!appAccountId:{0},redirectUrl:{1},type:{2}", appAccountId, redirectUrl, type);
            ArgumentHelper.AssertIsTrue(!string.IsNullOrWhiteSpace(redirectUrl), "redirectUrl is null or empty");

            AppAccountInfo appAccountPublic;
            AppAccountInfo appAccountPrivate;
            AppAccountInfo appAccountAuthorize;
            string appAccountPublicId;
            string appAccountPrivateId = null;
            CallbackContentState callbackState;
            var tenantId = 0;

            //根据类型初始化信息
            if (string.IsNullOrWhiteSpace(appAccountId))
            {
                //公共账户方式
                AppConnectLogHelper.Debug("进入GetAuthorizeUrl方法->公共账户方式!");
                appAccountPublic = ProviderGateway.AppAccountProvider.GetPublicByType(type);

                if (!string.IsNullOrWhiteSpace(CookieHelper.GetOpenId(cookie, appAccountPublic.AppId)))
                {
                    return null;
                }

                appAccountPublicId = appAccountPublic.AppAccountId;
                callbackState = CallbackContentState.Public;
                appAccountAuthorize = appAccountPublic;
            }
            else
            {
                //私有账户类型
                AppConnectLogHelper.Debug("进入GetAuthorizeUrl方法->私有账户方式!");
                appAccountPrivate = ProviderGateway.AppAccountProvider.Get(appAccountId);
                if (appAccountPrivate == null)
                {
                    AppConnectLogHelper.Error("AppAccountProvider-GetByAppAccountId 为 null");
                    return null;
                }
                appAccountPrivateId = appAccountPrivate.AppAccountId;
                tenantId = appAccountPrivate.TenantId;

                switch (appAccountPrivate.Type)
                {
                    case 11:
                    case 12:
                        AppConnectLogHelper.Debug("进入GetAuthorizeUrl方法->私有账户方式!->Case11,12");
                        appAccountPublic = ProviderGateway.AppAccountProvider.GetPublicByType(appAccountPrivate.Type);
                        appAccountPublicId = appAccountPublic.AppAccountId;

                        if (!string.IsNullOrWhiteSpace(CookieHelper.GetOpenId(cookie, appAccountPublic.AppId)) && !string.IsNullOrWhiteSpace(CookieHelper.GetOpenId(cookie, appAccountPrivate.AppId)))
                        {
                            return null;
                        }

                        if (!string.IsNullOrWhiteSpace(CookieHelper.GetOpenId(cookie, appAccountPublic.AppId)))
                        {
                            callbackState = CallbackContentState.Private;
                            appAccountAuthorize = appAccountPrivate;
                        }
                        else if (!string.IsNullOrWhiteSpace(CookieHelper.GetOpenId(cookie, appAccountPrivate.AppId)))
                        {
                            callbackState = CallbackContentState.Public;
                            appAccountAuthorize = appAccountPublic;
                        }
                        else
                        {
                            callbackState = CallbackContentState.PrivateAndPublic;
                            appAccountAuthorize = appAccountPublic;
                        }
                        break;
                    default:
                        AppConnectLogHelper.Debug("进入GetAuthorizeUrl方法->私有账户方式!->CaseDefault");
                        if (string.IsNullOrWhiteSpace(CookieHelper.GetOpenId(cookie, appAccountPrivate.AppId)))
                        {
                            callbackState = CallbackContentState.Private;
                            appAccountAuthorize = appAccountPrivate;
                            appAccountPublicId = appAccountPrivate.AppAccountId;
                        }
                        else
                        {
                            AppConnectLogHelper.Debug("进入GetAuthorizeUrl方法->私有账户方式!->CaseDefault->Cookie有值返回Null");
                            return null;
                        }
                        break;
                }
            }
            AppConnectLogHelper.Debug("进入GetAuthorizeUrl方法->私有账户方式!->Case->记录回调信息");
            //记录回调信息
            var batchId = Guid.NewGuid().ToString();
            var callbackContent = new CallbackContentInfo
            {
                BatchId = batchId,
                TenantId = tenantId,
                AppAccountPublic = appAccountPublicId,
                AppAccountPrivate = appAccountPrivateId,
                Content = redirectUrl,
                State = callbackState
            };
            ProviderGateway.CallbackContentProvider.Add(callbackContent);

            var state = Guid.NewGuid().ToString("N");

            AppConnectLogHelper.Debug("进入GetAuthorizeUrl方法->私有账户方式!->Case->记录回调信息->记录Cookie");
            try
            {
                AppConnectLogHelper.Debug("进入GetAuthorizeUrl方法->私有账户方式!->Case->记录回调信息->记录Cookie前->State:" + state);

                CookieHelper.SetState(state);
                var getCookieState = CookieHelper.GetState();
                AppConnectLogHelper.Debug("进入GetAuthorizeUrl方法->私有账户方式!->Case->记录回调信息->记录Cookie后获取->State:" + getCookieState);
            }
            catch (System.Exception ex)
            {
                AppConnectLogHelper.Debug("SetState加日志发生了错误!!!!!");
            }

            //生成授权Url
            var extend = new Dictionary<string, string>
            {
                {TemplateConst.ExtendState, state},
                {TemplateConst.ExtendBatch, batchId}
            };

            string url = null;
            //钉钉的PC登录
            if (!string.IsNullOrEmpty(loginType) && loginType == "0")
            {
                var requestTemplate = new DefaultApiTemplate(appAccountAuthorize, TemplateConst.AuthorizePCUrl, extend);
                url = requestTemplate.GetRequestUrl();
            }
            else
            {
                var requestTemplate = new DefaultApiTemplate(appAccountAuthorize, TemplateConst.AuthorizeUrl, extend);
                url = requestTemplate.GetRequestUrl();
            }
            AppConnectLogHelper.Debug("进入GetAuthorizeUrl方法->私有账户方式!->Case->记录回调信息->记录Cookie后获取->State->Url" + url);
            return url;
        }

        /// <summary>
        /// 获取用户身份并返回回调地址
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="batch"></param>
        /// <returns></returns>
        public string GetRedirectUrl(string code, string state, string batch)
        {
            ArgumentHelper.AssertIsTrue(!string.IsNullOrWhiteSpace(batch), "batch is null or empty");
            ArgumentHelper.AssertIsTrue(!string.IsNullOrWhiteSpace(state), "state is null or empty");

            var callbackContent = ProviderGateway.CallbackContentProvider.GetByBatchId(batch);

            //获取回调地址
            var handler = CallbackStateFactory.GetIntance(callbackContent);
            var url = handler.GetRedirectUrl(callbackContent, code, state);

            return url;
        }

        /// <summary>
        /// 校验用户身份，并返回绑定Url
        /// </summary>
        /// <param name="appAccountId"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="type"></param>
        /// <param name="cookie"></param>
        /// <returns>
        /// null：用户身份存在且状态正常
        /// 非null：返回绑定Url，直接跳转
        /// </returns>
        public string GetIdentityUrl(string appAccountId, string redirectUrl, int type, UserCookie cookie, CallbackContentState state = CallbackContentState.Bind)
        {
            ArgumentHelper.AssertIsTrue(!string.IsNullOrWhiteSpace(redirectUrl), "redirectUrl is null or empty");

            AppAccountInfo appAccountPublic;
            AppAccountInfo appAccountPrivate = null;
            var tenantId = 0;
            AppUserAccountInfo appUserAccountMaster;
            AppUserAccountInfo appUserAccount;
            string appAccountPublicId;

            //根据类型初始化信息
            if (string.IsNullOrWhiteSpace(appAccountId))
            {
                //公共账户方式
                appAccountPublic = ProviderGateway.AppAccountProvider.GetPublicByType(type);
                appUserAccountMaster = ProviderGateway.AppUserAccountProvider.GetByOpenId(appAccountPublic.AppId, cookie.OpenIds[appAccountPublic.AppId]);
                appAccountPublicId = appAccountPublic.AppAccountId;

                if (appUserAccountMaster != null && appUserAccountMaster.State == AppUserAccountState.Activated)
                {
                    cookie.AccountAppId = appAccountPublic.AppId;
                    cookie.AccountOpenId = cookie.OpenIds[appAccountPublic.AppId];
                    return null;
                }
            }
            else
            {
                //私有账户方式
                appAccountPrivate = ProviderGateway.AppAccountProvider.Get(appAccountId);
                tenantId = appAccountPrivate.TenantId;

                switch (appAccountPrivate.Type)
                {
                    case 11:
                    case 12:
                        appAccountPublic = ProviderGateway.AppAccountProvider.GetPublicByType(appAccountPrivate.Type);
                        appUserAccountMaster = ProviderGateway.AppUserAccountProvider.GetByOpenId(appAccountPublic.AppId, cookie.OpenIds[appAccountPublic.AppId]);
                        appUserAccount = ProviderGateway.AppUserAccountProvider.GetByOpenId(appAccountPrivate.AppId, cookie.OpenIds[appAccountPrivate.AppId]);
                        appAccountPublicId = appAccountPublic.AppAccountId;

                        if (appUserAccountMaster == null)
                        {
                            if (appUserAccount != null)
                            {
                                //如果与其他账户冲突
                                var appUserAccoutTemp = ProviderGateway.AppUserAccountProvider.GetByUserId(appUserAccount.TenantId, appUserAccount.UserId, appAccountPublic.AppId);
                                if (appUserAccoutTemp == null)
                                {
                                    //补充主账户
                                    var appUserAccountMasterNew = new AppUserAccountInfo();
                                    appUserAccountMasterNew.AppId = appAccountPublic.AppId;
                                    appUserAccountMasterNew.OpenId = cookie.OpenIds[appAccountPublic.AppId];
                                    appUserAccountMasterNew.TenantId = appUserAccount.TenantId;
                                    appUserAccountMasterNew.UserId = appUserAccount.UserId;
                                    appUserAccountMasterNew.BeisenAccount = BeisenUserProxy.GetUserEmail(appUserAccount.UserId);
                                    appUserAccountMasterNew.Type = AppUserAccountType.Email;
                                    appUserAccountMasterNew.State = AppUserAccountState.Activated;
                                    appUserAccountMasterNew.MasterAccountId = 0;
                                    var appUserAccountMasterNewId = ProviderGateway.AppUserAccountProvider.AddOrUpdate(appUserAccountMasterNew.TenantId, appUserAccountMasterNew);
                                    ProviderGateway.AppUserAccountProvider.UpdateMasterAccountId(appUserAccount.Id, appUserAccountMasterNewId);

                                    cookie.AccountAppId = appAccountPublic.AppId;
                                    cookie.AccountOpenId = cookie.OpenIds[appAccountPublic.AppId];

                                    return null;
                                }
                            }
                        }
                        else if (appUserAccountMaster.State == AppUserAccountState.Activated)
                        {
                            if (appUserAccount == null)
                            {
                                //补充账户信息
                                var appUserAccountNew = new AppUserAccountInfo();
                                appUserAccountNew.AppId = appAccountPrivate.AppId;
                                appUserAccountNew.OpenId = cookie.OpenIds[appAccountPrivate.AppId];
                                appUserAccountNew.TenantId = appUserAccountMaster.TenantId;
                                appUserAccountNew.UserId = appUserAccountMaster.UserId;
                                appUserAccountNew.BeisenAccount = appUserAccountMaster.BeisenAccount;
                                appUserAccountNew.Type = appUserAccountMaster.Type;
                                appUserAccountNew.State = appUserAccountMaster.State;
                                appUserAccountNew.MasterAccountId = appUserAccountMaster.Id;
                                ProviderGateway.AppUserAccountProvider.AddOrUpdate(appUserAccountNew.TenantId, appUserAccountNew);
                            }
                            else if (appUserAccount.MasterAccountId != appUserAccountMaster.Id)
                            {
                                //更新MasterId
                                ProviderGateway.AppUserAccountProvider.UpdateMasterAccountId(appUserAccount.Id, appUserAccountMaster.Id);
                            }
                            cookie.AccountAppId = appAccountPublic.AppId;
                            cookie.AccountOpenId = cookie.OpenIds[appAccountPublic.AppId];
                            return null;
                        }
                        break;
                    default:
                        appUserAccountMaster = ProviderGateway.AppUserAccountProvider.GetByOpenId(appAccountPrivate.AppId, cookie.OpenIds[appAccountPrivate.AppId]);
                        appAccountPublicId = appAccountPrivate.AppAccountId;
                        if (appUserAccountMaster != null && appUserAccountMaster.State == AppUserAccountState.Activated)
                        {
                            cookie.AccountAppId = appAccountPrivate.AppId;
                            cookie.AccountOpenId = cookie.OpenIds[appAccountPrivate.AppId];
                            return null;
                        }
                        break;
                }

            }
            //记录回调信息
            var batchId = Guid.NewGuid().ToString();
            var callbackContent = new CallbackContentInfo
            {
                BatchId = batchId,
                TenantId = tenantId,
                AppAccountPublic = appAccountPublicId,
                AppAccountPrivate = appAccountPrivate == null ? null : appAccountPrivate.AppAccountId,
                Content = redirectUrl,
                State = state
            };
            ProviderGateway.CallbackContentProvider.Add(callbackContent);

            if (state == CallbackContentState.Bind)
            {
                return UrlHelper.AddParameter(HostConst.UserBind, "batch", batchId);
            }
            else
            {
                var parameter = redirectUrl.Split('?');
                var query = parameter.Length > 0 ? string.Format("{0}&batch={1}", parameter[1], batchId) : string.Empty;
                return UrlHelper.AddQuery(HostConst.RegisterBind, query);
            }
        }

        /// <summary>
        /// 验证用户状态，用户接口请求验证
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public bool VerifyUser(UserCookie cookie)
        {
            if (string.IsNullOrWhiteSpace(cookie.AccountAppId) || string.IsNullOrWhiteSpace(cookie.AccountOpenId))
            {
                return false;
            }
            var appUserAccount = ProviderGateway.AppUserAccountProvider.GetByOpenId(cookie.AccountAppId, cookie.AccountOpenId);
            if (appUserAccount == null || appUserAccount.State != AppUserAccountState.Activated)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 校验应用
        /// </summary>
        /// <param name="titaAppId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool CheckApp(int titaAppId, string url)
        {
            AppConnectLogHelper.DebugFormat("检查App--titaAppId:{0},Url:{1}", titaAppId, url);
            if (!AppConnectTitaAppHostMappingConfig.AppKeyCache.ContainsKey(titaAppId))
            {
                return false;
            }
            if (!string.IsNullOrWhiteSpace(url))
            {
                if (!AppConnectTitaAppHostMappingConfig.HostCache.ContainsKey(titaAppId))
                {
                    return false;
                }
                var host = new Uri(url).Host.ToUpperInvariant();
                AppConnectLogHelper.DebugFormat("检查App--host:{0}", host);
                if (!AppConnectTitaAppHostMappingConfig.HostCache[titaAppId].Contains(host))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

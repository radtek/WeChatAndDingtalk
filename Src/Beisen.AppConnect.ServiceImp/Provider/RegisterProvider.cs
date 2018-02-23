using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.BeisenUser.ServiceImp;
using Beisen.Common.HelperObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    public class RegisterProvider : IRegisterProvider
    {
        #region 单例

        private static readonly IRegisterProvider _instance = new RegisterProvider();
        public static IRegisterProvider Instance
        {
            get { return _instance; }
        }

        private RegisterProvider()
        {
        }
        #endregion

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <param name="codeType"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="inviteUserId"></param>
        /// <param name="message"></param>
        /// <param name="registerType"></param>
        /// <returns></returns>
        public bool Register(int tenantId, string mobile, int code, int codeType, string userName, string password, int inviteUserId, out string message, int registerType = 1)
        {
            ArgumentHelper.AssertPositive(tenantId, "tenantId is 0");
            ArgumentHelper.AssertPositive(inviteUserId, "inviteUserId is 0");
            ArgumentHelper.AssertNotNullOrEmpty(mobile, "mobile is null");
            ArgumentHelper.AssertNotNullOrEmpty(userName, "userName is null");
            ArgumentHelper.AssertNotNullOrEmpty(password, "password is null");

            message = string.Empty;

            var registerUserInfo = new RegisterUserInfo
            {
                TenantId = tenantId,
                //Email = GetVirtualEmail(mobile),
                Name = userName,
                Password = password,
                InviteUser = inviteUserId,
                Mobile = mobile,
                RegisterType = registerType
            };

            if (Validate(registerUserInfo, code, codeType, out message))
            {
                var userId = ProviderGateway.StaffProvider.CreateStaff(registerUserInfo);
                message = userId > 0 ? "注册成功" : "注册失败";
                return userId > 0;
            }

            return false;
        }

        /// <summary>
        /// 获取绑定批次Id
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        public string GetBindBatchId(int tenantId, string appAccountId, int type, string redirectUrl)
        {
            var appAccountPublic = new AppAccountInfo();
            var appAccountPublicId = string.Empty;
            AppAccountInfo appAccountPrivate = null;
            
            //根据类型初始化信息
            if (string.IsNullOrWhiteSpace(appAccountId))
            {
                //公共账户方式
                appAccountPublic = ProviderGateway.AppAccountProvider.GetPublicByType(type);
                appAccountPublicId = appAccountPublic.AppAccountId;
            }
            else
            {
                //私有账户方式
                appAccountPrivate = ProviderGateway.AppAccountProvider.Get(appAccountId);

                if (tenantId != 0 && tenantId != appAccountPrivate.TenantId)
                {
                    return string.Empty;
                }

                tenantId = appAccountPrivate.TenantId;
                switch (appAccountPrivate.Type)
                {
                    case 11:
                    case 12:
                        appAccountPublic = ProviderGateway.AppAccountProvider.GetPublicByType(appAccountPrivate.Type);
                        appAccountPublicId = appAccountPublic.AppAccountId;
                        break;
                    default:
                        appAccountPublicId = appAccountPrivate.AppAccountId;
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
                State = CallbackContentState.RegisterBind
            };
            ProviderGateway.CallbackContentProvider.Add(callbackContent);

            return batchId;
        }

        /// <summary>
        /// 验证是否可以注册用户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="code"></param>
        /// <param name="codeType"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private static bool Validate(RegisterUserInfo info, int code, int codeType, out string message)
        {
            message = string.Empty;

            if (string.IsNullOrWhiteSpace(info.Password) || info.Password.Length < 6)
            {
                message = "参数错误";
                return false;
            }

            var checkEmail = ProviderGateway.StaffProvider.CheckUserNameExist(info.Mobile);
            if (checkEmail)
            {
                message = "该手机号已注册";
                return false;
            }

            var checkResult =
                ProviderGateway.MobileVerificationProvider.CheckCode(info.Mobile,
                    code, codeType);

            if (!checkResult)
            {
                message = "验证码错误";
                return false;
            }

            var user =
                ProviderGateway.StaffProvider.GetByUserName(info.RegisterType == 0
                    ? info.Email
                    : info.Mobile);
            if (user != null && user.Count > 0)
            {
                message = string.Format("用户{0}已存在", info.Mobile);
                return false;
            }

            var tenant = BeisenUserGateway.BeisenTenantProvider.GetTenantById(info.TenantId);
            if (tenant == null)
            {
                message = string.Format("租户{0}不存在", info.TenantId);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 生成虚拟邮箱
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        private static string GetVirtualEmail(string accountName)
        {
            return string.Format("{0}@titavirtual.tt", accountName.Replace("@", ""));
        }
    }
}

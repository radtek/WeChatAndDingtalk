using Beisen.AppConnectISV.Infrastructure;
using Beisen.AppConnectISV.Model;
using Beisen.UserFramework.Exceptions;
using Beisen.UserFramework.Models;
using Beisen.UserFramework.SDK.Account;
using Beisen.UserFramework.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Beisen.AppConnectISV.BusinessCore
{
    public class Account
    {
        #region Singleton 
        static readonly Account _Instance = new Account();
        public static Account Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AccountLoginInfo Login(string userName, string password, out AccountErrorInfo accountErrorInfo)
        {
            AccountLoginInfo accountLoginInfo = new AccountLoginInfo();
            accountErrorInfo = new AccountErrorInfo();
            var args = new LoginArgs
            {
                LoginName = userName,
                Password = password,

            };
            var options = LoginOptions.Default;
            try
            {
                var result = AccountService.Instance.Login(args, options);
                if (!result.Ok)
                {
                    throw result.Error;
                }
                accountErrorInfo.ErrCode = 1;
                accountLoginInfo.TenantId = result.State[0].TenantId;
                accountLoginInfo.UserId = result.State[0].UserId;
                accountLoginInfo.UserType = result.State[0].UserType;
                return accountLoginInfo;
            }
            catch (LoginNameNotExistException ex)
            {
                accountErrorInfo.ErrCode = -1;
                accountErrorInfo.ErrMsg = "账户或密码错误";
                return null;
            }
            catch (PasswordInvalidException ex)
            {
                accountErrorInfo.ErrCode = -1;
                accountErrorInfo.ErrMsg = "账户或密码错误";
                if (ex.MaxFailedTimes != -1)
                {
                    accountErrorInfo.ErrMsg = "账户或密码错误，{ex.MaxFailedTimes}次输入错误后账户将被锁定";
                }
                return null;
            }
            catch (StaffDimissionException ex)
            {

                accountErrorInfo.ErrCode = -2;
                accountErrorInfo.ErrMsg = ex.Message;
                return null;
            }
            catch (TenantDisabledException ex)
            {
                accountErrorInfo.ErrCode = -3;
                accountErrorInfo.ErrMsg = ex.Message;
                return null;
            }
            catch (PasswordExpiredException ex)
            {
                accountErrorInfo.ErrCode = -9;
                accountErrorInfo.ErrMsg = ex.Message;
                return null;
            }
            catch (PasswordStrengthException ex)
            {
                accountErrorInfo.ErrCode = -10;
                accountErrorInfo.ErrMsg = ex.Message;
                return null;
            }
            catch (LoginIntervalOverlongException ex)
            {
                accountErrorInfo.ErrCode = -14;
                accountErrorInfo.ErrMsg = ex.Message;
                return null;
            }
            catch (UserDisabledException ex)
            {
                accountErrorInfo.ErrCode = -14;
                accountErrorInfo.ErrMsg = ex.Message;
                return null;
            }
            catch (AccountLockedException ex)
            {
                accountErrorInfo.ErrCode = -15;
                //账号已被锁定，请点击忘记密码或30分钟后重试
                //账号已被锁定，请30分钟后重试
                accountErrorInfo.ErrMsg = ex.Message.Replace("点击忘记密码或", "");
                return null;
            }
            catch (NoTenantAvailableException ex)
            {
                accountErrorInfo.ErrCode = -1;
                accountErrorInfo.ErrMsg = ex.Message;
                return null;
            }
            catch (System.Exception ex)
            {
                accountErrorInfo.ErrCode = 0;
                accountErrorInfo.ErrMsg = "登录失败;" + ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 根据userId获取员工信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private StaffDto GetStaffByIds(int tenantId, int userId)
        {
            var args = new StaffsGetByIdsArgs
            {
                TenantId = tenantId,
                UserIds = new int[] { userId }
            };
            var options = QueryOptions.Default;
            options.IgnoreOperator = true;

            var provider = StaffService.Instance;
            if (provider == null)
            {
                LogHelper.Instance.Dump("UserFramework ESB Service Is null", LogType.Error);
                return null;
            }
            var result = provider.GetStaffByIds(args, options);
            if (result.Code != 200)
            {
                throw new Exception(string.Format("Code:{0},Message:{1}", result.Code, result.Message));
            }
            return result.Total > 0 ? result.Items[0] : null;
        }
        public string GetStaffNameById(int tenantId, int userId)
        {
            var staffName = string.Empty;
            var staffDto = GetStaffByIds(tenantId, userId);
            if (staffDto != null)
            {
                staffName = staffDto.Name;
            }
            return staffName;
        }
    }
}

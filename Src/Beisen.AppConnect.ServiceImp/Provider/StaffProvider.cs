using Beisen.AppConnect.Infrastructure.Exceptions;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.AppConnect.ServiceInterface.Model.WebModel;
using Beisen.ESB.Client;
using Beisen.UserFramework.Exceptions;
using Beisen.UserFramework.Models;
using Beisen.UserFramework.SDK.Account;
using Beisen.UserFramework.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using IUserAccountProvider = Beisen.AppConnect.ServiceInterface.Interface.IStaffProvider;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    public class StaffProvider : IUserAccountProvider
    {
        #region 单例

        private static readonly IUserAccountProvider _instance = new StaffProvider();
        public static IUserAccountProvider Instance
        {
            get { return _instance; }
        }

        private StaffProvider()
        {
        }
        #endregion

        /// <summary>
        /// 校验用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckUserNameExist(string userName)
        {
            try
            {
                // var provider = PlatformServiceFactory<IAccountProvider>.Instance();
                var provider = AccountService.Instance;
                if (provider == null)
                {
                    AppConnectLogHelper.Error(new ESBServiceNotFoundException("UserFramework ESB Service Is null"));
                    return false;
                }

                var args = new LoginNameCheckNotTakenArgs
                {
                    LoginName = userName
                };

                var result = provider.CheckLoginNameNotTaken(args);

                return !result.Ok;
            }
            catch (System.Exception ex)
            {
                AppConnectLogHelper.Error(ex.Message, new UserOperateException(string.Format("校验UserName失败,接口：AccountService.Instance.CheckLoginNameNotTaken,参数：userName:{0}", userName), ex));
                return false;
            }
        }

        /// <summary>
        /// 创建员工
        /// </summary>
        /// <returns></returns>
        public int CreateStaff(RegisterUserInfo info, int operatorId = 0)
        {
            var args = new StaffCreateArgs
            {
                TenantId = info.TenantId,
                Origin = (int)RegisterOriginState.WeChat,
                Staff = new Staff
                {
                    Email = info.Email,
                    Name = info.Name,
                    Mobile = info.Mobile,
                    WorkPhone = info.WorkPhone,
                    Position = info.Position,
                    Hobbies = info.DepartmentName,
                    EmployedDate = DateTime.Now
                },
                Account = new Account
                {
                    UserName = info.RegisterType == 1 ? null : info.Email,
                    Password = info.Password,
                    BindingMobile = info.RegisterType == 1 ? info.Mobile : null
                },
                OperatorId = 0
            };
            var options = StaffCreateOptions.CreateAccountAndActivate;
            options.IgnoreOperator = operatorId == 0;
            options.UseVirtualUserName = info.RegisterType == 1;
            try
            {
                var provider = Beisen.UserFramework.Service.StaffService.Instance;
                //   var provider = PlatformServiceFactory<UserFramework.ESB.ServiceInterface.IStaffProvider>.Instance();
                if (provider == null)
                {
                    AppConnectLogHelper.Error(new ESBServiceNotFoundException("UserFramework ESB Service Is null"));
                    return 0;
                }

                var result = provider.CreateStaff(args, options);
                if (!result.Ok)
                {
                    throw new UserOperateException(string.Format("Message:{0}", result.Error.Message));
                }
                return result.State;
            }
            catch (System.Exception ex)
            {
                AppConnectLogHelper.Error(ex.Message, new UserOperateException("创建员工失败,接口：StaffService.Instance.CreateStaff", ex));
                return 0;
            }
        }

        /// <summary>
        /// 根据userId获取员工信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public StaffDto GetByTenantIdAndUserId(int tenantId, int userId)
        {
            var args = new StaffsGetByIdsArgs
            {
                TenantId = tenantId,
                UserIds = new int[] { userId }
            };
            var options = QueryOptions.Default;
            options.IgnoreOperator = true;
            try
            {
                var provider = Beisen.UserFramework.Service.StaffService.Instance;
                //   var provider = PlatformServiceFactory<UserFramework.ESB.ServiceInterface.IStaffProvider>.Instance();
                if (provider == null)
                {
                    AppConnectLogHelper.Error(new ESBServiceNotFoundException("UserFramework ESB Service Is null"));
                    return null;
                }
                var result = provider.GetStaffByIds(args, options);
                if (result.Code != 200)
                {
                    throw new UserOperateException(string.Format("Code:{0},Message:{1}", result.Code, result.Message));
                }
                return result.Total > 0 ? result.Items[0] : null;
            }
            catch (System.Exception ex)
            {
                AppConnectLogHelper.Error(ex.Message, new UserOperateException(string.Format("根据租户id和userid获取用户信息失败,接口：StaffService.Instance.GetStaffByIds,参数：tenantId {0}，userId {1}",
                    tenantId, userId), ex));
                return null;
            }
        }

        /// <summary>
        /// 根据userName获取员工Id
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Dictionary<int, int> GetByUserName(string userName)
        {
            var users = new Dictionary<int, int>();
            var args = new AssociatedUsersGetByLoginNameArgs
            {
                LoginName = userName
            };
            try
            {
                var provider = AccountService.Instance;
                //var provider = PlatformServiceFactory<IAccountProvider>.Instance();
                if (provider == null)
                {
                    AppConnectLogHelper.Error(new ESBServiceNotFoundException("UserFramework ESB Service Is null"));
                    return null;
                }

                var result = provider.GetAssociatedUsersByLoginName(args);
                if (!result.Ok)
                {
                    throw new UserOperateException(string.Format("Message:{0}", result.Error.Message));
                }

                if (result.State != null && result.State.Count() > 0)
                {
                    result.State.ToList().ForEach(item => users.Add(item.UserId, item.TenantId));
                }

                return users;
            }
            catch (System.Exception ex)
            {
                AppConnectLogHelper.Error(ex.Message, new UserOperateException(string.Format("根据UserName获取用户信息失败,接口：AccountService.Instance.GetAssociatedUsersByLoginName,参数：userName{0}"
                    , userName), ex));
                return null;
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Authentication Login(string userName, string password, out ResultModel res)
        {

            res = new ResultModel();
            var args = new LoginArgs
            {
                LoginName = userName,
                Password = password,

            };
            //var args = new AuthenticateArgs
            //{
            //    LoginName = userName,
            //    Password = password,

            //};
            var options = LoginOptions.Default;
            try
            {
                var result = AccountService.Instance.Login(args, options);
                //  var result = AccountService.Instance.Authenticate(args);
                if (!result.Ok)
                {
                    throw result.Error;
                }
                res.ErrCode = 1;
                return result.State[0];
            }
            catch (LoginNameNotExistException ex)
            {
                res.ErrCode = -1;
                res.ErrMsg = "账户或密码错误";
                return null;
            }
            catch (PasswordInvalidException ex)
            {
                res.ErrCode = -1;
                res.ErrMsg = "账户或密码错误";
                if (ex.MaxFailedTimes != -1)
                {
                    res.ErrMsg = "账户或密码错误，{ex.MaxFailedTimes}次输入错误后账户将被锁定";
                }
                return null;
            }
            catch (StaffDimissionException ex)
            {

                AppConnectLogHelper.Error(ex.Message, new UserOperateException("用户已离职", ex));
                res.ErrCode = -2;
                res.ErrMsg = ex.Message;
                return null;
            }
            catch (TenantDisabledException ex)
            {
                AppConnectLogHelper.Error(ex.Message, new UserOperateException("租户已停用", ex));
                res.ErrCode = -3;
                res.ErrMsg = ex.Message;
                return null;
            }
            catch (PasswordExpiredException ex)
            {
                AppConnectLogHelper.Error(ex.Message, new UserOperateException("密码已过期", ex));
                res.ErrCode = -9;
                res.ErrMsg = ex.Message;
                return null;
            }
            catch (PasswordStrengthException ex)
            {
                AppConnectLogHelper.Error(ex.Message, new UserOperateException("密码强度不符合规范", ex));
                res.ErrCode = -10;
                res.ErrMsg = ex.Message;
                return null;
            }
            catch (LoginIntervalOverlongException ex)
            {
                AppConnectLogHelper.Error(ex.Message, new UserOperateException("账户已停用", ex));
                res.ErrCode = -14;
                res.ErrMsg = ex.Message;
                return null;
            }
            catch (UserDisabledException ex)
            {
                AppConnectLogHelper.Error(ex.Message, new UserOperateException("账户已停用", ex));
                res.ErrCode = -14;
                res.ErrMsg = ex.Message;
                return null;
            }
            catch (AccountLockedException ex)
            {
                AppConnectLogHelper.Error(ex.Message, new UserOperateException("账户已锁定", ex));
                res.ErrCode = -15;
                //账号已被锁定，请点击忘记密码或30分钟后重试
                //账号已被锁定，请30分钟后重试
                res.ErrMsg = ex.Message.Replace("点击忘记密码或", "");
                return null;
            }
            catch (NoTenantAvailableException ex)
            {
                AppConnectLogHelper.Error(ex.Message, new UserOperateException("登录失败", ex));
                res.ErrCode = -1;
                res.ErrMsg = ex.Message;
                return null;
            }
            catch (System.Exception ex)
            {
                AppConnectLogHelper.Error(ex.Message, new UserOperateException("登录失败,接口： AccountService.Instance.Login,参数：userName {userName}", ex));
                res.ErrCode = 0;
                res.ErrMsg = "登录失败";
                return null;
            }
        }
    }
}

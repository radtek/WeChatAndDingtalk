using System.Collections.Generic;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface IAppUserAccountProvider
    {
        /// <summary>
        /// 增加用户账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="appUserAccountInfo"></param>
        int AddOrUpdate(int tenantId, AppUserAccountInfo appUserAccountInfo);

        /// <summary>
        /// 根据Id获取用户账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        AppUserAccountInfo GetById(int tenantId, int id);

        /// <summary>
        /// 根据OpenId获取用户账户
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        AppUserAccountInfo GetByOpenId(string appId, string openId);

        /// <summary>
        /// 根据UserId获取用户账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        AppUserAccountInfo GetByUserId(int tenantId, int userId, string appId);

        /// <summary>
        /// 根据TenantId和状态获取用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="appId"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        List<AppUserAccountInfo> GetByTenantId(int tenantId, string appId, string states);

        /// <summary>
        /// 更新用户账户状态
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <param name="state"></param>
        void UpdateState(int tenantId, int id, AppUserAccountState state);

        /// <summary>
        /// 解绑账户
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        void UnBind(string appId, string openId);

        /// <summary>
        /// 解绑账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="ids"></param>
        void UnBind(int tenantId, int userId, string ids);

        /// <summary>
        /// 获取用户邮箱
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        string GetUserEmail(string appId, string openId);

        /// <summary>
        /// 激活绑定
        /// </summary>
        /// <param name="code"></param>
        bool Activate(string code);

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
        BindResult Bind(Dictionary<string, string> openIds, string batch, AppUserAccountType type, string userName, string password, string captcha);

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        byte[] GetCaptchaImage(string batch);

        /// <summary>
        /// 获取绑定中的状态信息
        /// </summary>
        /// <param name="openIds"></param>
        /// <param name="batch"></param>
        /// <returns></returns>
        BindingInfoResult GetBindingInfo(Dictionary<string, string> openIds, string batch);

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        /// <param name="titaAppId"></param>
        /// <returns></returns>
        string GetSginQuery(string appId, string openId, int titaAppId);
        /// <summary>
        /// 获取签名V2 调用Tita Api
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        /// <param name="titaAppId"></param>
        /// <returns></returns>
        string GetSginQueryV2(string appId, string openId, int titaAppId);

        /// <summary>
        /// 更新主账户ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="masterAccountId"></param>
        void UpdateMasterAccountId(int id, int masterAccountId);

        List<AppUserForCloud> GetCloudList(int tenantId, string appAccountId, int pageSize, int pageNum, out int total);

        /// <summary>
        /// 获取用户状态描述
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        string GetStateName(AppUserAccountState state);

        List<AppUserAccountInfo> GetListByUserId(int tenantId, string[] userIds);

        List<AppUserAccountInfo> GetListByUserId(int tenantId, string[] userId,string appId);

        List<AppUserAccountInfo> ConvertToUserId(string appAccountId, List<string> openIds);
        /// <summary>
        /// 删除绑定的数据
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool DeleteAppUser(int tenantId, int userId);
    }
}

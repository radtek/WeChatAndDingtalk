using System.Collections.Generic;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using AppAccountInfo = Beisen.AppConnect.ServiceInterface.Model.AppAccountInfo;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    /// <summary>
    /// 开放平台帐号信息接口
    /// </summary>
    public interface IAppAccountProvider
    {

        /// <summary>
        /// 增加开放平台帐号信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="info"></param>
        /// <param name="isSaveMultiTenant"></param>
        string Add(int tenantId, AppAccountInfo info, bool isSaveMultiTenant = true);

        /// <summary>
        /// 更新开放平台帐号信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="info"></param>
        /// <param name="isSaveMultiTenant"></param>
        void Update(int tenantId, AppAccountInfo info, bool isSaveMultiTenant = true);

        /// <summary>
        /// 增加或更新开放平台账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="info"></param>
        /// <param name="isSaveMultiTenant"></param>
        string AddOrUpdate(int tenantId, AppAccountInfo info, bool isSaveMultiTenant = true);

        /// <summary>
        /// 获取开放平台帐号信息
        /// </summary>
        /// <param name="appAccountId"></param>
        /// <returns></returns>
        AppAccountInfo Get(string appAccountId);

        /// <summary>
        /// 获取开放平台帐号信息（从多租赁）
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="appAccountId"></param>
        /// <returns></returns>
        AppAccountInfo GetMultiTenant(int tenantId, string appAccountId);

        /// <summary>
        /// 更新开放平台帐号信息（多租赁）
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="appAccountId"></param>
        /// <returns></returns>
        void UpdateMultiTenant(int tenantId, string appAccountId);

        /// <summary>
        /// 根据类型获取公共开放平台帐号
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        AppAccountInfo GetPublicByType(int type);

        /// <summary>
        /// 更新开放平台账户状态
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="appAccountId"></param>
        /// <param name="state"></param>
        void UpdateState(int tenantId, int userId, string appAccountId, AppAccountState state);

        /// <summary>
        /// 获取账户类型
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> GetAppAccountTypes();

        AppAccountInfo GetByTag(int tenantId, string tag);

        string GetAppAccountId(int tenantId, string appAccountId, string tag);

        /// <summary>
        /// 获取账户类型名称
        /// </summary>
        /// <param name="appAccountId"></param>
        /// <returns></returns>
        string GetAppAccountTypeName(string appAccountId);

        List<AppAccountInfo> GetListByAppId(int tenantId, List<string> appIds);

        List<AppAccountInfo> GetListByTenantId(int tenantId);
    }
}

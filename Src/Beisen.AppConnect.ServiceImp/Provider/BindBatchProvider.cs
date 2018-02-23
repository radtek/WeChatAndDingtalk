using Beisen.AppConnect.ServiceImp.Persistance;
using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.Common.HelperObjects;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    public class BindBatchProvider: IBindBatchProvider
    {
        #region 单例

        private static readonly IBindBatchProvider _instance = new BindBatchProvider();
        public static IBindBatchProvider Instance
        {
            get { return _instance; }
        }

        private BindBatchProvider()
        {
        }
        #endregion

        /// <summary>
        /// 增加绑定批次
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Add(int tenantId, BindBatchInfo info)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertNotNull(info, "info is null");
            ArgumentHelper.AssertIsTrue(info.TenantId>0, "info.TenantId is 0");
            ArgumentHelper.AssertIsTrue(info.AppUserAccountId > 0, "info.AppUserAccountId is 0");
            ArgumentHelper.AssertNotNullOrEmpty(info.BeisenAccount, "info.BeisenAccount is null");

            return BindBatchDao.Insert(tenantId, info);
        }

        /// <summary>
        /// 获取绑定批次
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public BindBatchInfo Get(int tenantId, int id)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertIsTrue(id > 0, "id is 0");

            return BindBatchDao.Get(tenantId, id);
        }

        /// <summary>
        /// 更新批次状态
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <param name="state"></param>
        public void UpdateState(int tenantId, int id, BindBatchState state)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertIsTrue(id > 0, "id is 0");

            BindBatchDao.UpdateState(tenantId, id, state);
        }

        /// <summary>
        /// 更新邮件、短信发送批次
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <param name="batchId"></param>
        public void UpdateBatchId(int tenantId, int id, string batchId)
        {
            ArgumentHelper.AssertIsTrue(tenantId > 0, "tenantId is 0");
            ArgumentHelper.AssertIsTrue(id > 0, "id is 0");
            ArgumentHelper.AssertNotNullOrEmpty(batchId, "batchId is null");

            BindBatchDao.UpdateBatchId(tenantId, id, batchId);
        }
    }
}

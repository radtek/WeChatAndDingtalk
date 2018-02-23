using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface IBindBatchProvider
    {
        /// <summary>
        /// 增加绑定批次
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        int Add(int tenantId, BindBatchInfo info);

        /// <summary>
        /// 获取绑定批次
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        BindBatchInfo Get(int tenantId, int id);

        /// <summary>
        /// 更新批次状态
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <param name="state"></param>
        void UpdateState(int tenantId, int id, BindBatchState state);

        /// <summary>
        /// 更新邮件、短信发送批次
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <param name="batchId"></param>
        void UpdateBatchId(int tenantId, int id, string batchId);
    }
}

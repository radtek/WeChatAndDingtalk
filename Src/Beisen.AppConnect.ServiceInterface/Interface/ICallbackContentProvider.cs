using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface ICallbackContentProvider
    {
        /// <summary>
        /// 增加返回信息
        /// </summary>
        /// <param name="content"></param>
        void Add(CallbackContentInfo content);

        /// <summary>
        /// 根据batchId获取回调信息
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        CallbackContentInfo GetByBatchId(string batchId);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="state"></param>
        void UpdateState(string batchId, CallbackContentState state);
    }
}

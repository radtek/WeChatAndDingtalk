using Beisen.AppConnect.ServiceImp.Persistance;
using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.Common.HelperObjects;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    public class CallbackContentProvider : ICallbackContentProvider
    {
        #region 单例

        private static readonly ICallbackContentProvider _instance = new CallbackContentProvider();
        public static ICallbackContentProvider Instance
        {
            get { return _instance; }
        }

        private CallbackContentProvider()
        {
        }

        #endregion

        /// <summary>
        /// 增加返回信息
        /// </summary>
        /// <param name="content"></param>
        public void Add(CallbackContentInfo content)
        {
            ArgumentHelper.AssertIsTrue(content != null, "content is null");
            ArgumentHelper.AssertIsTrue(content.TenantId >= 0, "AppAccountInfo.TenantId is less than 0");
            ArgumentHelper.AssertNotNullOrEmpty(content.BatchId, "content.BatchId is null or empty");
            ArgumentHelper.AssertNotNullOrEmpty(content.AppAccountPublic, "content.AppAccountPublic is null or empty");
            ArgumentHelper.AssertNotNullOrEmpty(content.Content, "content.Content is null or empty");

            CallbackContentDao.Insert(content);
        }

        /// <summary>
        /// 根据batchId获取回调信息
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public CallbackContentInfo GetByBatchId(string batchId)
        {
            ArgumentHelper.AssertNotNullOrEmpty(batchId, "batchId is null or empty");

            return CallbackContentDao.GetByBatchId(batchId);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="state"></param>
        public void UpdateState(string batchId, CallbackContentState state)
        {
            ArgumentHelper.AssertNotNullOrEmpty(batchId, "batchId is null or empty");

            CallbackContentDao.UpdateState(batchId, state);
        }
    }
}

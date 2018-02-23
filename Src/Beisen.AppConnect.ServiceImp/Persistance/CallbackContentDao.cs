using System;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.Data;

namespace Beisen.AppConnect.ServiceImp.Persistance
{
    public static class CallbackContentDao
    {
        /// <summary>
        /// 增加返回信息
        /// </summary>
        /// <param name="content"></param>
        public static void Insert(CallbackContentInfo content)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            SafeProcedure.ExecuteNonQuery(db, "dbo.CallbackContent_Insert", parameterMapper =>
            {
                parameterMapper.AddWithValue("@BatchId", new Guid(content.BatchId));
                parameterMapper.AddWithValue("@TenantId", content.TenantId);
                parameterMapper.AddWithValue("@AppAccountPublic", content.AppAccountPublic);
                parameterMapper.AddWithValue("@AppAccountPrivate", content.AppAccountPrivate);
                parameterMapper.AddWithValue("@Content", content.Content);
                parameterMapper.AddWithValue("@State", (short)content.State);
            });
        }

        /// <summary>
        /// 根据batchId获取回调信息
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public static CallbackContentInfo GetByBatchId(string batchId)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            return SafeProcedure.ExecuteAndGetInstance<CallbackContentInfo>(db, "dbo.CallbackContent_GetByBatchId", parameterMapper =>
            {
                parameterMapper.AddWithValue("@BatchId", new Guid(batchId));
            },
            BuildInfo);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="state"></param>
        public static void UpdateState(string batchId, CallbackContentState state)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            SafeProcedure.ExecuteNonQuery(db, "dbo.CallbackContent_UpdateState", parameterMapper =>
            {
                parameterMapper.AddWithValue("@BatchId", new Guid(batchId));
                parameterMapper.AddWithValue("@State", (short)state);
            });
        }

        /// <summary>
        /// 数据映射
        /// </summary>
        /// <param name="record">记录</param>
        /// <param name="info">实体</param>
        private static void BuildInfo(IRecord record, CallbackContentInfo info)
        {
            info.Id = record.Get<int>("Id");
            info.BatchId = record.Get<Guid>("BatchId").ToString();
            info.TenantId = record.Get<int>("TenantId");
            info.AppAccountPublic = record.Get<string>("AppAccountPublic");
            info.AppAccountPrivate = record.Get<string>("AppAccountPrivate");
            info.Content = record.Get<string>("Content");
            info.State = (CallbackContentState)record.Get<short>("State");
        }
    }
}

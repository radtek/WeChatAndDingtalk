using System.Data;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.Data;

namespace Beisen.AppConnect.ServiceImp.Persistance
{
    internal class BindBatchDao
    {
        /// <summary>
        /// 增加绑定批次
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        internal static int Insert(int tenantId, BindBatchInfo info)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            int id = 0;
            SafeProcedure.ExecuteNonQuery(db, "dbo.BindBatch_Insert", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", info.TenantId);
                parameterMapper.AddWithValue("@AppUserAccountId", info.AppUserAccountId);
                parameterMapper.AddWithValue("@BeisenAccount", info.BeisenAccount);
                parameterMapper.AddWithValue("@Batch", info.Batch);
                parameterMapper.AddWithValue("@Type", (short)info.Type);
                parameterMapper.AddTypedDbNull("@Id", ParameterDirectionWrap.Output, DbType.Int32);
            }, o =>
            {
                id = (int)o.GetValue("@Id");
            });
            return id;
        }

        /// <summary>
        /// 获取绑定批次
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static BindBatchInfo Get(int tenantId, int id)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            return SafeProcedure.ExecuteAndGetInstance<BindBatchInfo>(db, "dbo.BindBatch_Get", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@Id", id);
            },
            BuildInfo);
        }

        /// <summary>
        /// 更新批次状态
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <param name="state"></param>
        internal static void UpdateState(int tenantId, int id, BindBatchState state)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            SafeProcedure.ExecuteNonQuery(db, "dbo.BindBatch_UpdateState", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@Id", id);
                parameterMapper.AddWithValue("@State", (short)state);
            });
        }

        /// <summary>
        /// 更新邮件、短信发送批次
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <param name="batchId"></param>
        internal static void UpdateBatchId(int tenantId, int id, string batchId)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            SafeProcedure.ExecuteNonQuery(db, "dbo.BindBatch_UpdateBatchId", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@Id", id);
                parameterMapper.AddWithValue("@BatchId", batchId);
            });
        }

        /// <summary>
        /// 数据映射
        /// </summary>
        /// <param name="record">记录</param>
        /// <param name="info">实体</param>
        private static void BuildInfo(IRecord record, BindBatchInfo info)
        {
            info.Id = record.Get<int>("Id");
            info.TenantId = record.Get<int>("TenantId");
            info.AppUserAccountId = record.Get<int>("AppUserAccountId");
            info.BeisenAccount = record.Get<string>("BeisenAccount");
            info.Batch = record.Get<string>("Batch");
            info.Type = (AppUserAccountType)record.Get<short>("Type");
            info.State = (BindBatchState)record.Get<short>("State");
        }
    }
}

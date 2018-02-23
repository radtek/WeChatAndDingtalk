using System;
using System.Data;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.Data;

namespace Beisen.AppConnect.ServiceImp.Persistance
{
    internal static class MessageDao
    {
        internal static int Insert(MessageInfo info)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            int id = 0;
            SafeProcedure.ExecuteNonQuery(db, "dbo.Message_Insert", parameterMapper =>
            {
                parameterMapper.AddWithValue("@BatchId", info.BatchId);
                parameterMapper.AddWithValue("@TenantId", info.TenantId);
                parameterMapper.AddWithValue("@FromUser", info.FromUser);
                parameterMapper.AddWithValue("@ToUser", info.ToUser);
                parameterMapper.AddWithValue("@ToOpenId", info.ToOpenId);
                parameterMapper.AddWithValue("@AppAccountId", info.AppAccountId);
                parameterMapper.AddWithValue("@TemplateIdShort", info.TemplateIdShort);
                parameterMapper.AddWithValue("@TemplateId", info.TemplateId);
                parameterMapper.AddWithValue("@Content", info.ContentJson);
                parameterMapper.AddWithValue("@State", (short)info.State);
                parameterMapper.AddWithValue("@MessageId", info.MessageId);
                parameterMapper.AddWithValue("@Result", info.Result);
                parameterMapper.AddTypedDbNull("@Id", ParameterDirectionWrap.Output, DbType.Int32);
            }, o =>
            {
                id = (int)o.GetValue("@Id");
            });
            return id;
        }

        internal static void UpdateSendResult(int tenantId, int id, string messageId, MessageState state, string result)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            SafeProcedure.ExecuteNonQuery(db, "dbo.Message_UpdateSendResult", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@Id", id);
                parameterMapper.AddWithValue("@MessageId", messageId);
                parameterMapper.AddWithValue("@State", (short)state);
                parameterMapper.AddWithValue("@Result", result);
            });
        }

        internal static void UpdateSendResultForWeChatService(string appAccountId, int userId, string messageId, MessageState state, string result)
        {
            throw new NotImplementedException();
        }

        internal static MessageInfo Get(int tenantId,int id)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            return SafeProcedure.ExecuteAndGetInstance<MessageInfo>(db, "dbo.Message_Get", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@Id", id);

            }, BuildInfo);
        }

        /// <summary>
        /// 数据映射
        /// </summary>
        /// <param name="record">记录</param>
        /// <param name="info">实体</param>
        private static void BuildInfo(IRecord record, MessageInfo info)
        {
            info.Id = record.Get<int>("Id");
            info.BatchId = record.Get<string>("BatchId");
            info.TenantId = record.Get<int>("TenantId");
            info.FromUser = record.Get<int>("FromUser");
            info.ToUser = record.Get<string>("ToUser");
            info.AppAccountId = record.Get<string>("AppAccountId");
            info.TemplateIdShort = record.Get<string>("TemplateIdShort");
            info.TemplateId = record.Get<string>("TemplateId");
            info.ContentJson = record.Get<string>("Content");
            info.Content = SerializeHelper.Deserialize<MessageContent>(info.ContentJson);
            info.State = (MessageState)record.Get<short>("State");
            info.MessageId = record.Get<string>("MessageId");
            info.Result = record.Get<string>("Result");
        }
    }
}

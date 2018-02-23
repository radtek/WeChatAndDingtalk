using System.Data;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.Data;

namespace Beisen.AppConnect.ServiceImp.Persistance
{
    internal static class TemplateMappingDao
    {
        internal static int Insert(TemplateMappingInfo info)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            int id = 0;
            SafeProcedure.ExecuteNonQuery(db, "dbo.TemplateMapping_Insert", parameterMapper =>
            {
                parameterMapper.AddWithValue("@AppId", info.AppId);
                parameterMapper.AddWithValue("@TemplateIdShort", info.TemplateIdShort);
                parameterMapper.AddWithValue("@TemplateId", info.TemplateId);
                parameterMapper.AddWithValue("@UserId", info.CreateBy);
                parameterMapper.AddTypedDbNull("@Id", ParameterDirectionWrap.Output, DbType.Int32);
            }, o =>
            {
                id = (int)o.GetValue("@Id");
            });
            return id;
        }

        internal static TemplateMappingInfo Get(string appId,string templateIdShort)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            return SafeProcedure.ExecuteAndGetInstance<TemplateMappingInfo>(db, "dbo.TemplateMapping_Get", parameterMapper =>
            {
                parameterMapper.AddWithValue("@AppId", appId);
                parameterMapper.AddWithValue("@TemplateIdShort", templateIdShort);

            }, BuildInfo);
        }

        internal static void Delete(int userId,int id)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            SafeProcedure.ExecuteNonQuery(db, "dbo.TemplateMapping_Delete", parameterMapper =>
            {
                parameterMapper.AddWithValue("@Id", id);
                parameterMapper.AddWithValue("@UserId", userId);
            });
        }

        /// <summary>
        /// 数据映射
        /// </summary>
        /// <param name="record">记录</param>
        /// <param name="info">实体</param>
        private static void BuildInfo(IRecord record, TemplateMappingInfo info)
        {
            info.Id = record.Get<int>("Id");
            info.AppId = record.Get<string>("AppId");
            info.TemplateIdShort = record.Get<string>("TemplateIdShort");
            info.TemplateId = record.Get<string>("TemplateId");
        }
    }
}

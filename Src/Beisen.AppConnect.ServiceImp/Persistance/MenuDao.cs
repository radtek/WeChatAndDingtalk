using System.Collections.Generic;
using System.Data;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.Data;

namespace Beisen.AppConnect.ServiceImp.Persistance
{
    internal static class MenuDao
    {
        internal static int Insert(MenuInfo info)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            int id = 0;
            SafeProcedure.ExecuteNonQuery(db, "dbo.Menu_Insert", parameterMapper =>
            {
                parameterMapper.AddWithValue("@MenuId", info.MenuId);
                parameterMapper.AddWithValue("@Name", info.Name);
                parameterMapper.AddWithValue("@TenantId", info.TenantId);
                parameterMapper.AddWithValue("@AppAccountId", info.AppAccountId);
                parameterMapper.AddWithValue("@BeisenAppId", info.BeisenAppId);
                parameterMapper.AddWithValue("@Url", info.Url);
                parameterMapper.AddWithValue("@UserId", info.CreateBy);
                parameterMapper.AddTypedDbNull("@Id", ParameterDirectionWrap.Output, DbType.Int32);
            }, o =>
            {
                id = (int) o.GetValue("@Id");
            });
            return id;
        }

        internal static void Update(MenuInfo info)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            SafeProcedure.ExecuteNonQuery(db, "dbo.Menu_Update", parameterMapper =>
            {
                parameterMapper.AddWithValue("@MenuId", info.MenuId);
                parameterMapper.AddWithValue("@Name", info.Name);
                parameterMapper.AddWithValue("@TenantId", info.TenantId);
                parameterMapper.AddWithValue("@AppAccountId", info.AppAccountId);
                parameterMapper.AddWithValue("@BeisenAppId", info.BeisenAppId);
                parameterMapper.AddWithValue("@Url", info.Url);
                parameterMapper.AddWithValue("@UserId", info.ModifyBy);
            });
        }

        internal static List<MenuInfo> GetList(int tenantId)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            var list = SafeProcedure.ExecuteAndGetInstanceList<MenuInfo>(db, "dbo.Menu_GetList", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);

            }, BuildInfo);

            return list;
        }

        internal static MenuInfo Get(string menuId)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            return SafeProcedure.ExecuteAndGetInstance<MenuInfo>(db, "dbo.Menu_Get", parameterMapper =>
            {
                parameterMapper.AddWithValue("@MenuId", menuId);

            }, BuildInfo);
        }

        /// <summary>
        /// 数据映射
        /// </summary>
        /// <param name="record">记录</param>
        /// <param name="info">实体</param>
        private static void BuildInfo(IRecord record, MenuInfo info)
        {
            info.Id = record.Get<int>("Id");
            info.MenuId = record.Get<string>("MenuId");
            info.Name= record.Get<string>("Name");
            info.TenantId = record.Get<int>("TenantId");
            info.AppAccountId = record.Get<string>("AppAccountId");
            info.BeisenAppId = record.Get<int>("BeisenAppId");
            info.Url = record.Get<string>("Url");
        }
    }
}

using Beisen.AppConnectISV.Model.BusinessModel;
using Beisen.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Infrastructure
{
    public class HttpRequestDao
    {
        public static void Insert(string url, string method, string body, string response, string message)
        {
            var db = Database.GetDatabase(SqlConst.AppConnect);
            SafeProcedure.ExecuteNonQuery(db, "dbo.RequestLog_Insert", parameterMapper =>
            {
                parameterMapper.AddWithValue("@Url", url);
                parameterMapper.AddWithValue("@Method", method);
                parameterMapper.AddWithValue("@Body", body);
                parameterMapper.AddWithValue("@Response", response);
                parameterMapper.AddWithValue("@Message", message);
            });
        }

        public static List<AppAccount> GetAppIdByAppAccountId(string appAccountId)
        {
            var db = Database.GetDatabase(SqlConst.AppConnect);
            var result = SafeProcedure.ExecuteAndGetInstanceList<AppAccount>(db, "dbo.AppAccount_GetAppIdByAppAccountId", parameterMapper =>
            {
                parameterMapper.AddWithValue("@AppAccountId", appAccountId);
            }, AppIdInfo);

            return result;
        }


        public static List<UserInfoModel> GetListByUserId(int tenantId, string userIds, string appId)
        {
            var db = Database.GetDatabase(SqlConst.UserSecurity);
            var list = SafeProcedure.ExecuteAndGetInstanceList<UserInfoModel>(db, "dbo.AppUserAccount_GetInfoByUserId", parameterMapper =>
            {
                parameterMapper.AddWithValue("@UserIds", userIds);
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@AppId", appId);
            }, BuildInfo);

            return list;
        }


        /// <summary>
        /// 数据映射
        /// </summary>
        /// <param name="record">记录</param>
        /// <param name="info">实体</param>
        private static void BuildInfo(IRecord record, UserInfoModel info)
        {
            info.MappingUserId = record.Get<string>("OpenId");
            info.TenantId = record.Get<int>("TenantId");
            info.StaffId = record.Get<int>("UserId");
            info.BeisenAccount = record.Get<string>("BeisenAccount");
            info.MasterAccountId = record.Get<int>("MasterAccountId");
        }

        /// <summary>
        /// 数据映射
        /// </summary>
        /// <param name="record">记录</param>
        /// <param name="info">实体</param>
        private static void AppIdInfo(IRecord record, AppAccount info)
        {
            info.AppId = record.Get<string>("AppId");
            info.Type = record.Get<Int16>("Type");
        }
    }
}

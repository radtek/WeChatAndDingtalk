using Beisen.AppConnect.Tool.Model;
using Beisen.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Tool.Common
{
    public class SqlHelper
    {
        public static List<AppIdModel> GetAppIdByType()
        {
            var db = Database.GetDatabase("BeisenAppConnect");
            var result = SafeProcedure.ExecuteAndGetInstanceList<AppIdModel>(db, "dbo.AppAccount_GetAppIdByType",parameterMapper=>
            { },AppIdInfo);
            return result;
        }


        public static List<UserInfoModel> GetInfoByAppId(string appIds)
        {
            var db = Database.GetDatabase("BeisenUserSecurity");
            var list = SafeProcedure.ExecuteAndGetInstanceList<UserInfoModel>(db, "dbo.AppUserAccount_GetInfoByAppId", parameterMapper =>
            {
                parameterMapper.AddWithValue("@AppId", appIds);
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
            info.AppId = record.Get<string>("AppId");
            info.OpenId = record.Get<string>("OpenId");
            info.TenantId = record.Get<int>("TenantId");
            info.UserId = record.Get<int>("UserId");
            info.State = record.Get<short>("State");
            info.Type = record.Get<short>("Type");
        }

        /// <summary>
        /// 数据映射
        /// </summary>
        /// <param name="record">记录</param>
        /// <param name="info">实体</param>
        private static void AppIdInfo(IRecord record, AppIdModel info)
        {
            info.AppId = record.Get<string>("AppId");          
        }

    }
}

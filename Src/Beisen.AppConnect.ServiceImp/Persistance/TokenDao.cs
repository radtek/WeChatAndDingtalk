using System;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Cache;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.Data;

namespace Beisen.AppConnect.ServiceImp.Persistance
{
    /// <summary>
    /// 调用凭据数据访问层
    /// </summary>
    internal class TokenDao
    {
        /// <summary>
        /// 获取调用凭证
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static TokenInfo Get(string appId)
        {
            var token = new TokenInfo {AppId = appId};
            token = AppConnectEntityProcedure.GetEntityInstanceFromCacheByExtendedId(token);

            if (token == null || token.IsEmpty)
            {
                var db = Database.GetDatabase(DatabaseName.AppConnect);
                token = SafeProcedure.ExecuteAndGetInstance<TokenInfo>(db, "dbo.Token_Get", parameterMapper =>
                {
                    parameterMapper.AddWithValue("@AppId", appId);
                },
                    BuildInfo);

                if (token != null)
                {
                    token.IsEmpty = false;
                    AppConnectEntityProcedure.SaveEntityInstanceToCache(token);
                }
            }

            return token;
        }

        /// <summary>
        /// 增加或者更新调用凭证
        /// </summary>
        /// <param name="token"></param>
        internal static void InsertOrUpdate(TokenInfo token)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            SafeProcedure.ExecuteNonQuery(db, "dbo.Token_InsertOrUpdate", parameterMapper =>
            {
                parameterMapper.AddWithValue("@AppId", token.AppId);
                parameterMapper.AddWithValue("@Token", token.AccessToken);
                parameterMapper.AddWithValue("@ExpireTime", token.ExpireTime);
            });

            AppConnectEntityProcedure.SaveEntityInstanceToCache(token);
        }

        /// <summary>
        /// 数据映射
        /// </summary>
        /// <param name="record">记录</param>
        /// <param name="info">实体</param>
        private static void BuildInfo(IRecord record, TokenInfo info)
        {
            info.Id = record.Get<int>("Id");
            info.AppId = record.Get<string>("AppId");
            info.AccessToken = record.Get<string>("AccessToken");
            info.ExpireTime = record.Get<DateTime>("ExpireTime");
        }
    }
}

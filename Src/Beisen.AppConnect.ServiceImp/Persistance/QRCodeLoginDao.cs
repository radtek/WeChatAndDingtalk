using System;
using System.Data;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Cache;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.Data;

namespace Beisen.AppConnect.ServiceImp.Persistance
{
    public static class QrCodeLoginDao
    {
        /// <summary>
        /// 增加二维码登录
        /// </summary>
        /// <param name="qrCodeLoginInfo"></param>
        public static int Insert(QrCodeLoginInfo qrCodeLoginInfo)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            int id = 0;
            SafeProcedure.ExecuteNonQuery(db, "dbo.QrCodeLogin_Insert", parameterMapper =>
            {
                parameterMapper.AddWithValue("@Code", new Guid(qrCodeLoginInfo.Code));
                parameterMapper.AddWithValue("@TitaAppId", qrCodeLoginInfo.TitaAppId);
                parameterMapper.AddWithValue("@TenantId", qrCodeLoginInfo.TenantId);
                parameterMapper.AddWithValue("@UserId", qrCodeLoginInfo.UserId);
                parameterMapper.AddWithValue("@State", (short)qrCodeLoginInfo.State);
                parameterMapper.AddTypedDbNull("@Id", ParameterDirectionWrap.Output, DbType.Int32);
            }, o =>
            {
                id = (int)o.GetValue("@Id");
            });

            AppConnectEntityProcedure.SaveEntityInstanceToCache(qrCodeLoginInfo);
            AppConnectLogHelper.DebugFormat("增加二维码数据,返回值:{0}", id);
            return id;
        }

        /// <summary>
        /// 根据Code获取二维码登录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static QrCodeLoginInfo GetByCode(string code)
        {
            var info = new QrCodeLoginInfo {Code = code};
            info = AppConnectEntityProcedure.GetEntityInstanceFromCacheByExtendedId(info);

            if (info == null || info.IsEmpty)
            {
                var db = Database.GetDatabase(DatabaseName.AppConnect);
                info = SafeProcedure.ExecuteAndGetInstance<QrCodeLoginInfo>(db, "dbo.QrCodeLogin_GetByCode", parameterMapper =>
                {
                    parameterMapper.AddWithValue("@Code", new Guid(code));
                },
                BuildInfo);

                if (info != null)
                {
                    info.IsEmpty = false;
                    AppConnectEntityProcedure.SaveEntityInstanceToCache(info);
                }
            }
            return info;
        }

        /// <summary>
        /// 更新二维码登录状态
        /// </summary>
        /// <param name="info"></param>
        public static void UpdateState(QrCodeLoginInfo info)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            SafeProcedure.ExecuteNonQuery(db, "dbo.QrCodeLogin_UpdateState", parameterMapper =>
            {
                parameterMapper.AddWithValue("@Code", new Guid(info.Code));
                parameterMapper.AddWithValue("@State", (short)info.State);
            });
            AppConnectEntityProcedure.SaveEntityInstanceToCache(info);
        }

        /// <summary>
        /// 更新二维码用户信息
        /// </summary>
        /// <param name="info"></param>
        public static void UpdateIdentity(QrCodeLoginInfo info)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            SafeProcedure.ExecuteNonQuery(db, "dbo.QrCodeLogin_UpdateIdentity", parameterMapper =>
            {
                parameterMapper.AddWithValue("@Code", new Guid(info.Code));
                parameterMapper.AddWithValue("@State", (short)info.State);
                parameterMapper.AddWithValue("@TenantId", info.TenantId);
                parameterMapper.AddWithValue("@UserId", info.UserId);
            });
            AppConnectEntityProcedure.SaveEntityInstanceToCache(info);
        }

        /// <summary>
        /// 数据映射
        /// </summary>
        /// <param name="record">记录</param>
        /// <param name="info">实体</param>
        private static void BuildInfo(IRecord record, QrCodeLoginInfo info)
        {
            info.Id = record.Get<int>("Id");
            info.Code = record.Get<Guid>("Code").ToString();
            info.TitaAppId = record.Get<int>("TitaAppId");
            info.TenantId = record.Get<int>("TenantId");
            info.UserId = record.Get<int>("UserId");
            info.State = (QrCodeLoginState)record.Get<short>("State");
        }
    }
}

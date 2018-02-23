using System;
using System.Collections.Generic;
using System.Data;
using Beisen.AppConnect.Infrastructure;
using Beisen.Data;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.ESB.Client;
using Beisen.MultiTenant.ServiceInterface;
using Beisen.MultiTenant.Model;
using Beisen.DynamicScript.SDK;

namespace Beisen.AppConnect.ServiceImp.Persistance
{
    /// <summary>
    /// 开放平台帐号数据层
    /// </summary>
    internal class AppAccountDao
    {
        private static IDataAccessProvider _DataAccessProvider = ESBProxyV2.GetInstance<IDataAccessProvider>("ESB_MultiTenant");
        private static IMetaObjectProvider _MetaObjectProvider = ESBProxyV2.GetInstance<IMetaObjectProvider>("ESB_MultiTenant");

        private const string MultiTenantMetaObjectName = "AppConnect.AppAccount";

        /// <summary>
        /// 增加开放平台帐号信息
        /// </summary>
        /// <param name="info"></param>
        internal static int Insert(AppAccountInfo info)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            int id = 0;
            SafeProcedure.ExecuteNonQuery(db, "dbo.AppAccount_Insert", parameterMapper =>
            {
                parameterMapper.AddWithValue("@AppAccountId", info.AppAccountId);
                parameterMapper.AddWithValue("@TenantId", info.TenantId);
                parameterMapper.AddWithValue("@Name", info.Name);
                parameterMapper.AddWithValue("@Type", info.Type);
                parameterMapper.AddWithValue("@SourceId", info.SourceId);
                parameterMapper.AddWithValue("@AppId", info.AppId);
                parameterMapper.AddWithValue("@AppSecret", info.AppSecret);
                parameterMapper.AddWithValue("@AgentId", info.AgentId);
                parameterMapper.AddWithValue("@State", (short)info.State);
                parameterMapper.AddWithValue("@Tag", info.Tag);
                parameterMapper.AddWithValue("@UserId", info.CreateBy);
                parameterMapper.AddTypedDbNull("@Id", ParameterDirectionWrap.Output, DbType.Int32);
            }, o =>
            {
                id = (int)o.GetValue("@Id");
            });
            return id;
        }
        internal static void AddOrUpdateCLoud(int tenantId, int userId, AppAccountInfo info)
        {
            var metaObject = CloudDataHelper.GetMetaObject(tenantId, "AppConnect.AppUser");
            List<ObjectData> objectDatas = new List<ObjectData>();
            ObjectData objectdata = new ObjectData(metaObject);
            objectdata.ID = Guid.NewGuid();
            objectdata.CreatedBy = userId;
            objectdata.CreatedTime = DateTime.Now;
            objectdata.ModifiedBy = userId;
            objectdata.ModifiedTime = DateTime.Now;
            objectdata["StdIsDeleted"] = false;
            objectdata["TenantId"] = tenantId;
            objectdata["StaffID"] = info.CreateBy;
            objectdata["UserEmail"] = info.CreateBy;


            objectDatas.Add(objectdata);
            CloudDataHelper.Add(metaObject, objectDatas);

        }

        /// <summary>
        /// 更新开放平台帐号信息
        /// </summary>
        /// <param name="info"></param>
        internal static void Update(AppAccountInfo info)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            SafeProcedure.ExecuteNonQuery(db, "dbo.AppAccount_Update", parameterMapper =>
            {
                parameterMapper.AddWithValue("@AppAccountId", info.AppAccountId);
                parameterMapper.AddWithValue("@TenantId", info.TenantId);
                parameterMapper.AddWithValue("@Name", info.Name);
                parameterMapper.AddWithValue("@Type", info.Type);
                parameterMapper.AddWithValue("@SourceId", info.SourceId);
                parameterMapper.AddWithValue("@AppId", info.AppId);
                parameterMapper.AddWithValue("@AppSecret", info.AppSecret);
                parameterMapper.AddWithValue("@AgentId", info.AgentId);
                parameterMapper.AddWithValue("@State", (short)info.State);
                parameterMapper.AddWithValue("@Tag", info.Tag);
                parameterMapper.AddWithValue("@UserId", info.ModifyBy);
            });
        }

        /// <summary>
        /// 获取开放平台帐号信息
        /// </summary>
        /// <param name="appAccountId"></param>
        /// <returns></returns>
        internal static AppAccountInfo Get(string appAccountId)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            return SafeProcedure.ExecuteAndGetInstance<AppAccountInfo>(db, "dbo.AppAccount_Get", parameterMapper =>
            {
                parameterMapper.AddWithValue("@AppAccountId", appAccountId);
            },
            BuildInfo);
        }

        /// <summary>
        /// 根据类型获取公共开放平台帐号
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static AppAccountInfo GetPubilicByType(int type)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            return SafeProcedure.ExecuteAndGetInstance<AppAccountInfo>(db, "dbo.AppAccount_GetPubilicByType", parameterMapper =>
            {
                parameterMapper.AddWithValue("@Type", type);
            },
            BuildInfo);
        }

        internal static AppAccountInfo GetByTag(int tenantId, string tag)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            return SafeProcedure.ExecuteAndGetInstance<AppAccountInfo>(db, "dbo.AppAccount_GetByTag", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@Tag", tag);
            },
            BuildInfo);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="appAccountId"></param>
        /// <param name="state"></param>
        internal static void UpdateState(int tenantId, int userId, string appAccountId, AppAccountState state)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            SafeProcedure.ExecuteNonQuery(db, "dbo.AppAccount_UpdateState", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@UserId", userId);
                parameterMapper.AddWithValue("@AppAccountId", appAccountId);
                parameterMapper.AddWithValue("@State", (short)state);
            });
        }

        internal static List<AppAccountInfo> GetListByAppId(int tenantId, string appIds)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            var list = SafeProcedure.ExecuteAndGetInstanceList<AppAccountInfo>(db, "dbo.AppAccount_GetListByAppId", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@AppIds", appIds);

            }, BuildInfo);

            return list;
        }

        internal static List<AppAccountInfo> GetListByTenantId(int tenantId)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            var list = SafeProcedure.ExecuteAndGetInstanceList<AppAccountInfo>(db, "dbo.AppAccount_GetListByTenantId", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);

            }, BuildInfo);

            return list;
        }

        /// <summary>
        /// 数据映射
        /// </summary>
        /// <param name="record">记录</param>
        /// <param name="info">实体</param>
        private static void BuildInfo(IRecord record, AppAccountInfo info)
        {
            info.Id = record.Get<int>("Id");
            info.AppAccountId = record.Get<string>("AppAccountId");
            info.TenantId = record.Get<int>("TenantId");
            info.Name = record.Get<string>("Name");
            info.Type = record.Get<short>("Type");
            info.SourceId = record.Get<string>("SourceId");
            info.AppId = record.Get<string>("AppId");
            info.AppSecret = record.Get<string>("AppSecret");
            info.AgentId = record.Get<string>("AgentId");
            info.State = (AppAccountState)record.Get<short>("State");
            info.Tag = record.Get<string>("Tag");
        }

        internal static void SaveMultiTenant(AppAccountInfo info)
        {
            var metaObject = _MetaObjectProvider.GetObjectMeta(MultiTenantMetaObjectName, info.TenantId);
            var objectData = new ObjectData(metaObject);
            objectData.ID = new Guid(info.AppAccountId);
            objectData.TenantID = info.TenantId;
            objectData["Name"] = info.Name;
            objectData["Type"] = info.Type;
            objectData["SourceId"] = info.SourceId;
            objectData["AppId"] = info.AppId;
            objectData["AppSecret"] = info.AppSecret;
            objectData["State"] = (int)info.State;
            objectData["AgentId"] = info.AgentId;
            objectData["Tag"] = info.Tag;
            objectData.ModifiedBy = info.ModifyBy;
            objectData.ModifiedTime = DateTime.Now;

            var isExist = _DataAccessProvider.IsExists(MultiTenantMetaObjectName, info.TenantId, info.AppAccountId);

            if (isExist)
            {
                _DataAccessProvider.Update(objectData);
            }
            else
            {
                objectData.CreatedBy = info.CreateBy;
                objectData.CreatedTime = DateTime.Now;
                _DataAccessProvider.Add(objectData);
            }
        }

        internal static AppAccountInfo GetMultiTenant(int tenantId, string appAccountId)
        {
            var objectData = _DataAccessProvider.GetEntity(MultiTenantMetaObjectName, tenantId, appAccountId);

            var appAccountInfo = new AppAccountInfo();
            appAccountInfo.AppAccountId = objectData.ID.ToString();
            appAccountInfo.Name = objectData["Name"] as string;
            appAccountInfo.Type = Convert.ToInt32(objectData["Type"]);
            appAccountInfo.SourceId = objectData["SourceId"] as string;
            appAccountInfo.AppId = objectData["AppId"] as string;
            appAccountInfo.AppSecret = objectData["AppSecret"] as string;
            appAccountInfo.State = (AppAccountState)Convert.ToInt32(objectData["State"]);
            appAccountInfo.AgentId = objectData["AgentId"] as string;
            appAccountInfo.Tag = objectData["Tag"] as string;
            appAccountInfo.CreateBy = objectData.CreatedBy;
            appAccountInfo.ModifyBy = objectData.ModifiedBy;
            appAccountInfo.TenantId = objectData.TenantID;

            return appAccountInfo;
        }
    }
}

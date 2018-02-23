using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.Data;
using Beisen.MultiTenant.Model;
using Beisen.Common.Context;
using Beisen.SearchV3.DSL.Filters;
using Beisen.AppConnect.Infrastructure.Constants;
using Beisen.AppConnect.Infrastructure.Helper;
using Newtonsoft.Json;

namespace Beisen.AppConnect.ServiceImp.Persistance
{
    internal static class AppUserAccountDao
    {
        /// <summary>
        /// 增加用户账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="appUserAccountInfo"></param>
        internal static int InsertOrUpdate(int tenantId, AppUserAccountInfo appUserAccountInfo)
        {
            var db = Database.GetDatabase(DatabaseName.UserSecurity);
            int id = 0;
            SafeProcedure.ExecuteNonQuery(db, "dbo.AppUserAccount_InsertOrUpdate", parameterMapper =>
            {
                parameterMapper.AddWithValue("@AppId", appUserAccountInfo.AppId);
                parameterMapper.AddWithValue("@OpenId", appUserAccountInfo.OpenId);
                parameterMapper.AddWithValue("@TenantId", appUserAccountInfo.TenantId);
                parameterMapper.AddWithValue("@UserId", appUserAccountInfo.UserId);
                parameterMapper.AddWithValue("@BeisenAccount", appUserAccountInfo.BeisenAccount);
                parameterMapper.AddWithValue("@Type", (short)appUserAccountInfo.Type);
                parameterMapper.AddWithValue("@State", (short)appUserAccountInfo.State);
                parameterMapper.AddWithValue("@MasterAccountId", appUserAccountInfo.MasterAccountId);
                parameterMapper.AddTypedDbNull("@Id", ParameterDirectionWrap.Output, DbType.Int32);
            }, o =>
            {
                id = (int)o.GetValue("@Id");
            });
            return id;
        }
        internal static void AddOrUpdateCLoud(int tenantId, int sqlId, AppUserAccountInfo appUserAccountInfo)
        {
            var objectData = IsExistLogin(tenantId, appUserAccountInfo);
            if (objectData != null)
            {
                UpdateCLoud(tenantId, sqlId, objectData, appUserAccountInfo);
            }
            else
            {
                AddCLoud(tenantId, sqlId, appUserAccountInfo);
            }
        }
        private static ObjectData IsExistLogin(int tenantId, AppUserAccountInfo appUserAccountInfo)
        {
            ApplicationContext.Current.ApplicationName = AppUserConstants.MetaName;
            ApplicationContext.Current.TenantId = tenantId;
            ApplicationContext.Current.UserId = appUserAccountInfo.UserId;
            ObjectData objectData = new ObjectData();
            var filter = new BooleanFilter()
                   .Must(new TermFilter(AppUserConstants.AppUser_AppId, appUserAccountInfo.AppId))
                   .Must(new TermFilter(AppUserConstants.AppUser_OpenId, appUserAccountInfo.OpenId));
            // .Must(new TermFilter(AppUserConstants.AppUser_UserId, appUserAccountInfo.UserId));
            var result = CloudDataHelper.GetEntityAllList("AppConnect.AppUser", tenantId, filter).ToList();
            if (result != null && result.Count > 0)
            {
                if (result.Count > 1)
                {
                    AppConnectLogHelper.Error("AppId下面的OpenId个数大于一个!appUserAccountInfo:" + JsonConvert.SerializeObject(appUserAccountInfo));
                    objectData = null;
                }
                else
                {
                    objectData = result.First();
                }
            }
            else
            {
                objectData = null;
            }
            return objectData;
        }

        private static void AddCLoud(int tenantId, int sqlId, AppUserAccountInfo appUserAccountInfo)
        {
            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = tenantId;
            ApplicationContext.Current.UserId = appUserAccountInfo.UserId;

            var metaObject = CloudDataHelper.GetMetaObject(tenantId, "AppConnect.AppUser");
            List<ObjectData> objectDatas = new List<ObjectData>();
            ObjectData objectData = new ObjectData(metaObject);
            objectData.ID = Guid.NewGuid();
            objectData.CreatedBy = appUserAccountInfo.UserId;
            objectData.CreatedTime = DateTime.Now;
            objectData.ModifiedBy = appUserAccountInfo.UserId;
            objectData.ModifiedTime = DateTime.Now;
            objectData["StdIsDeleted"] = false;
            objectData["TenantId"] = tenantId;
            objectData["UserId"] = appUserAccountInfo.UserId;
            objectData["UserEmail"] = appUserAccountInfo.BeisenAccount;
            objectData["State"] = (int)appUserAccountInfo.State;
            objectData["ActivateTime"] = DateTime.Now;
            objectData["OpenId"] = appUserAccountInfo.OpenId;
            objectData["AppId"] = appUserAccountInfo.AppId;
            objectData["SqlId"] = sqlId;
            objectData["LoginType"] = ((int)appUserAccountInfo.Type).ToString();
            objectData["MasterAccountId"] = appUserAccountInfo.MasterAccountId;
            objectDatas.Add(objectData);
            CloudDataHelper.Add(metaObject, objectDatas);

        }
        internal static void UpdateCLoud(int tenantId, int sqlId, ObjectData objectData, AppUserAccountInfo appUserAccountInfo)
        {
            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = tenantId;
            ApplicationContext.Current.UserId = appUserAccountInfo.UserId;
            objectData["AppId"] = appUserAccountInfo.AppId;
            objectData["OpenId"] = appUserAccountInfo.OpenId;
            objectData["UserId"] = appUserAccountInfo.UserId;
            objectData["UserEmail"] = appUserAccountInfo.BeisenAccount;
            objectData["State"] = (int)appUserAccountInfo.State;
            objectData["LoginType"] = ((int)appUserAccountInfo.Type).ToString();
            objectData["ActivateTime"] = DateTime.Now;
            objectData["MasterAccountId"] = appUserAccountInfo.MasterAccountId;
            objectData["SqlId"] = sqlId;
            objectData.ModifiedBy = appUserAccountInfo.UserId;
            objectData.ModifiedTime = DateTime.Now;
            CloudDataHelper.Update(objectData);
        }
        
        /// <summary>
        /// 根据Id获取用户账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static AppUserAccountInfo GetById(int tenantId, int id)
        {
            var db = Database.GetDatabase(DatabaseName.UserSecurity);
            return SafeProcedure.ExecuteAndGetInstance<AppUserAccountInfo>(db, "dbo.AppUserAccount_GetById", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@Id", id);
            },
            BuildInfo);
        }

        /// <summary>
        /// 根据OpenId获取用户
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        internal static AppUserAccountInfo GetByOpenId(string appId, string openId)
        {
            var db = Database.GetDatabase(DatabaseName.UserSecurity);
            return SafeProcedure.ExecuteAndGetInstance<AppUserAccountInfo>(db, "dbo.AppUserAccount_GetByOpenId", parameterMapper =>
            {
                parameterMapper.AddWithValue("@AppId", appId);
                parameterMapper.AddWithValue("@OpenId", openId);
            },
            BuildInfo);
        }

        /// <summary>
        /// 根据UserId获取用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        internal static AppUserAccountInfo GetByUserId(int tenantId, int userId, string appId)
        {
            var db = Database.GetDatabase(DatabaseName.UserSecurity);
            return SafeProcedure.ExecuteAndGetInstance<AppUserAccountInfo>(db, "dbo.AppUserAccount_GetByUserId", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@UserId", userId);
                parameterMapper.AddWithValue("@AppId", appId);
            },
            BuildInfo);
        }

        /// <summary>
        /// 根据TenantId和状态获取用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="appId"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        internal static List<AppUserAccountInfo> GetByTenantId(int tenantId, string appId, string states)
        {
            var db = Database.GetDatabase(DatabaseName.UserSecurity);
            return SafeProcedure.ExecuteAndGetInstanceList<AppUserAccountInfo>(db, "dbo.AppUserAccount_GetByTenantId", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@States", states);
                parameterMapper.AddWithValue("@AppId", appId);
            },
            BuildInfo);
        }

        /// <summary>
        /// 更新用户账户状态
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <param name="state"></param>
        internal static void UpdateState(int tenantId, int id, AppUserAccountState state)
        {
            var db = Database.GetDatabase(DatabaseName.UserSecurity);
            SafeProcedure.ExecuteNonQuery(db, "dbo.AppUserAccount_UpdateState", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@Id", id);
                parameterMapper.AddWithValue("@State", (short)state);
            });
        }

        /// <summary>
        /// 用户解绑
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        internal static void UnBind(string appId, string openId)
        {
            var db = Database.GetDatabase(DatabaseName.UserSecurity);
            SafeProcedure.ExecuteNonQuery(db, "dbo.AppUserAccount_UnBind", parameterMapper =>
            {
                parameterMapper.AddWithValue("@AppId", appId);
                parameterMapper.AddWithValue("@OpenId", openId);
            });
        }
        #region 单个解绑
        //internal static void UnbindCloud(int tenantId, int userId, string appId, string openId)
        //{
        //    ApplicationContext.Current.ApplicationName = "AppConnect";
        //    ApplicationContext.Current.TenantId = tenantId;
        //    ApplicationContext.Current.UserId = userId;
        //    var appUsers = GetAppUserList(tenantId, ids);
        //    if (appUsers != null && appUsers.Count > 0)
        //    {
        //        var metaObject = CloudDataHelper.GetMetaObject(tenantId, "AppConnect.AppUser");
        //        appUsers.ForEach(appUser =>
        //        {
        //            appUser["State"] = (int)AppUserAccountState.Unbind;
        //            appUser.ModifiedBy = userId;
        //            appUser.ModifiedTime = DateTime.Now;
        //        });
        //        CloudDataHelper.Update(metaObject, appUsers);
        //    }
        //}
        //private static ObjectData GetAppUser(int tenantId, string appId, string openId)
        //{
        //    var filter = new BooleanFilter()
        //            .Must(new TermsFilter(AppUserConstants.AppUser_OpenId, openId))
        //            .Must(new TermFilter("AppConnect.AppUser.StdIsDeleted", false));
        //    return null;
        //}
        #endregion
        /// <summary>
        /// 解绑账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="ids"></param>
        internal static void UnBind(int tenantId, int userId, string ids)
        {
            var db = Database.GetDatabase(DatabaseName.UserSecurity);
            SafeProcedure.ExecuteNonQuery(db, "dbo.AppUserAccount_UnBindByIds", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@UserId", userId);
                parameterMapper.AddWithValue("@Ids", ids);
            });
        }

        internal static void UnbindCloud(int tenantId, int userId, string ids)
        {
            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = tenantId;
            ApplicationContext.Current.UserId = userId;
            var appUsers = GetAppUserList(tenantId, ids);
            if (appUsers != null && appUsers.Count > 0)
            {
                var metaObject = CloudDataHelper.GetMetaObject(tenantId, "AppConnect.AppUser");
                appUsers.ForEach(appUser =>
                {
                    appUser["State"] = (int)AppUserAccountState.Unbind;
                    appUser.ModifiedBy = userId;
                    appUser.ModifiedTime = DateTime.Now;
                });
                CloudDataHelper.Update(metaObject, appUsers);
            }

        }
        internal static List<ObjectData> GetAppUserList(int tenantId, string ids)
        {
            if (!string.IsNullOrWhiteSpace(ids))
            {
                var filter = new BooleanFilter()
                      .Must(new TermsFilter("_id", ids.Split(',').ToArray()))
                      .Must(new TermFilter("AppConnect.AppUser.StdIsDeleted", false));
                var result = CloudDataHelper.GetEntityAllList("AppConnect.AppUser", tenantId, filter);
                return result.ToList();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 更新主账户ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="masterAccountId"></param>
        internal static void UpdateMasterAccountId(int id, int masterAccountId)
        {
            var db = Database.GetDatabase(DatabaseName.UserSecurity);
            SafeProcedure.ExecuteNonQuery(db, "dbo.AppUserAccount_UpdateMasterAccountId", parameterMapper =>
            {
                parameterMapper.AddWithValue("@Id", id);
                parameterMapper.AddWithValue("@MasterAccountId", masterAccountId);
            });
        }

        internal static List<AppUserAccountInfo> List(int tenantId, string appId, int pageSize, int pageNum, out int total)
        {
            var db = Database.GetDatabase(DatabaseName.UserSecurity);
            var totalParameter = new SqlParameter("@Total", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var result = SafeProcedure.ExecuteAndGetInstanceList<AppUserAccountInfo>(db, "dbo.AppUserAccount_List", BuildInfo,
                new SqlParameter[]
                {
                    totalParameter,
                    new SqlParameter("@TenantId", tenantId),
                    new SqlParameter("@AppId", appId),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageNum", pageNum)
                });
            total = (int)totalParameter.Value;

            return result;
        }

        internal static List<AppUserAccountInfo> GetListByUserId(int tenantId, string userIds)
        {
            var db = Database.GetDatabase(DatabaseName.UserSecurity);
            var list = SafeProcedure.ExecuteAndGetInstanceList<AppUserAccountInfo>(db, "dbo.AppUserAccount_GetListByUserId", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@UserIds", userIds);

            }, BuildInfo);

            return list;
        }

        internal static List<AppUserAccountInfo> GetListByUserId(int tenantId, string userIds, string appId)
        {
            var db = Database.GetDatabase(DatabaseName.UserSecurity);
            var list = SafeProcedure.ExecuteAndGetInstanceList<AppUserAccountInfo>(db, "dbo.AppUserAccount_GetListByUserIdAndAppId", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@UserIds", userIds);
                parameterMapper.AddWithValue("@AppId", appId);

            }, BuildInfo);

            return list;
        }

        internal static List<AppUserAccountInfo> ConvertToUserId(int tenantId, string appId, string openIds)
        {
            var db = Database.GetDatabase(DatabaseName.UserSecurity);
            var list = SafeProcedure.ExecuteAndGetInstanceList<AppUserAccountInfo>(db, "dbo.AppUserAccount_ConvertToUserId", parameterMapper =>
            {
                parameterMapper.AddWithValue("@AppId", appId);
                parameterMapper.AddWithValue("@OpenIds", openIds);
                parameterMapper.AddWithValue("@TenantId", tenantId);

            }, delegate (IRecord record, AppUserAccountInfo entity)
            {
                entity.OpenId = record.Get<string>("OpenId");
                entity.TenantId = record.Get<int>("TenantId");
                entity.UserId = record.Get<int>("UserId");
            });


            return list;
        }

        /// <summary>
        /// 数据映射
        /// </summary>
        /// <param name="record">记录</param>
        /// <param name="info">实体</param>
        private static void BuildInfo(IRecord record, AppUserAccountInfo info)
        {
            info.Id = record.Get<int>("Id");
            info.AppId = record.Get<string>("AppId");
            info.OpenId = record.Get<string>("OpenId");
            info.TenantId = record.Get<int>("TenantId");
            info.UserId = record.Get<int>("UserId");
            info.BeisenAccount = record.Get<string>("BeisenAccount");
            info.Type = (AppUserAccountType)record.Get<short>("Type");
            info.State = (AppUserAccountState)record.Get<short>("State");
            info.MasterAccountId = record.Get<int>("MasterAccountId");
            info.CreateTime = record.Get<DateTime>("CreateTime");
            info.ModifyTime = record.Get<DateTime>("ModifyTime");
            info.ActivateTime = record.GetOrDefault<DateTime?>("ActivateTime", null);
            info.UnbindTime = record.GetOrDefault<DateTime?>("UnbindTime", null);
        }
        /// <summary>
        /// 删除数据记录
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal static int DeleteAppUser(int tenantId, int userId)
        {
            var db = Database.GetDatabase(DatabaseName.UserSecurity);
            var count = SafeProcedure.ExecuteNonQuery(db, "dbo.DAL_AppUserAccount_DeleteUser", parameterMapper =>
            {
                parameterMapper.AddWithValue("@TenantId", tenantId);
                parameterMapper.AddWithValue("@UserId", userId);
            });
            return count;
        }
        /// <summary>
        /// 删除数据记录(Cloud)
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        internal static void DeleteCLoud(int tenantId, int userId)
        {
            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = tenantId;
            ApplicationContext.Current.UserId = userId;
            var filter = new BooleanFilter()
                   .Must(new TermFilter(AppUserConstants.AppUser_UserId, userId));
            var resultList = CloudDataHelper.GetEntityAllList("AppConnect.AppUser", tenantId, filter);
            var objectDataIds = resultList.Select(t => Convert.ToString(t.ID));
            CloudDataHelper.Delete("AppConnect.AppUser", tenantId, objectDataIds.ToArray());
        }
    }
}

using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.Tool.Common;
using Beisen.Data;
using Beisen.MultiTenant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Froms = System.Windows.Forms;

namespace Beisen.AppConnect.Tool.RepairMethod
{
    public class AppUserRepair
    {
        public static int userId = 100000;

        public static void RunRepair_AppUserRepair(List<int> tenantIdList, string metaObjectName, ref string message)
        {
            try
            {
                var appUserAccountInfoDic = GetAppUserAccountInSql();
                if (appUserAccountInfoDic != null && appUserAccountInfoDic.Count > 0)
                {
                    foreach (var userInfo in appUserAccountInfoDic)
                    {
                        //霸气一回，先删后加
                        AddCloudData(userInfo.Key, appUserAccountInfoDic[userInfo.Key], ref message);
                    }
                }
            }
            catch (Exception ex)
            {
                message = "组件状态修复失败！" + ex.Message;
                Froms.MessageBox.Show("组件状态修复失败！");
                AppConnectLogHelper.Error("组件状态修复失败！", ex);
            }
        }

        private static void AddCloudData(int tenantId, List<AppUserAccountInfo> appUserAccountInfos, ref string message)
        {
            try
            {
                message += string.Format("租户：{0} 开始处理数据\r\n", tenantId);
                BeisenUserDao.SetContext(tenantId);
                //先删除该租户数据
                List<string> delObjectIds = new List<string>();
                var multiDataDic = GetMultiTenantData(tenantId);
                foreach (var item in multiDataDic)
                {
                    if (!delObjectIds.Contains(Convert.ToString(item.ID)))
                        delObjectIds.Add(Convert.ToString(item.ID));
                }
                CloudDataHelper.Delete("AppConnect.AppUser", tenantId, delObjectIds.ToArray());

                message += string.Format("租户：{0} 开始删除{1}条数据\r\n", tenantId, delObjectIds.Count);

                var metaObject = CloudDataHelper.GetMetaObject(tenantId, "AppConnect.AppUser");
                List<ObjectData> objectDatas = new List<ObjectData>();
                foreach (var appUserAccountInfo in appUserAccountInfos)
                {
                    ObjectData objectData = new ObjectData(metaObject);
                    objectData.ID = Guid.NewGuid();
                    objectData.CreatedBy = appUserAccountInfo.UserId;
                    objectData.CreatedTime = appUserAccountInfo.CreateTime;
                    objectData.ModifiedBy = appUserAccountInfo.UserId;
                    objectData.ModifiedTime = appUserAccountInfo.ModifyTime;
                    objectData["StdIsDeleted"] = false;
                    objectData["TenantId"] = appUserAccountInfo.TenantId;
                    objectData["UserId"] = appUserAccountInfo.UserId;
                    objectData["UserEmail"] = appUserAccountInfo.BeisenAccount;
                    objectData["State"] = appUserAccountInfo.StateNotEnum;
                    objectData["ActivateTime"] = appUserAccountInfo.CreateTime;
                    objectData["OpenId"] = appUserAccountInfo.OpenId;
                    objectData["AppId"] = appUserAccountInfo.AppId;
                    objectData["SqlId"] = appUserAccountInfo.Id;
                    objectData["LoginType"] = appUserAccountInfo.TypeNotEnum;
                    objectData["SqlId"] = appUserAccountInfo.Id;
                    objectData["MasterAccountId"] = appUserAccountInfo.MasterAccountId;
                    objectDatas.Add(objectData);
                }
                CloudDataHelper.Add(metaObject, objectDatas);
                message += string.Format("租户：{0} 处理完毕，共处理{1}条数据 \r\n", tenantId, objectDatas.Count);
            }
            catch (Exception ex)
            {
                message += string.Format("租户：{0} 修复数据失败！\r\n", tenantId);
                AppConnectLogHelper.ErrorFormat(message, ex);
                Froms.MessageBox.Show(ex.Message);
            }
        }

        private static List<ObjectData> GetMultiTenantData(int tenantId)
        {
            var sortString = string.Format("{0}.{1}", "AppConnect.AppUser", "SqlId");
            Dictionary<string, SortDirection> sortFields = new Dictionary<string, SortDirection>();
            sortFields.Add(sortString, SortDirection.Asc);
            var list = CloudDataHelper.GetEntityAllList("AppConnect.AppUser", tenantId, sortFields: sortFields);
            if (list == null)
            {
                return null;
            }
            return list.ToList();
        }

        private static Dictionary<int, List<AppUserAccountInfo>> GetAppUserAccountInSql()
        {
            Dictionary<int, List<AppUserAccountInfo>> appUserAccountInfoDic = new Dictionary<int, List<AppUserAccountInfo>>();
            Database database = Database.GetDatabase("BeisenUserSecurity");
            SafeProcedure.ExecuteAndGetInstanceList(database, "[dbo].[AppUserAccount_GetAll]",
            delegate (IRecord record, int entity)
           {
               AppUserAccountInfo appUserAccountInfo = new AppUserAccountInfo();
               appUserAccountInfo.Id = record.GetOrDefault<int>("Id", 0);
               appUserAccountInfo.AppId = record.GetOrDefault<string>("AppId", "");
               appUserAccountInfo.OpenId = record.GetOrDefault<string>("OpenId", "");
               appUserAccountInfo.TenantId = record.GetOrDefault<int>("TenantId", 0);
               appUserAccountInfo.UserId = record.GetOrDefault<int>("UserId", 0);
               appUserAccountInfo.BeisenAccount = record.GetOrDefault<string>("BeisenAccount", "");
               appUserAccountInfo.TypeNotEnum = record.GetOrDefault<Int16>("Type", 0);
               appUserAccountInfo.StateNotEnum = record.GetOrDefault<Int16>("State", 0);
               appUserAccountInfo.CreateTime = record.GetOrDefault<DateTime>("CreateTime", DateTime.Now);
               appUserAccountInfo.ActivateTime = record.GetOrDefault<DateTime>("ActivateTime", appUserAccountInfo.CreateTime);
               appUserAccountInfo.UnbindTime = record.GetOrDefault<DateTime>("UnbindTime", DateTime.Now);
               appUserAccountInfo.MasterAccountId = record.GetOrDefault<int>("MasterAccountId", 0);

               if (appUserAccountInfoDic.ContainsKey(appUserAccountInfo.TenantId))
               {
                   var existList = appUserAccountInfoDic[appUserAccountInfo.TenantId];
                   existList.Add(appUserAccountInfo);
                   appUserAccountInfoDic[appUserAccountInfo.TenantId] = existList;
               }
               else
               {
                   appUserAccountInfoDic.Add(appUserAccountInfo.TenantId, new List<AppUserAccountInfo> { appUserAccountInfo });
               }
           });

            return appUserAccountInfoDic;
        }

        private static void AddCLoud(int tenantId, List<AppUserAccountInfo> appUserAccountInfos, ref string message)
        {
            try
            {
                BeisenUserDao.SetContext(tenantId);
                message += string.Format("租户：{0} 开始处理\r\n", tenantId);
                var metaObject = CloudDataHelper.GetMetaObject(tenantId, "AppConnect.AppUser");
                List<ObjectData> objectDatas = new List<ObjectData>();
                var appUserAccountInfos_tenantId = appUserAccountInfos.Where(w => w.TenantId == tenantId).ToList();
                if (appUserAccountInfos_tenantId != null && appUserAccountInfos_tenantId.Count > 0)
                {
                    appUserAccountInfos_tenantId.ForEach(appUserAccountInfo =>
                    {
                        ObjectData objectData = new ObjectData(metaObject);
                        objectData.ID = Guid.NewGuid();
                        objectData.CreatedBy = appUserAccountInfo.UserId;
                        objectData.CreatedTime = DateTime.Now;
                        objectData.ModifiedBy = appUserAccountInfo.UserId;
                        objectData.ModifiedTime = DateTime.Now;
                        objectData["StdIsDeleted"] = false;
                        objectData["TenantId"] = appUserAccountInfo.TenantId;
                        objectData["UserId"] = appUserAccountInfo.UserId;
                        objectData["UserEmail"] = appUserAccountInfo.BeisenAccount;
                        objectData["State"] = appUserAccountInfo.StateNotEnum;
                        objectData["ActivateTime"] = DateTime.Now;
                        objectData["OpenId"] = appUserAccountInfo.OpenId;
                        objectData["AppId"] = appUserAccountInfo.AppId;
                        objectData["SqlId"] = appUserAccountInfo.Id;
                        objectData["LoginType"] = appUserAccountInfo.TypeNotEnum;
                        objectData["SqlId"] = appUserAccountInfo.Id;
                        objectData["MasterAccountId"] = appUserAccountInfo.MasterAccountId;
                        objectDatas.Add(objectData);
                    });
                }
                CloudDataHelper.Add(metaObject, objectDatas);
                message += string.Format("租户：{0} 处理完毕，共处理{objectDatas.Count}条数据 \r\n", tenantId);
            }
            catch (Exception ex)
            {
                message += string.Format("租户：{0} 修复数据失败！\r\n", tenantId);
                AppConnectLogHelper.ErrorFormat(message, ex);
                Froms.MessageBox.Show(ex.Message);
            }
        }

        private static List<int> GetTenantId(List<AppUserAccountInfo> appUserAccountInfos)
        {
            if (appUserAccountInfos != null && appUserAccountInfos.Count > 0)
            {
                var tenantIds = appUserAccountInfos.Select(s => s.TenantId).Distinct().ToList();
                return tenantIds;
            }
            else
            {
                return null;
            }
        }
    }
}
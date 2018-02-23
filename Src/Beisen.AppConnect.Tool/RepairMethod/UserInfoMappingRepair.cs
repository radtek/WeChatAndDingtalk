using Beisen.AppConnect.Tool.Common;
using Beisen.AppConnect.Tool.Common.Config;
using Beisen.AppConnect.Tool.Model;
using Beisen.MultiTenant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Beisen.AppConnect.Tool.RepairMethod
{
    public class UserInfoMappingRepair
    {
        public static Logging.LogWrapper logger = new Logging.LogWrapper();
        public static int userId = 100000;

        public static void RepairUserInfoMapping(List<int> tenantIdList, string metaObjectName, string fieldName, ref string message)
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(message))
                {
                    MessageBox.Show("数据修复失败，原因：{0}", message);
                    return;
                }
                switch (fieldName)
                {
                    case "UserId":
                        RunRepair_UserId(tenantIdList, metaObjectName, ref message);
                        break;
                    case "UserInfo":
                        MigrationUserInfoMapping(metaObjectName, ref message);
                        break;
                    default:
                        MessageBox.Show("字段名错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }
            }
            catch (Exception ex)
            {
                message = "修复失败！" + ex.Message;
                MessageBox.Show("数据修复失败！");
                logger.Error("修复失败！", ex);
            }
        }

        public static void RunRepair_UserId(List<int> tenantIdList, string metaObjectName, ref string message)
        {
            foreach (var tenantId in tenantIdList)
            {
                try
                {
                    BeisenUserDao.SetContext(tenantId);
                    message += string.Format("租户：{tenantId} 开始处理\r\n");
                    var userInfoMappingList = CloudDataHelper.GetEntityAllList(metaObjectName, tenantId).ToList();
                    var count = userInfoMappingList.Count;
                    if (!userInfoMappingList.Any())
                    {
                        message += string.Format("租户：{tenantId} 处理完毕，共处理{0}条数据 \r\n");
                        continue;
                    }
                    int tempId = 0;
                    foreach (var item in userInfoMappingList)
                    {
                        item["StaffId"] = item["UserId"];
                        tempId++;
                    }
                    if (tempId > 0)
                    {
                        var metaobj = CloudDataHelper.GetMetaObject(tenantId, metaObjectName);
                        CloudDataHelper.Update(metaobj, userInfoMappingList);
                    }
                    message += string.Format("租户：{tenantId} 处理完毕，UserInfo映射信息共{count}条记录，处理{tempId}条数据 \r\n");
                }
                catch (Exception ex)
                {
                    message += string.Format("租户：{tenantId} 修复数据失败！\r\n");
                    logger.ErrorFormat(message, ex);
                    MessageBox.Show(message);
                    return;
                }
            }

        }

        public static void MigrationUserInfoMapping(string metaObjectName, ref string message)
        {
            try
            {
                var appId = SqlHelper.GetAppIdByType();
                var appIdList = appId.Select(n => Convert.ToString(n.AppId)).ToList();
                var appIdStr = string.Join(",", appIdList.ToArray());

                var userInfoList = SqlHelper.GetInfoByAppId(appIdStr);
                var userInfoGroupByTenantId = userInfoList.GroupBy(n => n.TenantId);

                foreach (var item in userInfoGroupByTenantId)
                {
                    //存储到各个租户
                    var metaObj = CloudDataHelper.GetMetaObject(item.Key, metaObjectName);
                    List<ObjectData> objectDataList = new List<ObjectData>();
                    foreach (var info in item)
                    {
                        var data = saveCloud(metaObj, info);
                        objectDataList.Add(data);
                    }
                    CloudDataHelper.Add(metaObj, objectDataList);
                    //存储到公共租户
                    var sysMetaObj = CloudDataHelper.GetMetaObject(SystemInfo.ISVSystemTenantId, metaObjectName);
                    List<ObjectData> sysObjectDataList = new List<ObjectData>();
                    foreach (var info in item)
                    {
                        var data = saveCloud(sysMetaObj, info);
                        objectDataList.Add(data);
                    }
                    CloudDataHelper.Add(sysMetaObj, sysObjectDataList);
                }
            }
            catch (Exception ex)
            {
                message += string.Format("租户：{tenantId} 修复数据失败！\r\n");
                logger.ErrorFormat(message, ex);
                MessageBox.Show(message);
                return;
            }

        }

        public static ObjectData saveCloud(MetaObject metaObj, UserInfoModel info)
        {
            ObjectData data = new ObjectData(metaObj);
            data.CreatedBy = userId;
            data.CreatedTime = DateTime.Now;
            data.ModifiedBy = userId;
            data.ModifiedTime = DateTime.Now;
            data["StdIsDeleted"] = false;
            data["SuiteKey"] = SystemInfo.SuiteKey; //配置文件
            data["ISVTenantId"] = info.TenantId;
            data["UserId"] = info.UserId;
            data["UserType"] = info.Type;
            data["MappingUserId"] = info.OpenId;
            data["CorpId"] = info.AppId;
            data["Status"] = info.State;// 值需要对应一下，，值代表的含义不一致
            data["StaffId"] = info.UserId;
            data["MappingType"] = "21";
            data["AppId"] = SystemInfo.AppId; //配置文件

            return data;
        }

    }
}


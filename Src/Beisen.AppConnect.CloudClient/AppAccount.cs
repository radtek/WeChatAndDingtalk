using System;
using System.Collections.Generic;
using Beisen.AppConnect.CloudClient.Configuration;
using Beisen.AppConnect.CloudClient.Models;
using Beisen.AppConnect.CloudClient.RequestUtility;
using Beisen.Common.Context;
using Beisen.MultiTenant.Model;
using Beisen.MultiTenant.Model.UIMetaDataV2.TableList;
using Beisen.MultiTenant.Model.UIMetaDataV2;
using RestSharp;

namespace Beisen.AppConnect.CloudClient
{
    public class AppAccount
    {
        public static List<DataSourceObject> GetAppAccountType()
        {
            var url = string.Format("{0}/appaccount/type", AppConnectHostConfig.Cache[1]);

            var resquestResult = Request.SendRequest<AppAccountTypesResult>(url);

            var result = new List<DataSourceObject>();
            foreach (var type in resquestResult.TypeList)
            {
                result.Add(new DataSourceObject(type.Descript, type.Type.ToString()));
            }

            return result;
        }

        public static TableListComponent GetList(SearchCondition conditon, TableListComponent result)
        {
            for (var i = 0; i < 40; i++)
            {
                var data = new Dictionary<string, FieldBizData>();
                var id = Guid.NewGuid().ToString();
                data.Add("_id", new FieldBizData { Name = "_id", Text = id, Value = id });
                data.Add("AgentId", new FieldBizData { Name = "AgentId", Text = "AgentId" + i, Value = "AgentId" + i });
                data.Add("AppId", new FieldBizData { Name = "AppId", Text = "AppId" + i, Value = "AppId" + i });
                data.Add("AppSecret", new FieldBizData { Name = "AppSecret", Text = "AppSecret" + i, Value = "AppSecret" + i });
                data.Add("Name", new FieldBizData { Name = "Name", Text = "Name" + i, Value = "Name" + i });
                data.Add("SourceId", new FieldBizData { Name = "SourceId", Text = "SourceId" + i, Value = "SourceId" + i });
                data.Add("State", new FieldBizData { Name = "State", Text = "启用", Value = "0" });
                data.Add("StdIsDeleted", new FieldBizData { Name = "StdIsDeleted", Text = "否", Value = "0" });
                data.Add("Tag", new FieldBizData { Name = "Tag", Text = "Tag" + i, Value = "Tag" + i });
                data.Add("Type", new FieldBizData { Name = "Type", Text = "微信企业号", Value = "21" });
                result.BizData.Add(data);
            }

            result.CmpData.Paging.Total = 40;

            return result;
        }

        public static OperationResult Save(ObjectData dataObject, OperationResult result)
        {
            if (result.Code == 200)
            {
                var tenantId = ApplicationContext.Current.TenantId;
                var userId = ApplicationContext.Current.UserId;

                var appAccountInfo = new SaveAppAccountArgument();
                appAccountInfo.AppAccountId = dataObject.ID.ToString();
                appAccountInfo.TenantId = dataObject.TenantID;
                appAccountInfo.Name = dataObject["Name"] as string;
                appAccountInfo.Type = Convert.ToInt32(dataObject["Type"]);
                appAccountInfo.SourceId = dataObject["SourceId"] as string;
                appAccountInfo.AppId = dataObject["AppId"] as string;
                appAccountInfo.AppSecret = dataObject["AppSecret"] as string;
                appAccountInfo.State = Convert.ToInt32(dataObject["State"]);
                appAccountInfo.AgentId = dataObject["AgentId"] as string;
                appAccountInfo.Tag = dataObject["Tag"] as string;
                appAccountInfo.CreateBy = userId;
                appAccountInfo.ModifyBy = userId;

                var url = string.Format("{0}/AppAccount/SaveForCloud?tenant_id={1}&appaccount_id={2}", AppConnectHostConfig.Cache[1], tenantId, appAccountInfo.AppAccountId);
                Request.SendRequest<ApiResult>(url, Method.POST, appAccountInfo);
            }

            return result;
        }

        public static OperationResult Delete(OperationResult result)
        {
            if (result.Code == 200)
            {
                var tenantId = ApplicationContext.Current.TenantId;
                var userId = ApplicationContext.Current.UserId;

                var url = string.Format("{0}/AppAccount/DeleteForCloud?tenant_id={1}&user_id={2}&appaccount_id={3}", AppConnectHostConfig.Cache[1], tenantId, userId, result.ID);
                Request.SendRequest<ApiResult>(url, Method.POST);
            }

            return result;
        }
    }
}

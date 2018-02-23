using Beisen.MultiTenant.Model.UIMetaDataV2;
using Beisen.MultiTenant.Model.UIMetaDataV2.TableList;
using System.Collections.Generic;
using Beisen.Common.Context;
using Beisen.AppConnect.CloudClient.Configuration;
using Beisen.AppConnect.CloudClient.Models;
using Beisen.AppConnect.CloudClient.RequestUtility;
using Beisen.MultiTenant.Model;
using RestSharp;

namespace Beisen.AppConnect.CloudClient
{
    public class AppUser
    {
        public static TableListComponent GetList(SearchCondition conditon, TableListComponent result)
        {
            var tenantId = ApplicationContext.Current.TenantId;
            var appAccountId = conditon.PageContext.DataId;
            var pageSize = result.CmpData.Paging.Capacity;
            var pageNum = result.CmpData.Paging.Page;

            var url = string.Format("{0}/user/ListForCloud?tenant_id={1}&appaccount_id={2}&page_size={3}&page_num={4}", AppConnectHostConfig.Cache[1], tenantId,appAccountId, pageSize, pageNum);

            var resquestResult = Request.SendRequest<ListForCloudResult>(url);
            foreach (var appUser in resquestResult.List)
            {
                var data = new Dictionary<string, FieldBizData>();
                data.Add("_id", new FieldBizData { Name = "_id", Text = appUser.Id.ToString(), Value = appUser.Id.ToString() });
                data.Add("UserName", new FieldBizData { Name = "UserName", Text = appUser.UserName, Value = appUser.UserName });
                data.Add("UserEmail", new FieldBizData { Name = "UserEmail", Text = appUser.UserEmail, Value = appUser.UserEmail });
                data.Add("State", new FieldBizData { Name = "State", Text = appUser.State, Value = appUser.State });
                data.Add("CreatedTime", new FieldBizData { Name = "CreatedTime", Text = appUser.CreateTime, Value = appUser.CreateTime });
                data.Add("ActivateTime", new FieldBizData { Name = "ActivateTime", Text = appUser.ActivateTime, Value = appUser.ActivateTime });
                data.Add("UnbindTime", new FieldBizData { Name = "UnbindTime", Text = appUser.UnbindTime, Value = appUser.UnbindTime });
                data.Add("StdIsDeleted", new FieldBizData { Name = "StdIsDeleted", Text = "否", Value = "0" });
                result.BizData.Add(data);
            }

            result.CmpData.Paging.Total = resquestResult.Total;
            return result;
        }

        public static OperationResult Unbind(int tenantId, int userId, string ids)
        {
            var url = string.Format("{0}/user/unbind?tenant_id={1}&user_id={2}&ids={3}", AppConnectHostConfig.Cache[1], tenantId, userId, ids);

            var resquestResult = Request.SendRequest<ApiResult>(url, Method.POST);

            if (resquestResult.ErrCode != 0)
            {
                return new OperationResult { Code = 500, Message = "解绑失败" };
            }
            return new OperationResult {Code = 200, Message = "解绑成功"};
        }
    }
}

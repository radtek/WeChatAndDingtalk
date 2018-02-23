using System;
using System.Collections.Generic;
using System.Web.Http;
using Beisen.AppConnect.Api.Controller.Models;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using System.Net.Http;

namespace Beisen.AppConnect.Api.Controller.Controllers
{
    public class AppAccountController:ApiControllerBase
    {      
        [HttpGet]
        public ApiResult Get(int tenant_id, string appaccount_id="", string tag="")
        {
            if (string.IsNullOrWhiteSpace(_AppAccountId))
            {
                return new ApiResult
                {
                    ErrCode = 2002,
                    ErrMsg = "账户不存在"
                };
            }
            var appAccount = ProviderGateway.AppAccountProvider.Get(_AppAccountId);

            return new GetAppAccountResult
            {
                AppAccountId= appAccount.AppAccountId,
                TenantId = appAccount.TenantId,
                Name = appAccount.Name,
                Type = appAccount.Type,
                SourceId = appAccount.SourceId,
                AppId = appAccount.AppId,
                AppSecret = appAccount.AppSecret,
                AgentId = appAccount.AgentId,
                State = (int)appAccount.State,
                Tag=appAccount.Tag
            };
        }

        [HttpPost]
        public ApiResult Create([FromUri]int tenant_id, [FromBody]AddAppAccountArgument model)
        {
            var appAccount = new AppAccountInfo
            {
                AppAccountId = Guid.NewGuid().ToString(),
                TenantId = model.tenant_id,
                Name = model.name,
                Type = model.type,
                SourceId = model.source_id,
                AppId = model.app_id,
                AppSecret = model.app_secret,
                AgentId = model.agent_id,
                State = ServiceInterface.Model.Enum.AppAccountState.Enable,
                Tag=model.tag
            };

            var appAccountId = ProviderGateway.AppAccountProvider.Add(tenant_id, appAccount);

            return new AddAppAccountResult
            {
                AppAccountId= appAccountId
            };
        }

        
        [HttpPost]
        public ApiResult Update([FromBody]UpdateAppAccountArgument model, [FromUri]int tenant_id, [FromUri]string appaccount_id = "", [FromUri]string tag = "")
        {
            var appAccount = new AppAccountInfo
            {
                AppAccountId = _AppAccountId,
                TenantId = model.tenant_id,
                Name = model.name,
                Type = model.type,
                SourceId = model.source_id,
                AppId = model.app_id,
                AppSecret = model.app_secret,
                AgentId = model.agent_id,
                State = (AppAccountState)model.state,
                Tag= model.tag
            };

            ProviderGateway.AppAccountProvider.Update(tenant_id, appAccount);

            return new ApiResult
            {
                ErrCode=0
            };
        }

        [HttpGet]
        public ApiResult Type()
        {
            var types = ProviderGateway.AppAccountProvider.GetAppAccountTypes();

            var result = new AppAccountTypesResult();
            result.TypeList = new List<AppAccountTypeResult>();
            foreach (var type in types)
            {
                result.TypeList.Add(new AppAccountTypeResult {Type = type.Key, Descript = type.Value});
            }

            return result;
        }

        public ApiResult SaveForCloud([FromUri] int tenant_id, [FromUri] string appaccount_id, [FromBody] UpdateAppAccountArgument model)
        {
            var appAccount = new AppAccountInfo
            {
                AppAccountId = appaccount_id,
                TenantId = model.tenant_id,
                Name = model.name,
                Type = model.type,
                SourceId = model.source_id,
                AppId = model.app_id,
                AppSecret = model.app_secret,
                AgentId = model.agent_id,
                State = (AppAccountState)model.state,
                Tag = model.tag,
                CreateBy=model.create_by,
                ModifyBy=model.modify_by
            };

            ProviderGateway.AppAccountProvider.AddOrUpdate(tenant_id, appAccount, false);

            return new ApiResult
            {
                ErrCode = 0
            };
        }

        [HttpPost]
        public ApiResult DeleteForCloud([FromUri] int tenant_id, [FromUri] int user_id, [FromUri] string appaccount_id)
        {
            ProviderGateway.AppAccountProvider.UpdateState(tenant_id, user_id, appaccount_id, AppAccountState.Deleted);

            return new ApiResult
            {
                ErrCode = 0
            };
        }

        [HttpGet]
        public ApiResult List(int tenant_id, int user_id)
        {
            var list = ProviderGateway.AppAccountProvider.GetListByTenantId(tenant_id);

            var result = new AppAccountListResult();
            result.List = new List<AppAccountListDetail>();
            foreach (var appAccount in list)
            {
                result.List.Add(new AppAccountListDetail {AppAccountId = appAccount.AppAccountId, Name = appAccount.Name, Type = appAccount.Type});
            }

            return result;
        }
    }
}

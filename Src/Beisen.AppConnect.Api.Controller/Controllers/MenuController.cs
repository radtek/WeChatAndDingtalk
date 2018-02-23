using System.Collections.Generic;
using System.Web.Http;
using Beisen.AppConnect.Api.Controller.Models;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;

namespace Beisen.AppConnect.Api.Controller.Controllers
{
    public class MenuController : ApiControllerBase
    {
        [HttpPost]
        public ApiResult Create([FromBody]MenuCreateArgument model, [FromUri]int tenant_id, [FromUri]string appaccount_id = "", [FromUri]string tag = "")
        {
            var menuInfo = new MenuInfo
            {
                Name=model.name,
                TenantId = model.tenant_id,
                AppAccountId = _AppAccountId,
                BeisenAppId = model.beisen_app_id,
                Url = model.url
            };

            var menuId = ProviderGateway.MenuProvider.Create(tenant_id, menuInfo);

            return new MenuCreateResult
            {
                MenuId = menuId
            };
        }

        [HttpPost]
        public ApiResult Update([FromUri]int tenant_id, [FromUri]string menu_id, [FromBody]MenuCreateArgument model)
        {
            var appaccount_id = ProviderGateway.AppAccountProvider.GetAppAccountId(tenant_id, model.appaccount_id, model.tag);

            var menuInfo = new MenuInfo
            {
                MenuId=menu_id,
                Name = model.name,
                TenantId = model.tenant_id,
                AppAccountId = appaccount_id,
                BeisenAppId = model.beisen_app_id,
                Url = model.url
            };

            ProviderGateway.MenuProvider.Update(tenant_id, menuInfo);

            return new ApiResult();
        }

        [HttpGet]
        public ApiResult List(int tenant_id,int user_id=0)
        {
            var list = ProviderGateway.MenuProvider.GetList(tenant_id);
            var result = new MenuGetListResult();
            result.MenuList = new List<MenuGetResult>();
            foreach (var menu in list)
            {
                result.MenuList.Add(new MenuGetResult
                {
                    MenuId = menu.MenuId,
                    Name = menu.Name,
                    TenantId = menu.TenantId,
                    AppAccountId = menu.AppAccountId,
                    BeisenAppId = menu.BeisenAppId,
                    Url = menu.Url
                });
            }

            return result;
        }
    }
}
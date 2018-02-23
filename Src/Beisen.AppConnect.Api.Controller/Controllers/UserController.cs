using System.Collections.Generic;
using System.Web.Http;
using Beisen.AppConnect.Api.Controller.Models;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.Infrastructure.Helper;
using Newtonsoft.Json;
namespace Beisen.AppConnect.Api.Controller.Controllers
{
    public class UserController : ApiControllerBase
    {
        [HttpGet]
        public ApiResult OpenId(int tenant_id, int user_id, string appaccount_id = "", string tag = "")
        {
            var account = ProviderGateway.AppAccountProvider.Get(_AppAccountId);
            var appUser = ProviderGateway.AppUserAccountProvider.GetByUserId(tenant_id, user_id, account.AppId);

            if (appUser == null)
            {
                return new OpenIdResult
                {
                    ErrCode = 20002,
                    ErrMsg = "用户未绑定"
                };
            }

            return new OpenIdResult
            {
                OpenId = appUser.OpenId
            };
        }

        [HttpGet]
        public ApiResult ListForCloud(int tenant_id, string appaccount_id, int page_size, int page_num)
        {
            int total;
            var appUsers = ProviderGateway.AppUserAccountProvider.GetCloudList(tenant_id, appaccount_id, page_size, page_num, out total);

            var result = new ListForCloudResult();
            result.List = new List<ListForCloudDetail>();
            result.Total = total;
            foreach (var appUser in appUsers)
            {
                result.List.Add
                    (
                        new ListForCloudDetail
                        {
                            Id = appUser.Id,
                            UserName = appUser.UserName,
                            UserEmail = appUser.UserEmail,
                            State = ProviderGateway.AppUserAccountProvider.GetStateName(appUser.State),
                            CreateTime = appUser.CreateTime.ToString("yyyy/MM/dd HH:mm:ss"),
                            ActivateTime = appUser.ActivateTime == null ? "" : appUser.ActivateTime.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                            UnbindTime = appUser.UnbindTime == null ? "" : appUser.UnbindTime.Value.ToString("yyyy/MM/dd HH:mm:ss")
                        }
                    );
            }

            return result;
        }

        [HttpPost]
        public ApiResult Unbind(int tenant_id, int user_id, string ids)
        {
            AppConnectLogHelper.Debug("ids------------1111" + ids);
            ProviderGateway.AppUserAccountProvider.UnBind(tenant_id, user_id, ids);
            return new ApiResult { ErrCode = 0 };
        }

        [HttpPost]
        public ApiResult ConvertToUserId([FromBody]ConvertToUserIdArgument openIds, [FromUri]int tenant_id, [FromUri]string appaccount_id = "", [FromUri]string tag = "")
        {
            var result = new ConvertToUserIdResult();

            if (openIds.open_id_list.Count > 100)
            {
                result.ErrCode = 20001;
                result.ErrMsg = "查询openid数量过多";
                return result;
            }

            var userList = ProviderGateway.AppUserAccountProvider.ConvertToUserId(_AppAccountId, openIds.open_id_list);
            result.UserList = new List<ConvertToUserIdDetail>();
            foreach (var user in userList)
            {
                result.UserList.Add(new ConvertToUserIdDetail { OpenId = user.OpenId, TenantId = user.TenantId, UserId = user.UserId });
            }

            return result;
        }

        [HttpGet]
        public ApiResult UserMaps(int tenant_id, string appaccount_id = "", string tag = "")
        {
            if (string.IsNullOrWhiteSpace(_AppAccountId))
            {
                return new ApiResult
                {
                    ErrCode = 2002,
                    ErrMsg = "账户不存在"
                };
            }

            var account = ProviderGateway.AppAccountProvider.Get(_AppAccountId);
            var states = System.Convert.ToInt32(Beisen.AppConnect.ServiceInterface.Model.Enum.AppUserAccountState.Activated).ToString();
            var appUsers = ProviderGateway.AppUserAccountProvider.GetByTenantId(tenant_id, account.AppId, states);

            if (appUsers == null)
            {
                return new UserMapsResult
                {
                    ErrCode = 0,
                    TenantId = tenant_id,
                    Total = 0
                };
            }

            var userMaps = new Dictionary<int, string>();
            appUsers.ForEach(appUser => userMaps.Add(appUser.UserId, appUser.OpenId));
            return new UserMapsResult
            {
                TenantId = tenant_id,
                Total = userMaps.Count,
                UserMaps = userMaps
            };
        }
    }
}

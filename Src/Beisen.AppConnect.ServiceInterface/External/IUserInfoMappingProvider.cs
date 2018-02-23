using Beisen.Common.DynamicScript;
using Beisen.MultiTenant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceInterface
{
    [ServiceContract]
    [DynamicMicroService(Name = "UserInfoMapping", Description = "ISV登录映射")]
    public interface IUserInfoMappingProvider
    {
        [OperationContract]
        [DynamicRest(HttpVerb = "POST", UrlTemplate = "UserUnbind", InterceptAction = DynamicScriptAction.None, Description = "用户解绑")]
        OperationResult UserUnbind(string objectIds);

        [OperationContract(Name = "UnBindInfo")]
        void UnBindInfo(int tenantId, int userId);

    }
}

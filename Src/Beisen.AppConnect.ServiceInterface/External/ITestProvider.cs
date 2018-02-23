using Beisen.Common.DynamicScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceInterface
{
    [ServiceContract]
    [DynamicMicroService(Name = "Test", Description = "测试")]
    public interface ITestProvider
    {
        [OperationContract(Name = "GetString")]
        [DynamicRest(HttpVerb = "POST", UrlTemplate = "GetString", InterceptAction = DynamicScriptAction.None, Description = "测试mrest")]
        string GetString();

        [OperationContract]
        [DynamicRest(HttpVerb = "GET", UrlTemplate = "MRestDefService", InterceptAction = DynamicScriptAction.None, Description = "测试MRest")]
        string MRestDefService();
    }
}

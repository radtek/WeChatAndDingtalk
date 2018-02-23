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
    [DynamicMicroService(Name = "AppAccount", Description = "开放应用帐号")]
    public interface IAppAccountMRestProvider
    {
        [OperationContract(Name = "ExportExcelTemplate")]
        [DynamicRest(HttpVerb = "GET", UrlTemplate = "ExportExcelTemplate", InterceptAction = DynamicScriptAction.None, Description = "导出模板")]
        string ExportExcelTemplate();

        [OperationContract(Name = "ImportUserInfoExcel")]
        [DynamicRest(HttpVerb = "POST", UrlTemplate = "ImportUserInfoExcel", InterceptAction = DynamicScriptAction.None, Description = "导入数据")]
        OperationResult ImportUserInfoExcel(ObjectDataFromJson dataObject);
    }
}

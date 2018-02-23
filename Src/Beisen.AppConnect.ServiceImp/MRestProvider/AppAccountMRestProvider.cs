using Beisen.AppConnect.Infrastructure.AppConnectTask;
using Beisen.AppConnect.Infrastructure.ConfigHandler;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceImp.Service;
using Beisen.AppConnect.ServiceInterface;
using Beisen.Common.Context;
using Beisen.DfsClient;
using Beisen.Microservices.Common.Model;
using Beisen.MultiTenant.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Beisen.AppConnect.ServiceImp
{
    public class AppAccountMRestProvider : IAppAccountMRestProvider
    {
        #region Singleton 
        private static readonly IAppAccountMRestProvider _Instance = new AppAccountMRestProvider();
        public static IAppAccountMRestProvider Instance
        {
            get
            {
                return _Instance;
            }
        }
        #endregion
        public string ExportExcelTemplate()
        {
            var tenantId = ApplicationContext.Current.TenantId;
            var userId = ApplicationContext.Current.UserId;
            var path = string.Empty;
            try
            {
                //设置Excel表头
                var templateData = AppAccountService.Instance.GetTemplateStream(tenantId, userId);
                if (templateData.Length == 0)
                {
                    //error
                    AppConnectLogHelper.Error("生成模板失败，文件数据流为null");
                    return path;
                }
                var dfsItem = new DfsItem("AppConnectFile", "人员信息模板表.xls", templateData, tenantId);
                var dfsPath = Dfs.Store(dfsItem);
                path = Dfs.ToDownloadUrl(dfsPath.ToString(), UrlSignDomain.Tms, userId);

                ApplicationContext.Current.Put(Const.BeisenContextXHasException, ExceptionType.UrlRedirect);
                ApplicationContext.Current.Put(Const.BeisenContextXExResultModel, new ResultModel { code = "302", message = "url redirect", param = path });
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.Error("生成模板失败", ex);
            }
            return path;
        }

        public OperationResult ImportUserInfoExcel(ObjectDataFromJson dataObject)
        {
            var resultMsg = new OperationResult() { Code = 200 };
            bool isAsync = false;
            try
            {
                var tenantId = ApplicationContext.Current.TenantId;
                var userId = ApplicationContext.Current.UserId;
                var datalist = dataObject.metaFields;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (var data in datalist)
                {
                    dic.AddOrUpdate(data.name, data.value);
                }
                var dfsPath = dic["FileUpload"];
                var appAccountId = dic["PObjectDataID"];
                //通过Id获取AppAccount数据
                var appAccountInfo = ProviderGateway.AppAccountProvider.Get(appAccountId);
                if (appAccountInfo != null)
                {
                    string appId = appAccountInfo.AppId;
                    //处理excel：当数据大于1000条时 开启异步
                    var workbook = ExcelService.Instance.GetExcel(dfsPath);
                    if (workbook.NumberOfSheets < 1)
                    {
                        AppConnectLogHelper.Error("导入的数据excel缺少sheet页");
                        resultMsg.Code = 500;
                    }
                    else
                    {
                        var staffSheet = workbook.GetSheetAt(0);//获取 人员信息的sheet页
                        if (staffSheet.LastRowNum > EnterpriseESBConfigHandler.Instance.ImportAsyncThreshold)
                        {
                            isAsync = true;
                            AppConnectTask.AddTask(() =>
                            {
                                AppContext.Set(tenantId, userId);
                                //处理比较多的数据
                                AppAccountService.Instance.BatchHandleExcelData(tenantId, userId, staffSheet, appId, isAsync);
                            });
                        }
                        else
                        {
                            AppAccountService.Instance.BatchHandleExcelData(tenantId, userId, staffSheet, appId, isAsync);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.Error("导入人员信息表发生异常：" + ex.Message);
                resultMsg.Code = 417;
                resultMsg.Message = "导入人员信息表异常!";
            }

            string popMsg = isAsync ? "因耗时较长，任务转为后台执行，导入完成后系统将向您发送通知，请留意系统消息" : "导入数据成功！";
            if (resultMsg.Code == 200)
            {
                resultMsg.Message = popMsg;
                resultMsg.TipType = "normalTip";
            }
            else
            {
                ApplicationContext.Current.Put(Const.BeisenContextXHasException, ExceptionType.KnownException);
                ApplicationContext.Current.Put(Const.BeisenContextXExResultModel, new ResultModel { code = "417", message = resultMsg.Message });
            }
            return resultMsg;
        }
    }
}

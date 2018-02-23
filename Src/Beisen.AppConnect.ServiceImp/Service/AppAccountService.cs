using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.DfsClient;
using Beisen.DynamicScript.SDK;
using Beisen.MultiTenant.Model;
using Beisen.UserFramework.Models;
using Beisen.UserFramework.Service;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Beisen.AppConnect.ServiceImp.Service
{
    public class AppAccountService
    {
        private static readonly AppAccountService _Instance = new AppAccountService();
        public static AppAccountService Instance
        {
            get
            {
                return _Instance;
            }
        }

        public byte[] GetTemplateStream(int tenantId, int userId)
        {
            try
            {
                //构造excel数据
                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet("人员信息表");
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 26));
                sheet.SetColumnWidth(0, 30 * 256);
                sheet.SetColumnWidth(1, 30 * 256);
                sheet.SetColumnWidth(2, 30 * 256);
                IRow warningRow = sheet.CreateRow(0);
                warningRow.CreateCell(0).SetCellValue("注意：邮箱与手机必须有一个不为空，否则会导入失败；（从第三行开始导入数据）");

                IRow titleRow = sheet.CreateRow(1);
                titleRow.CreateCell(0).SetCellValue("姓名");
                titleRow.CreateCell(1).SetCellValue("邮箱");
                titleRow.CreateCell(2).SetCellValue("手机");
                using (var ms = new MemoryStream())
                {
                    workbook.Write(ms);
                    return ms.GetBuffer();
                }
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.ErrorFormat("调用GetTemplateStream获取模板数据流报错:{0}", ex.Message);
                return new byte[] { };
            }
        }

        public Dictionary<string, StaffDto> GetUserByMobile(int tenantId, int userId, IEnumerable<string> mobileList)
        {
            var result = new Dictionary<string, StaffDto>();
            if (!mobileList.Any()) return result;
            var provider = StaffService.Instance;
            if (provider != null)
            {
                var res = provider.GetStaffsByMobiles(new StaffsGetByMobilesArgs()
                {
                    TenantId = tenantId,
                    OperatorId = userId,
                    Mobiles = mobileList.ToArray()
                }).Items;
                if (res != null && res.Any())
                {
                    foreach (var data in res)
                    {
                        result.AddOrUpdate(data.Mobile, data);
                    }
                }
            }
            else
            {
                AppConnectLogHelper.Error("UserFramework接口实例化失败");
                throw new Exception("UserFramework接口实例化失败");
            }
            return result;
        }

        public Dictionary<string, StaffDto> GetUserByEmail(int tenantId, int userId, IEnumerable<string> emailList)
        {
            var result = new Dictionary<string, StaffDto>();
            if (!emailList.Any()) return result;
            var provider = StaffService.Instance;
            if (provider != null)
            {
                var res = provider.GetStaffsByEmails(new StaffsGetByEmailsArgs()
                {
                    TenantId = tenantId,
                    OperatorId = userId,
                    Emails = emailList.ToArray()
                }).Items;
                if (res != null && res.Any())
                {
                    foreach (var data in res)
                    {
                        result.AddOrUpdate(data.Email, data);
                    }
                }
            }
            else
            {
                AppConnectLogHelper.Error("UserFramework接口实例化失败");
                throw new Exception("UserFramework接口实例化失败");
            }
            return result;
        }

        internal void BatchHandleExcelData(int tenantId, int userId, ISheet staffSheet, string appId, bool isAsync)
        {
            List<AppUserAccountInfo> appUserAccountInfoList = new List<AppUserAccountInfo>();

            Dictionary<string, string> emailDic = new Dictionary<string, string>();
            Dictionary<string, string> mobileDic = new Dictionary<string, string>();
            //将excel数据 转换成datatable，接着转换成Dictionary
            var dataTable = ExcelService.Instance.ExcelToDataTable(staffSheet);
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var openId = Convert.ToString(dataTable.Rows[i][0]);
                var email = Convert.ToString(dataTable.Rows[i][1]);
                var mobile = Convert.ToString(dataTable.Rows[i][2]);

                if (!string.IsNullOrEmpty(email))
                {
                    emailDic.Add(email, openId);
                }
                else
                {
                    if (!string.IsNullOrEmpty(mobile))
                        mobileDic.Add(mobile, openId);
                    else
                        continue;
                }
            }
            IEnumerable<string> emailList = emailDic.Select(t => t.Key);
            IEnumerable<string> mobileList = mobileDic.Select(t => t.Key);
            var emailResult = GetUserByEmail(tenantId, userId, emailList);
            var mobileResult = GetUserByMobile(tenantId, userId, mobileList);

            foreach (var emailItem in emailResult)
            {
                AppUserAccountInfo userInfo = new AppUserAccountInfo()
                {
                    AppId = appId,
                    OpenId = emailDic[emailItem.Key],
                    TenantId = emailItem.Value.TenantId,
                    UserId = emailItem.Value.UserId,
                    BeisenAccount = emailItem.Value.Email,
                    Type = AppUserAccountType.Batch,
                    State = AppUserAccountState.Activated,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    ActivateTime = DateTime.Now
                };
                appUserAccountInfoList.Add(userInfo);
            }
            foreach (var mobileItem in mobileResult)
            {
                AppUserAccountInfo userInfo = new AppUserAccountInfo()
                {
                    AppId = appId,
                    OpenId = mobileDic[mobileItem.Key],
                    TenantId = mobileItem.Value.TenantId,
                    UserId = mobileItem.Value.UserId,
                    BeisenAccount = mobileItem.Value.Mobile,
                    Type = AppUserAccountType.Batch,
                    State = AppUserAccountState.Activated,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    ActivateTime = DateTime.Now
                };
                appUserAccountInfoList.Add(userInfo);
            }

            foreach (var user in appUserAccountInfoList)
            {
                var rest = ProviderGateway.AppUserAccountProvider.AddOrUpdate(tenantId, user);
                if (rest <= 0)
                {
                    AppConnectLogHelper.ErrorFormat("执行AddOrUpdate失败:tenantId:{0},userInfo:{1}", tenantId, Newtonsoft.Json.JsonConvert.SerializeObject(user));
                }
            }

            if (isAsync)
            {
                TitaMessageHelper.SendMessage(tenantId, userId, "导入数据成功");
            }
        }
    }
}

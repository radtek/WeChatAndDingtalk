using Beisen.Common.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.Test.MRestProviderTest
{
    [TestClass]
    public class MRestProviderTest
    {
        public void SetTestContext(int tenantId = 100002, int userId = 112664957)
        {
            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = tenantId;
            ApplicationContext.Current.UserId = userId;
        }
        [TestMethod]
        public void TestExportExcel()
        {
            SetTestContext();
            var path = AppAccountMRestProvider.Instance.ExportExcelTemplate();
        }
    }
}

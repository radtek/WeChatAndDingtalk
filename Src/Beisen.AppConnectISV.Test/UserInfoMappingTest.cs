using Beisen.AppConnectISV.ServiceImp;
using Beisen.Common.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Test
{
    [TestClass]
    public class UserInfoMappingTest
    {
        [TestMethod]
        public void GetUserInfoMapping()
        {
            ApplicationContext.Current.TenantId = 100039;
            ApplicationContext.Current.UserId = 112140918;
            ApplicationContext.Current.ApplicationName = "AppConnect";
            int tenantId = 100002;
            List<int> userIds = new List<int> { 112737565 };
            string appAccountId = "eaadfd00-7804-4eb2-a7aa-98543db32c4c";
            UserInfoMappingProvider provider = new UserInfoMappingProvider();
            var result = provider.GetUserInfoMapping(tenantId, userIds, appAccountId);
        }
    }
}

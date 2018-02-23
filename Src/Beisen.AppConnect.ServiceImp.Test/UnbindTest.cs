using Beisen.Common.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.Test
{
    [TestClass]
    public class UnbindTest
    {
        [TestMethod]
        public void test()
        {
            ApplicationContext.Current.TenantId = 100002;
            ApplicationContext.Current.UserId = 112737565;

            UserInfoMappingProvider provider = new UserInfoMappingProvider();

            provider.UserUnbind("0eb854ee-7cc8-408d-a0e0-40a99114a24b,d3798936-625a-4ea0-95d8-49eb9a8aba95");
        }
    }
}

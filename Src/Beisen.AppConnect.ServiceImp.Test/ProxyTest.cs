using System;
using Beisen.AppConnect.ServiceImp.Provider.Proxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beisen.AppConnect.ServiceImp.Test
{
    [TestClass]
    public class ProxyTest
    {
        [TestMethod]
        public void TestSendEmail()
        {
            try
            {
                var batch = EmailProxy.SendEmail(0, "subject", "", "test@beisen.com", "hahahahahha");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

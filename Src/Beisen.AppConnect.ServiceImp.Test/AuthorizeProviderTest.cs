using System;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beisen.AppConnect.ServiceImp.Test
{
    [TestClass]
    public class AuthorizeProviderTest
    {
        private readonly IAuthorizeProvider _authorizeProvider = ProviderGateway.AuthorizeProvider;

        /// <summary>
        /// 测试获取授权URL
        /// </summary>
        [TestMethod]
        public void TestGetAuthorizeUrl()
        {
            try
            {
                var url = _authorizeProvider.GetAuthorizeUrl("1", "http://www.baidu.com",0, null);
                Assert.IsNotNull(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

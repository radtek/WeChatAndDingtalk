using Beisen.AppConnect.ServiceInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beisen.AppConnect.ServiceImp.Test
{
    [TestClass]
    public class TokenTest
    {
        /// <summary>
        /// 测试获取Token
        /// </summary>
        [TestMethod]
        public void TestGetToken()
        {
            var token = ProviderGateway.TokenProvider.GetToken(0, "0ABC313D-A6CC-4ABF-BAF1-369866277064");

            Assert.IsFalse(string.IsNullOrWhiteSpace(token));
        }

        /// <summary>
        /// 测试获取Token
        /// </summary>
        [TestMethod]
        public void TestGetTokenDev()
        {
            var token = ProviderGateway.TokenProvider.GetToken(0, "da312dc7-0cdc-4b79-bf45-2a731b50ee5c");

            Assert.IsFalse(string.IsNullOrWhiteSpace(token));
        }
    }
}

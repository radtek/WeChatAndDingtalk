using Beisen.AppConnect.ServiceImp.Provider;
using Beisen.AppConnect.ServiceInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beisen.AppConnect.ServiceImp.Test
{
    /// <summary>
    /// 测试请求模板
    /// </summary>
    [TestClass]
    public class ApiTemplateTest
    {
        /// <summary>
        /// 测试获取返回值
        /// </summary>
        [TestMethod]
        public void TestGetResponse()
        {
            var appAccount = ProviderGateway.AppAccountProvider.Get("1111");
            var template = new DefaultApiTemplate(appAccount, "TEST");
            var result = template.GetResponse();
        }
    }
}

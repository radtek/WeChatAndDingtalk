using Beisen.AppConnect.Infrastructure.RequestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace Beisen.AppConnect.Infrastructure.Test
{
    /// <summary>
    /// 请求测试
    /// </summary>
    [TestClass]
    public class RequestTest
    {
        /// <summary>
        /// 执行GET请求
        /// </summary>
        [TestMethod]
        public void TestSendRequest()
        {
            var url = "https://tms.beisen.com/Account/AjaxGetServerTime";
            var method = Method.GET;

            var result = Request.SendRequest(url, method);

            Assert.IsNotNull(result);
        }
    }
}

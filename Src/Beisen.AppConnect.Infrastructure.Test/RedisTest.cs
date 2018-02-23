using Beisen.AppConnect.Infrastructure.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beisen.AppConnect.Infrastructure.Test
{
    [TestClass]
    public class RedisTest
    {
        [TestMethod]
        public void TestGet()
        {
            var aaa = "aaaa";

            var bbb = "bbbb";

            var key = "test";

            RedisHelper.SetRedis(key, aaa, 600);

            var value = RedisHelper.GetRedis(key);

            RedisHelper.SetRedis(key, bbb, 600);

            var value2 = RedisHelper.GetRedis(key);

        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beisen.AppConnect.Infrastructure.Test
{
    /// <summary>
    /// 配置文件测试
    /// </summary>
    [TestClass]
    public class ConfigTest
    {
        /// <summary>
        /// 请求模板配置
        /// </summary>
        [TestMethod]
        public void TestAppConnectRequest()
        {
            var config = Beisen.AppConnect.Infrastructure.Configuration.AppConnectRequestTemplateConfig.Cache;
            Assert.IsTrue(config!=null && config.Count>0);
        }

        /// <summary>
        /// 域名配置
        /// </summary>
        [TestMethod]
        public void TestAppConnectHost()
        {
            var config = Beisen.AppConnect.Infrastructure.Configuration.AppConnectHostConfig.Cache;
            Assert.IsTrue(config != null && config.Count > 0);
        }

        /// <summary>
        /// 第三方账户类型配置
        /// </summary>
        [TestMethod]
        public void TestAppConnectAppAccountType()
        {
            var config = Beisen.AppConnect.Infrastructure.Configuration.AppConnectAppAccountTypeConfig.Cache;
            Assert.IsTrue(config != null && config.Count > 0);
        }

        /// <summary>
        /// 依赖注入配置
        /// </summary>
        [TestMethod]
        public void TestAppConnectProvider()
        {
            var config = Beisen.AppConnect.Infrastructure.Configuration.AppConnectProviderConfig.Cache;
            Assert.IsTrue(config != null && config.Count > 0);
        }

        //[TestMethod]
        //public void TestAppConnectRedirectUrl()
        //{
        //    //var config0 = Beisen.AppConnect.Infrastructure.Configuration.AppConnectRedirectUrlConfig.Instance;
        //    var config = Beisen.AppConnect.Infrastructure.Configuration.AppConnectRedirectUrlConfig.Cache;
        //    Assert.IsTrue(config != null && config.Count > 0);
        //}

        //[TestMethod]
        //public void TestAppConnectTitaApp()
        //{
        //    try
        //    {
        //        var config = Beisen.AppConnect.Infrastructure.Configuration.AppConnectTitaAppConfig.Cache;
        //        Assert.IsTrue(config != null && config.Count > 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [TestMethod]
        public void TestAppConnectCacheVersion()
        {
            try
            {
                var config = Beisen.AppConnect.Infrastructure.Configuration.AppConnectCacheVersionConfig.Cache;
                Assert.IsTrue(config != null && config.Count > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void TestAppConnectTitaAppHostMapping()
        {
            try
            {
                var appKey = Beisen.AppConnect.Infrastructure.Configuration.AppConnectTitaAppHostMappingConfig.AppKeyCache;
                var host = Beisen.AppConnect.Infrastructure.Configuration.AppConnectTitaAppHostMappingConfig.HostCache;
                Assert.IsTrue(appKey != null && appKey.Count > 0 && host != null && host.Count > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

using System;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Beisen.AppConnect.ServiceInterface.Model.WebModel;
using System.Collections.Generic;
using Beisen.DynamicScript.SDK;

namespace Beisen.AppConnect.ServiceImp.Test
{
    [TestClass]
    public class AppAccountTest
    {
        [TestMethod]
        public void test()
        {
            var provider = ESBProxy.GetInstance<IVerifyAppAccountProvider>("AppConnect");
            //provider.VerifyAppAccount(100002,"124", "2NupQIVJ1OIIgNJTk96j51TJKsXBwbk1z_pql2BgaBZA7fzSNdLtF8qM-8pNXIDh");
            //provider.EditVerifyAppAccount(100002, "", "2NupQIVJ1OIIgNJTk96j51TJKsXBwbk1z_pql2BgaBZA7fzSNdLtF8qM-8pNXIDh");
        }

        /// <summary>
        /// 测试获取帐号类型
        /// </summary>
        [TestMethod]
        public void TestGetAppAccountTypes()
        {
            var result = ProviderGateway.AppAccountProvider.GetAppAccountTypes();

            Assert.IsTrue(result != null && result.Count > 0);
        }

        /// <summary>
        /// 测试增加开放平台帐号
        /// </summary>
        [TestMethod]
        public void TestAdd()
        {
            var appAccount = new AppAccountInfo();
            appAccount.AppAccountId = Guid.NewGuid().ToString();
            appAccount.TenantId = 204233;
            appAccount.Name = "开发公共";
            appAccount.Type = 12;
            appAccount.SourceId = "111";
            appAccount.AppId = "222";
            appAccount.AppSecret = "3333";
            appAccount.State = AppAccountState.Enable;

            ProviderGateway.AppAccountProvider.Add(204233, appAccount);
        }

        /// <summary>
        /// 测试获取帐号
        /// </summary>
        [TestMethod]
        public void TestGet()
        {
            var result = ProviderGateway.AppAccountProvider.Get("da312dc7-0cdc-4b79-bf45-2a731b50ee5c");

            Assert.IsTrue(result == null);
        }

        /// <summary>
        /// 测试获取开放平台公共帐号
        /// </summary>
        [TestMethod]
        public void TesGetPublicByType()
        {
            var result = ProviderGateway.AppAccountProvider.GetPublicByType(12);

            Assert.IsTrue(result != null);
        }

        /// <summary>
        /// 测试增加开放平台帐号
        /// </summary>
        [TestMethod]
        public void TestGetMultiTenant()
        {
            var info = ProviderGateway.AppAccountProvider.GetMultiTenant(204233, "e57f6fec-5e73-415a-8ee8-abf21329c6f4");

            Assert.IsTrue(info != null);
        }
        /// <summary>
        /// Account登陸
        /// </summary>
        [TestMethod]
        public void TestLogin()
        {
            //var userName = "sunzhenyong666@bs.com";
            //var password = "111111";
            var userName = "13001081953";
            var password = "111111333";
            var tt = ProviderGateway.StaffProvider.CheckUserNameExist("112737565");
            var tt1 = ProviderGateway.StaffProvider.CheckUserNameExist("sunzhenyong666@bs.com");


            ResultModel resultModel = new ResultModel();
            var authentication = ProviderGateway.StaffProvider.Login(userName, password, out resultModel);


            var info = ProviderGateway.AppAccountProvider.GetMultiTenant(204233, "e57f6fec-5e73-415a-8ee8-abf21329c6f4");

            Assert.IsTrue(info != null);
        }


        /// <summary>
        /// Account登陸
        /// </summary>
        [TestMethod]
        public void CheckApp()
        {
            int app_id = 1;
            string redirect_url = "http://www.italent.cn/v1/sso/italent/auth?return_url=http://www.italent.cn/v1/sso/italent/auth?return_url=http%3a%2f%2fcloud.italent.cn%2fAttendance%2f%23%2findexPage%3fmetaObjName%3dAttendance.AttendanceStatistics%26app%3dAttendance";
            var result = ProviderGateway.AuthorizeProvider.CheckApp(app_id, redirect_url);

            int app_id1 = 909;
            string redirect_url1 = "http://www.italent.link";
            var result1 = ProviderGateway.AuthorizeProvider.CheckApp(app_id1, redirect_url1);
        }
        /// <summary>
        /// 获取所有消息类型
        /// </summary>
        [TestMethod]
        public void GetAllMessageType()
        {
            List<string> message = new List<string>();
            int productId = 909;
            var types = ProviderGateway.AppAccountProvider.GetAppAccountTypes();
            foreach (var item in types)
            {
                message.Add(productId + "_" + item.Key);
            }
        }

    }
}

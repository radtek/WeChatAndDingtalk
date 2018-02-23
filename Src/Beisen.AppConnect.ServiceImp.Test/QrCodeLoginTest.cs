using System;
using System.Threading;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.ServiceImp.Provider;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beisen.AppConnect.ServiceImp.Test
{
    [TestClass]
    public class QrCodeLoginTest
    {
        [TestMethod]
        public void TestAdd()
        {
            var info = new QrCodeLoginInfo();
            info.Code = Guid.NewGuid().ToString();
            info.State = QrCodeLoginState.UnLogin;
            ProviderGateway.QrCodeLoginProvider.Add(info);
        }

        [TestMethod]
        public void TestGetByCode()
        {
            var info = ProviderGateway.QrCodeLoginProvider.GetAndUpdateByCode("9B3BE695-6DE8-48DA-BAE8-5336942CEFA2");

            Thread.Sleep(3000);

            Assert.IsNotNull(info);
        }

        [TestMethod]
        public void TestUpdateState()
        {
            ProviderGateway.QrCodeLoginProvider.UpdateState("6374CCB0-B191-4BCB-85FD-976CA111D5ED",QrCodeLoginState.Scanned);
        }

        [TestMethod]
        public void TestUpdateTicket()
        {
            //ProviderGateway.QrCodeLoginProvider.UpdateIdentity("6374CCB0-B191-4BCB-85FD-976CA111D5ED", QrCodeLoginState.Login,10001,1001);
            //var a =  QrCodeLoginProvider.Instance.GenerateQrCode(100);
            var b = ProviderFactory.GetProvider<IAuthorizeProvider>();
        }
    }
}

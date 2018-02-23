using System;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beisen.AppConnect.ServiceImp.Test
{
    [TestClass]
    public class CallbackContentTest
    {
        [TestMethod]
        public void TestAdd()
        {
            var content = new CallbackContentInfo();
            content.BatchId = Guid.NewGuid().ToString();
            content.TenantId = 0;
            content.AppAccountPublic = "AppAccountPublic";
            content.AppAccountPrivate = "AppAccountPrivate";
            content.Content = "Content";
            content.State = CallbackContentState.Public;

            ProviderGateway.CallbackContentProvider.Add(content);
        }

        [TestMethod]
        public void TestGetByBatchId()
        {
            var content = ProviderGateway.CallbackContentProvider.GetByBatchId("018E8FA1-98C5-44AC-BA81-762FB5AD9887");

            Assert.IsNotNull(content);
        }

        [TestMethod]
        public void TestUpdateState()
        {
            ProviderGateway.CallbackContentProvider.UpdateState("018E8FA1-98C5-44AC-BA81-762FB5AD9887", CallbackContentState.Finish);
        }
    }
}

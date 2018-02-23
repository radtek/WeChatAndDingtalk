using Beisen.Amqp;
using Beisen.AppConnect.Infrastructure.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.Test
{
    [TestClass]
    public class RabbitMQTest
    {
        [TestMethod]
        public void TestSendRbmq()
        {
            //消费端监听消息
            ActivateServiceProvider.Instance._ActivateService();
            Thread.Sleep(9999999);
        }
    }
}

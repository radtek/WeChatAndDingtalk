using Beisen.AppConnect.ServiceImp.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.Consumers
{
    public class AccountStaffConsumer : IServiceConsumer
    {
        private readonly QueueProcess<AccountStaffEmpHandler> accountStaffEmpHandler;
        public AccountStaffConsumer()
        {
            accountStaffEmpHandler = new QueueProcess<AccountStaffEmpHandler>("TmsStaffForAppConnect");
        }

        public void ActiveService()
        {
            accountStaffEmpHandler.Start();
        }

        public void UnActiveService()
        {
            accountStaffEmpHandler.Stop();
        }
    }
}

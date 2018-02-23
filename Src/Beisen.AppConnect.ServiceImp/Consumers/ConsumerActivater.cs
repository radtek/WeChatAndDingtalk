using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beisen.AppConnect.Infrastructure.Helper;

namespace Beisen.AppConnect.ServiceImp.Consumers
{
    public class ConsumerActivater : IConsumerActivater
    {
        private static readonly IConsumerActivater _instance = new ConsumerActivater();
        public static IConsumerActivater Instance
        {
            get { return _instance; }
        }
        private ConsumerActivater(){}

        public bool ActivateServices()
        {
            return DispatcherService(true);
        }

        public bool UnActivateServices()
        {
            return DispatcherService(false);
        }

        #region Helper

        private bool DispatcherService(bool boot)
        {
            bool succeed = true;
            IConsumerPipeline instance = ConsumerPipeline.Instance;
            foreach (var consumer in instance.GetConsumer())
            {
                try
                {
                    if (boot)
                        consumer.ActiveService();
                    else
                        consumer.UnActiveService();
                }
                catch (Exception ex)
                {
                    AppConnectLogHelper.Error("appconnect 消息队列" + consumer.GetType().Name + " failed in Dispatch " + Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                    succeed = false;
                }
            }
            return succeed;
        }

        #endregion
    }
}

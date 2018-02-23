using Beisen.AppConnect.ServiceImp.Consumers;
using Beisen.AppConnect.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp
{
    public class ActivateServiceProvider : IActivateServiceProvider
    {
        #region Singleton  
        private static readonly IActivateServiceProvider _Instance = new ActivateServiceProvider();
        private static readonly IConsumerActivater consumerInstance = ConsumerActivater.Instance;
        public static IActivateServiceProvider Instance
        {
            get { return _Instance; }
        }
        #endregion

        private ActivateServiceProvider()
        {
            
        }

        public bool _ActivateService()
        {
            return consumerInstance.ActivateServices();
        }

        public bool _UnActivateService()
        {
            return consumerInstance.UnActivateServices();
        }
    }
}

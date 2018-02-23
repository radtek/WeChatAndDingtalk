using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceInterface
{
    [ServiceContract]
    public interface IActivateServiceProvider
    {
        [OperationContract(Action = "_ActivateService")]
        bool _ActivateService();

        [OperationContract(Action = "_UnActivateService")]
        bool _UnActivateService();
    }
}

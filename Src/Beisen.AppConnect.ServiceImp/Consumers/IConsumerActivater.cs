using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.Consumers
{
    public interface IConsumerActivater
    {
        bool ActivateServices();

        bool UnActivateServices();
    }
}

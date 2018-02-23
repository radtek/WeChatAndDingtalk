using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.Consumers
{
    public interface IConsumerPipeline
    {
        IEnumerable<IServiceConsumer> GetConsumer();
    }
}

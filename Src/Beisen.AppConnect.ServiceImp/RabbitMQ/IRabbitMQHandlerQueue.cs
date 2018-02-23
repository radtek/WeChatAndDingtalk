using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.RabbitMQ
{
    public interface IRabbitMQHandlerQueue
    {
        bool Post(RabbitMqHandler handler);
    }
}

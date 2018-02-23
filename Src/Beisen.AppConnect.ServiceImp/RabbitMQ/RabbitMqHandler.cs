using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.RabbitMQ
{
    public abstract class RabbitMqHandler
    {
        public RabbitMqHandlerType RabbitMqHandlerType { get; private set; }
        public RabbitMqHandler(RabbitMqHandlerType type = RabbitMqHandlerType.Normal)
        {
            RabbitMqHandlerType = type;
        }

        public abstract void Excute();
    }

    public enum RabbitMqHandlerType
    {
        Normal = 0,
        Timeline = 1,
        Computing = 2
    }
}

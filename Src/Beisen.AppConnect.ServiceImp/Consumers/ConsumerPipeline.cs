using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.Consumers
{
    public class ConsumerPipeline : IConsumerPipeline
    {
        private static readonly IConsumerPipeline _instance = new ConsumerPipeline();
        public static IConsumerPipeline Instance
        {
            get { return _instance; }
        }
        private readonly IEnumerable<IServiceConsumer> _consumer;

        private ConsumerPipeline()
        {
            //注册所有的RabbitMq的消费者
            List<IServiceConsumer> consumers = new List<IServiceConsumer>();
            consumers.Add(new AccountStaffConsumer());//同步Account的人员信息（离职，删除等修改UserId的操作）

            _consumer = consumers;
        }

        public IEnumerable<IServiceConsumer> GetConsumer()
        {
            return _consumer;
        }
    }
}

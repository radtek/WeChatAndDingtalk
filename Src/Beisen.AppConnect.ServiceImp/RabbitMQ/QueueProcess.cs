using Beisen.Amqp;
using Beisen.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.RabbitMQ
{
    public class QueueProcess<T>
       where T : IMessageHandler, new()
    {
        private static readonly LogWrapper Logger = new LogWrapper();
        private string _name;
        private string _key;
        private int _level;
        private object _queue;
        public QueueProcess(string queueName, string routKey, int level)
        {
            Run(() =>
            {
                _name = queueName;
                _key = routKey;
                _level = level;
                _queue = QueueConsumerFactory.Create<T>(_name, _key, _level);
            }, "Queue init failed");
        }

        public QueueProcess(string queueName)
        {
            Run(() =>
            {
                _name = queueName;
                //CurLog.Log("Creating Queue Consume Factory");
                _queue = QueueConsumerFactory.Create<T>(_name);
            }, "Queue init failed");
        }

        public QueueProcess(string queueName, string routKey)
        {
            Run(() =>
            {
                _name = queueName;
                _key = routKey;
                _queue = QueueConsumerFactory.Create<T>(_name, _key);
            }, "Queue init failed");
        }

        private void Run(Action action, string message)
        {
            Run(action,
                () => { },
                (e) =>
                Logger.Error(
                    new ProcessServiceAmqpException(
                        string.Format("{3}. queueName:{0},routKey:{1},Level:{2}", _name, _key, _level, message), e)));
        }

        private void Run(Action action, Action success, Action<Exception> failed)
        {
            try
            {
                action();
                success();
            }
            catch (Exception e)
            {
                failed(e);
            }
        }

        public void Start()
        {
            Run(() =>
            {
                if (_queue is QueueConsumer)
                {
                    (_queue as QueueConsumer).Start();
                }

                if (_queue is ConcurrentQueueConsumer)
                {
                    (_queue as ConcurrentQueueConsumer).Start();
                }
            }, "Queue start failed");
        }

        public void Stop()
        {
            Run(() =>
            {
                if (_queue is QueueConsumer)
                {
                    (_queue as QueueConsumer).Stop();
                }

                if (_queue is ConcurrentQueueConsumer)
                {
                    (_queue as ConcurrentQueueConsumer).Stop();
                }
            }, "Queue Stop failed");
        }
    }

    public class ProcessServiceAmqpException : ApplicationException
    {
        public ProcessServiceAmqpException()
            : base()
        {
        }

        public ProcessServiceAmqpException(String message)
            : base(message)
        {
        }

        public ProcessServiceAmqpException(String message, System.Exception innerException)
            : base(message, innerException)
        {
        }


        protected ProcessServiceAmqpException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

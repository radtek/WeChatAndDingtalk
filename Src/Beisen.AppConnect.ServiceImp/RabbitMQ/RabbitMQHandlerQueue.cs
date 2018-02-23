using Beisen.AppConnect.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Beisen.AppConnect.ServiceImp.RabbitMQ
{
    public class RabbitMQHandlerQueue : IRabbitMQHandlerQueue
    {
        const int MaxHandlerCount = 100;

        #region 处理队列
        private static  ExecutionDataflowBlockOptions _ExecutionDataflowBlockOptions = new ExecutionDataflowBlockOptions()
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount < 20 ? 20 : Environment.ProcessorCount
        };

        private static ActionBlock<RabbitMqHandler> _asyncNormalHandleQueue = new ActionBlock<RabbitMqHandler>(handler =>
        {
            try
            {
                handler.Excute();
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.Error("AppConnect Normal RabbitMQ , Handle出现异常：" + Newtonsoft.Json.JsonConvert.SerializeObject(ex));
            }
        }, _ExecutionDataflowBlockOptions);
        
        #endregion

        private bool Initialized = false;
        public RabbitMQHandlerQueue()
        {
            if (!Initialized)
            {
                Initialized = true;
            }
        }

        public bool Post(RabbitMqHandler handler)
        {
            switch (handler.RabbitMqHandlerType)
            {
                case RabbitMqHandlerType.Normal:
                default:
                    if (_asyncNormalHandleQueue.InputCount < MaxHandlerCount)
                    {
                        return _asyncNormalHandleQueue.Post(handler);
                    }
                    break;
            }
            return false;
        }

    }
}

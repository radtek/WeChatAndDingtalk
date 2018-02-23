using Beisen.AppConnect.Infrastructure.Helper;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Infrastructure.AppConnectTask
{
    public class AppConnectTask
    {
        private const int ConcurrencyLevel = 5;
        private static readonly TaskFactory factory;

        static AppConnectTask()
        {
            factory = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(ConcurrencyLevel));
        }
        private static Action GetTaskAction(Action action)
        {
            return new Action(() =>
            {
                try
                {
                    action();
                }
                catch (Exception err)
                {
                    AppConnectLogHelper.Debug("AppConnectTask action err,errormessage:" + err.Message);
                }
            });
        }
        public static Task AddTask(Action action)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            return factory.StartNew(GetTaskAction(action), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
    internal class AppConnectTaskException : ApplicationException
    {
        public AppConnectTaskException()
        {
        }

        public AppConnectTaskException(String message)
            : base(message)
        {
        }

        public AppConnectTaskException(String message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        protected AppConnectTaskException(System.Runtime.Serialization.SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

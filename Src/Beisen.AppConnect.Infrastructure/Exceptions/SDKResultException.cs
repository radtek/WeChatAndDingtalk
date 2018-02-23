using System;

namespace Beisen.AppConnect.Infrastructure.Exceptions
{
    /// <summary>
    /// SDK返回结果异常
    /// </summary>
    public class SDKResultException : ApplicationException
    {
        public SDKResultException() { }

        public SDKResultException(string message) : base(message)
        {
        }

        public SDKResultException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
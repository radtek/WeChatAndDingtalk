using System;

namespace Beisen.AppConnect.Infrastructure.Exceptions
{
    /// <summary>
    /// 执行请求异常
    /// </summary>
    public  class ExcuteRequestException : ApplicationException
    {
        public ExcuteRequestException() { }

        public ExcuteRequestException(string message) : base(message)
        {
        }

        public ExcuteRequestException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
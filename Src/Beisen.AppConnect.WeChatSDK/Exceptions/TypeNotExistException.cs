using System;

namespace Beisen.AppConnect.WeChatSDK.Exceptions
{
    public class TypeNotExistException : ApplicationException
    {
        public TypeNotExistException()
        {
        }

        public TypeNotExistException(string message) : base(message)
        {
        }

        public TypeNotExistException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Infrastructure.Exceptions
{
    public class UserOperateException : ApplicationException
    {
        public UserOperateException(string message, System.Exception exception)
            : base(message, exception)
        {

        }

        public UserOperateException(string message)
            : base(message)
        {

        }

    }
}

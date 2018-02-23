using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Infrastructure.Exceptions
{
    public class SMSSendException : ApplicationException
    {
        public SMSSendException(string message)
            : base(message)
        {

        }

        public SMSSendException(string message, Exception exception)
            : base(message, exception)
        {

        }
    }
}

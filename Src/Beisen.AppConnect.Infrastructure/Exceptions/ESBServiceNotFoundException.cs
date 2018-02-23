using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Infrastructure.Exceptions
{
    public class ESBServiceNotFoundException : ApplicationException
    {
        public ESBServiceNotFoundException(string message)
            : base(message)
        {

        }

        public ESBServiceNotFoundException(string message, Exception exception)
            : base(message, exception)
        {

        }
    }
}

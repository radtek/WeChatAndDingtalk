using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Infrastructure
{
    public class UrlTool
    {
        public static string AddQuery(string url, string query)
        {
            var index = url.IndexOf('?');
            var connector = "?";
            if (index > 0)
            {
                connector = "&";
            }
            url += connector + query;

            return url;
        }
    }
}

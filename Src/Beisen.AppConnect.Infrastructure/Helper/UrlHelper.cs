using System.Collections.Generic;

namespace Beisen.AppConnect.Infrastructure.Helper
{
    public class UrlHelper
    {
        public static string AddParameter(string url, string key, string value)
        {
            var index = url.IndexOf('?');
            var connector = "?";
            if (index > 0)
            {
                connector = "&";
            }
            url += connector + key+"=" + value;

            return url;
        }

        public static string AddParameter(string url, IDictionary<string,string> parameters)
        {
            var isFirst = true;
            foreach (var parameter in parameters)
            {
                var connector = "?";
                if (!isFirst)
                {
                    connector = "&";
                }
                else
                {
                    var index = url.IndexOf('?');
                    if (index > 0)
                    {
                        connector = "&";
                    }
                    isFirst = false;
                }
                url += connector + parameter.Key + "=" + parameter.Value;
            }
            return url;
        }

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

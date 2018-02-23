using Beisen.AppConnect.CloudClient.Configuration;
using Beisen.AppConnect.CloudClient.Models;
using Beisen.AppConnect.CloudClient.RequestUtility;
using Beisen.MultiTenant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.CloudClient
{
    public class Corp
    {
        public static List<DataSourceObject> GetCorpId()
        {
            var url = string.Format("{0}/Api/ISVApi/CorpId", AppConnectHostConfig.Cache[0]);
            // var url1 = "http://appconnect-test.beisencorp.com/Api/AppConnectISV/CorpId";

            var resquestResult = Request.SendRequest<CorpsResult>(url, RestSharp.Method.POST);

            var result = new List<DataSourceObject>();
            if (resquestResult != null)
            {
                foreach (var corp in resquestResult.CorpList)
                {
                    result.Add(new DataSourceObject(key: corp.CorpName, value: corp.CorpId));
                }
            }
            return result;
        }
    }
}

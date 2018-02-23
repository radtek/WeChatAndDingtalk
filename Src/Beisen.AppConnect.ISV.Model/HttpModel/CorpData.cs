using Beisen.MultiTenant.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Model
{
    public class CorpData
    {
        [JsonProperty("corp_list")]
        public List<CorpResult> CorpList { get; set; }
    }
    public class CorpResult
    {
        [JsonProperty("corpName")]
        public string CorpName { get; set; }

        [JsonProperty("corpId")]
        public string CorpId { get; set; }
    }
}

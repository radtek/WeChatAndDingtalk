using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.CloudClient.Models
{
    public class CorpsResult
    {
        [JsonProperty("CorpList")]
        public List<CorpResult> CorpList { get; set; }
    }

    public class CorpResult
    {
        [JsonProperty("CorpName")]
        public string CorpName { get; set; }

        [JsonProperty("CorpId")]
        public string CorpId { get; set; }
    }
}

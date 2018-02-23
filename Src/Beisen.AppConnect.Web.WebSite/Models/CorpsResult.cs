using Beisen.AppConnect.CloudClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Beisen.AppConnect.Web.WebSite.Models
{
    public class CorpsResult : ApiResult
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
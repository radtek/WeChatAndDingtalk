using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class AppAccountTypesResult:ApiResult
    {
        [JsonProperty("type_list")]
        public List<AppAccountTypeResult> TypeList { get; set; }
    }

    public class AppAccountTypeResult
    {
        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("descript")]
        public string Descript { get; set; }
    }
}

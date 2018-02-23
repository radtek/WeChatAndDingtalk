using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Api.Controller.Models
{
    class UserMapsResult : ApiResult
    {
        [JsonProperty("tenant_id")]
        public int TenantId { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("user_maps")]
        public Dictionary<int, string> UserMaps { get; set; }
    }
}

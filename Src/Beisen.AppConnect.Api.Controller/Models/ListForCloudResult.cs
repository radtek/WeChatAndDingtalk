using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class ListForCloudResult:ApiResult
    {
        [JsonProperty("list")]
        public List<ListForCloudDetail> List { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public class ListForCloudDetail
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("user_email")]
        public string UserEmail { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("create_time")]
        public string CreateTime { get; set; }

        [JsonProperty("activate_time")]
        public string ActivateTime { get; set; }

        [JsonProperty("unbind_time")]
        public string UnbindTime { get; set; }
    }
}

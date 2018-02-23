using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class ConvertToUserIdResult:ApiResult
    {
        [JsonProperty("user_list")]
        public List<ConvertToUserIdDetail> UserList { get; set; }
    }

    public class ConvertToUserIdDetail
    {

        [JsonProperty("open_id")]
        public string OpenId { get; set; }

        [JsonProperty("tenant_id")]
        public int TenantId { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }
}

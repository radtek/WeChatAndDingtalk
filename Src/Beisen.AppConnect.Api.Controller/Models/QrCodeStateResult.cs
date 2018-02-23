using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class QrCodeStateResult: ApiResult
    {
        [JsonProperty("state")]
        public int State { get; set; }

        [JsonProperty("tenant_id")]
        public int TenantId { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }
}

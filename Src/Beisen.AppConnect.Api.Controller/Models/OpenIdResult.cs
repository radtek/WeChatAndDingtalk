using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class OpenIdResult:ApiResult
    {
        [JsonProperty("open_id")]
        public string OpenId { get; set; }
    }
}

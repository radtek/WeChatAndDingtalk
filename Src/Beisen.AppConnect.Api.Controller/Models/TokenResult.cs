using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class TokenResult:ApiResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}

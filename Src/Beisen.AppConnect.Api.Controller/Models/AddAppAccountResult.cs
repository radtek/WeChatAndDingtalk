using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class AddAppAccountResult:ApiResult
    {
        [JsonProperty("appaccount_id")]
        public string AppAccountId { get; set; }
    }
}

using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class MessageSendResult:ApiResult
    {
        [JsonProperty("batch_id")]
        public string BatchId { get; set; }
    }
}

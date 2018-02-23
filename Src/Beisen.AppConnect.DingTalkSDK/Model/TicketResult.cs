using Newtonsoft.Json;

namespace Beisen.AppConnect.DingTalkSDK.Model
{
    public class TicketResult: DingTalkResult
    {
        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}

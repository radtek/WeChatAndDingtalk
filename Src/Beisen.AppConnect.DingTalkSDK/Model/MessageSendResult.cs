using Newtonsoft.Json;

namespace Beisen.AppConnect.DingTalkSDK.Model
{
    public class MessageSendResult : DingTalkResult
    {
        [JsonProperty("invaliduser")]
        public string InvalidUser { get; set; }

        [JsonProperty("invalidparty")]
        public string InvalidParty { get; set; }

        [JsonProperty("forbiddenUserId")]
        public string ForbiddenUserId { get; set; }

        [JsonProperty("messageId")]
        public string MessageId { get; set; }
    }
}

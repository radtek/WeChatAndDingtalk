using Newtonsoft.Json;

namespace Beisen.AppConnect.DingTalkSDK.Model
{
    public class TokenResult: DingTalkResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}

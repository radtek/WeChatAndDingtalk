using Newtonsoft.Json;

namespace Beisen.AppConnect.DingTalkSDK.Model
{
    public class PersistentCodeResult:DingTalkResult
    {
        [JsonProperty("openid")]
        public string OpenId { get; set; }

        [JsonProperty("persistent_code")]
        public string PersistentCode { get; set; }

        [JsonProperty("unionid")]
        public string UnionId { get; set; }
    }
}

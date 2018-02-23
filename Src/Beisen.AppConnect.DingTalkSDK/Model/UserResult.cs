using Newtonsoft.Json;

namespace Beisen.AppConnect.DingTalkSDK.Model
{
    public class UserResult:DingTalkResult
    {
        [JsonProperty("userid")]
        public string UserId { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("is_sys")]
        public bool IsSys { get; set; }

        [JsonProperty("sys_level")]
        public int SysLevel { get; set; }
    }
}

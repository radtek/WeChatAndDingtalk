using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class MenuGetResult
    {
        [JsonProperty("menu_id")]
        public string MenuId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tenant_id")]
        public int TenantId { get; set; }

        [JsonProperty("appaccount_id")]
        public string AppAccountId { get; set; }

        [JsonProperty("beisen_app_id")]
        public int BeisenAppId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}

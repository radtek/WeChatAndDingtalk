using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class MenuCreateResult:ApiResult
    {
        [JsonProperty("menu_id")]
        public string MenuId { get; set; }
    }
}

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class MenuGetListResult:ApiResult
    {
        [JsonProperty("menu_list")]
        public List<MenuGetResult> MenuList { get; set; }
    }
}

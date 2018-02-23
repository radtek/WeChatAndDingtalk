using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class AppAccountListResult : ApiResult
    {
        [JsonProperty("list")]
        public List<AppAccountListDetail> List { get; set; }
    }

    public class AppAccountListDetail
    {
        /// <summary>
        /// 多租赁Id
        /// </summary>
        [JsonProperty("appaccount_id")]
        public string AppAccountId { get; set; }

        /// <summary>
        /// 账户名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 账户类型
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore.DingDing
{
    public class UserInfo_Result : DingTalkBase
    {
        /// <summary>
        /// userid
        /// </summary>
        [JsonProperty("userid")]
        public string Userid { get; set; }
        /// <summary>
        /// deviceId
        /// </summary>
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
        /// <summary>
        /// userid
        /// </summary>
        [JsonProperty("sys_level")]
        public string Sys_level { get; set; }
        /// <summary>
        /// userid
        /// </summary>
        [JsonProperty("is_sys")]
        public string Is_sys { get; set; }
    }
}

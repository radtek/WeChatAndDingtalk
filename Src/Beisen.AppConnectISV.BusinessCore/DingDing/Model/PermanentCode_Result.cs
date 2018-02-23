using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore.DingDing
{
    public class PermanentCode_Result : DingTalkBase
    {
        /// <summary>
        /// Permanent_Code
        /// </summary>
        [JsonProperty("permanent_code")]
        public string Permanent_Code { get; set; }
        /// <summary>
        /// Ch_Permanent_Code
        /// </summary>
        [JsonProperty("ch_permanent_code")]
        public string Ch_Permanent_Code { get; set; }

        /// <summary>
        /// Ch_Permanent_Code
        /// </summary>
        [JsonProperty("auth_corp_info")]
        public Auth_Corp_Info AuthCorpInfo { get; set; }
    }
    public class Auth_Corp_Info
    {
        /// <summary>
        /// Ch_Permanent_Code
        /// </summary>
        [JsonProperty("corpid")]
        public string Corpid { get; set; }

        /// <summary>
        /// Ch_Permanent_Code
        /// </summary>
        [JsonProperty("corp_name")]
        public string Corp_name { get; set; }

    }
}

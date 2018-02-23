using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore.DingDing
{
    public class SuiteAccessToken_Post
    {
        /// <summary>
        /// Suite_Key
        /// </summary>
        [JsonProperty("suite_key")]
        public string Suite_Key { get; set; }
        /// <summary>
        /// Suite_Secret
        /// </summary>
        [JsonProperty("suite_secret")]
        public string Suite_Secret { get; set; }
        /// <summary>
        /// Suite_Ticket
        /// </summary>
        [JsonProperty("suite_ticket")]
        public string Suite_Ticket { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore.DingDing
{
    public class SuiteAccessToken_Result : DingTalkBase
    {
        /// <summary>
        /// Suite_Access_Token
        /// </summary>
        [JsonProperty("suite_access_token")]
        public string Suite_Access_Token { get; set; }

        /// <summary>
        /// Expires_in
        /// </summary>
        [JsonProperty("expires_in")]
        public int Expires_in { get; set; }
    }
}

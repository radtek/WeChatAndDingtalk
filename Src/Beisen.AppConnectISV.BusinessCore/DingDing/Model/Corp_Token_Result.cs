using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore.DingDing
{
    public class Corp_Token_Result : DingTalkBase
    {
        /// <summary>
        /// Suite_Key
        /// </summary>
        [JsonProperty("access_token")]
        public string Access_Token { get; set; }
    }
}

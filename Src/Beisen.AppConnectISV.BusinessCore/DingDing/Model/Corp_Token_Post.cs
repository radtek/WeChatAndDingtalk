using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore.DingDing
{
    public class Corp_Token_Post
    {
        /// <summary>
        /// Suite_Key
        /// </summary>
        [JsonProperty("auth_corpid")]
        public string Auth_Corpid { get; set; }
        /// <summary>
        /// Suite_Key
        /// </summary>
        [JsonProperty("permanent_code")]
        public string Permanent_Code { get; set; }
    }
}

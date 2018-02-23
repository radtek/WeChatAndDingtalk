using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore.DingDing
{
    public class PermanentCode_Post
    {
        /// <summary>
        /// Permanent_Code
        /// </summary>
        [JsonProperty("tmp_auth_code")]
        public string Tmp_auth_code { get; set; }
    }
}

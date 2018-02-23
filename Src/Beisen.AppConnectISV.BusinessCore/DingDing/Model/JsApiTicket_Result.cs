using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore.DingDing
{
    public class JsApiTicket_Result : DingTalkBase
    {
        /// <summary>
        ///ticket
        /// </summary>
        [JsonProperty("ticket")]
        public string Ticket { get; set; }
    }
}

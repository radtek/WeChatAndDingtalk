using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore.DingDing.Model
{
    public class AsyncMessageSend_Result : DingTalkBase
    {
        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("task_id")]
        public string Task_Id { get; set; }
    }
}

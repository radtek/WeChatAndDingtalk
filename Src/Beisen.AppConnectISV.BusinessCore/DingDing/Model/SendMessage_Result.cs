using Beisen.AppConnectISV.BusinessCore.DingDing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore
{
    public class SendMessage_Result : DingTalkBase
    {
        /// <summary>
        /// 无效的userid
        /// </summary>
        [JsonProperty("invaliduser")]
        public string InvalidUser { get; set; }

        /// <summary>
        /// 无效的部门id
        /// </summary>
        [JsonProperty("invalidparty")]
        public string InvalidParty { get; set; }

        /// <summary>
        /// 被流控过滤后实际未发送的userid
        /// </summary>
        [JsonProperty("forbiddenUserId")]
        public string ForbiddenUserId { get; set; }
        /// <summary>
        /// 标识企业消息的id
        /// </summary>
        [JsonProperty("messageId")]
        public string MessageId { get; set; }
    }
}

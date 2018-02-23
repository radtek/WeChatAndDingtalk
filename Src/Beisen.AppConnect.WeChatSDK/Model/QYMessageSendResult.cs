﻿using Newtonsoft.Json;

namespace Beisen.AppConnect.WeChatSDK.Model
{
    /// <summary>
    /// 企业微信发送消息结果
    /// </summary>
    public class QYMessageSendResult : WeChatResult
    {
        /// <summary>
        /// 无效UserId
        /// </summary>
        [JsonProperty("invaliduser")]
        public string InvalidUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("invalidparty")]
        public string InvalidParty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("invalidtag")]
        public string InvalidTag { get; set; }
    }
}
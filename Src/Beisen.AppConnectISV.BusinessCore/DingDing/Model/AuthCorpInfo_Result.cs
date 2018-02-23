using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore.DingDing
{
    public class AuthCorpInfo_Result : DingTalkBase
    {
        [JsonProperty("auth_corp_info")]
        public AuthCorpInfo auth_corp_info { get; set; }
        [JsonProperty("auth_user_info")]
        public AuthUserInfo auth_user_info { get; set; }
        [JsonProperty("auth_info")]
        public AuthInfo auth_info { get; set; }
    }
    public class AuthCorpInfo
    {
        [JsonProperty("corp_logo_url")]
        public string corp_logo_url { get; set; }
        [JsonProperty("corp_name")]
        public string corp_name { get; set; }
        [JsonProperty("corpid")]
        public string corpid { get; set; }
        [JsonProperty("industry")]
        public string industry { get; set; }
        [JsonProperty("invite_code")]
        public string invite_code { get; set; }
        [JsonProperty("license_code")]
        public string license_code { get; set; }
        [JsonProperty("auth_channel")]
        public string auth_channel { get; set; }
        [JsonProperty("auth_channel_type")]
        public string auth_channel_type { get; set; }
        [JsonProperty("is_authenticated")]
        public string is_authenticated { get; set; }
        [JsonProperty("auth_level")]
        public string auth_level { get; set; }
        [JsonProperty("invite_url")]
        public string invite_url { get; set; }
    }

    public class AuthUserInfo
    {
        [JsonProperty("userId")]
        public string userId { get; set; }
    }

    public class AuthInfo
    {
        [JsonProperty("agent")]
        public AgentInfo[] agent { get; set; }
    }
    public class AgentInfo
    {
        [JsonProperty("agent_name")]
        public string agent_name { get; set; }
        [JsonProperty("agentid")]
        public string agentid { get; set; }
        [JsonProperty("appid")]
        public string appid { get; set; }
        [JsonProperty("logo_url")]
        public string logo_url { get; set; }

    }
}

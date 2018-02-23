using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Model
{
    public class AccountErrorInfo
    {
        /// <summary>
        /// 错误码（0：成功，其他失败）
        /// </summary>
        [JsonProperty("errcode")]
        public int ErrCode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        [JsonProperty("errmsg")]
        public string ErrMsg { get; set; }
        /// <summary>
        /// LoginType
        /// </summary>
        [JsonProperty("authRedirectUrl")]
        public string AuthRedirectUrl { get; set; }


    }
}

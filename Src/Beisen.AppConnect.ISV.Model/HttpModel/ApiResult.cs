using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Model.HttpModel
{
    public class ApiResult
    {
        /// <summary>
        /// 错误码（0：成功，其他失败）
        /// 2001:未开通应用
        /// 2002:账户不存在
        /// 20001:查询openid数量过多
        /// 20002:用户未绑定
        /// </summary>
        [JsonProperty("errcode")]
        public int ErrCode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        [JsonProperty("errmsg")]
        public string ErrMsg { get; set; }
    }
}

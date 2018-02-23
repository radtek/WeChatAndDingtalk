using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Model
{
    public class JsApiInfo
    {
        //public JsApiInfo(string sign, string nonce, string agentId, string url, double timeStamp, string corpId)
        //{
        //    Signature = sign;
        //    Nonce = nonce;
        //    Url = url;
        //    TimeStamp = timeStamp;
        //    CorpId = corpId;
        //    AgentId = agentId;
        //}
        /// <summary>
        /// 当前请求页面url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 应用id
        /// </summary>
        public string AgentId { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public double TimeStamp { get; set; }

        /// <summary>
        /// CorpId
        /// </summary>
        public string CorpId { get; set; }

        /// <summary>
        /// 钉钉签名包
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string Nonce { get; set; }
    }
}

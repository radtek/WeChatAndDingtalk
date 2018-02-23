using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Model.HttpModel
{
    public class MessageInfoArgument
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public string productId { get; set; }
        /// <summary>
        /// 消息类型,0则全部发送
        /// </summary>
        public string messageType { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string receiverId { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public MessageContent content { get; set; }
    }
    public class MessageContent
    {
        /// <summary>
        /// 消息链接
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 消息明细
        /// </summary>
        public List<MessageContentDetail> detail { get; set; }
    }

    /// <summary>
    /// 消息明细
    /// </summary>
    public class MessageContentDetail
    {
        /// <summary>
        /// 服务后模板消息Key
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// Key的明细描述
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 明细内容
        /// </summary>
        public string value { get; set; }
    }
}

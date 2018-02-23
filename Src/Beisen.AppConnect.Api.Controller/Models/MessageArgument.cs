using System.Collections.Generic;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class MessageArgument
    {
        public string to_user { get; set; }

        public string to_openid { get; set; }

        public string appaccount_id { get; set; }

        public string tag { get; set; }

        public string template_id_short { get; set; }

        public string template_id { get; set; }

        public MessageContentArgument content { get; set; }
    }

    /// <summary>
    /// 给Tita公开的参数
    /// </summary>
    public class MessageArgumentForTita
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
        public MessageContentArgument content { get; set; }
    }

    public class MessageContentArgument
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
        public List<MessageContentDetailArgument> detail { get; set; }
    }

    /// <summary>
    /// 消息明细
    /// </summary>
    public class MessageContentDetailArgument
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

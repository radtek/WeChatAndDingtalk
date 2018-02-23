using System;

namespace Beisen.AppConnect.WeChatSDK.Enum
{
    /// <summary>
    /// 接收消息类型
    /// </summary>
    public enum RequestMsgType
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        Text = 1,

        /// <summary>
        /// 图片消息
        /// </summary>
        Image = 2,

        /// <summary>
        /// 语音消息
        /// </summary>
        Voice = 3,

        /// <summary>
        /// 视频消息
        /// </summary>
        Video = 4,

        /// <summary>
        /// 小视频消息
        /// </summary>
        ShortVideo = 5,

        /// <summary>
        /// 地理位置消息
        /// </summary>
        Location = 6,

        /// <summary>
        /// 链接消息
        /// </summary>
        Link = 7, 

        /// <summary>
        /// 事件消息
        /// </summary>
        Event = 8 
    }

    /// <summary>
    /// 接收消息类型帮助
    /// </summary>
    public class RequestMsgTypeHelper
    {
        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <param name="type">枚举字符串</param>
        /// <returns>枚举</returns>
        public static RequestMsgType GetEnum(string type)
        {
            var value = type.ToUpper();
            RequestMsgType result;
            switch (value)
            {
                case "TEXT":
                    result = RequestMsgType.Text;
                    break;
                case "IMAGE":
                    result = RequestMsgType.Image;
                    break;
                case "VOICE":
                    result = RequestMsgType.Voice;
                    break;
                case "VIDEO":
                    result = RequestMsgType.Video;
                    break;
                case "SHORTVIDEO":
                    result = RequestMsgType.ShortVideo;
                    break;
                case "LOCATION":
                    result = RequestMsgType.Location;
                    break;
                case "LINK":
                    result = RequestMsgType.Link;
                    break;
                case "EVENT":
                    result = RequestMsgType.Event;
                    break;
                default:
                    throw new ArgumentException("接收消息类型无效：type=" + type);
            }
            return result;
        }
    }
}

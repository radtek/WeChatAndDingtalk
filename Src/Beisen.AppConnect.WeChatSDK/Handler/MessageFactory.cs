using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;
using Beisen.AppConnect.WeChatSDK.Exceptions;
using Beisen.AppConnect.WeChatSDK.Handler.Request;

namespace Beisen.AppConnect.WeChatSDK.Handler
{
    /// <summary>
    /// 回调消息工厂
    /// </summary>
    public class MessageFactory
    {
        /// <summary>
        /// 获取消息实例
        /// </summary>
        /// <param name="doc">XML消息</param>
        /// <returns>消息实例</returns>
        public static RequestMsgBase GetMessage(XDocument doc)
        {
            RequestMsgBase message;

            var msgtypeStr = doc.Root.Element("MsgType").Value;
            var msgType = (RequestMsgType) System.Enum.Parse(typeof (RequestMsgType), msgtypeStr, true);
            switch (msgType)
            {
                case RequestMsgType.Text:
                    message = new RequestMsgText(doc);
                    break;
                case RequestMsgType.Image:
                    message = new RequestMsgImage(doc);
                    break;
                case RequestMsgType.Voice:
                    message = new RequestMsgVoice(doc);
                    break;
                case RequestMsgType.Video:
                    message = new RequestMsgVideo(doc);
                    break;
                case RequestMsgType.Location:
                    message = new RequestMsgLocation(doc);
                    break;
                case RequestMsgType.Link:
                    message = new RequestMsgLink(doc);
                    break;
                case RequestMsgType.Event:
                    //判断Event类型
                    var msgEventStr = doc.Root.Element("Event").Value;
                    var msgEvent =
                        (RequestMsgEventType) System.Enum.Parse(typeof (RequestMsgEventType), msgEventStr, true);
                    switch (msgEvent)
                    {
                        case RequestMsgEventType.Subscribe: //订阅（关注）
                            message = new RequestMsgEventSubscribe(doc);
                            break;
                        case RequestMsgEventType.Unsubscribe: //取消订阅（关注）
                            message = new RequestMsgEventUnsubscribe(doc);
                            break;
                        case RequestMsgEventType.Scan: //二维码扫描
                            message = new RequestMsgEventScan(doc);
                            break;
                        case RequestMsgEventType.Location: //地理位置
                            message = new RequestMsgEventLocation(doc);
                            break;
                        case RequestMsgEventType.Click: //菜单点击
                            message = new RequestMsgEventClick(doc);
                            break;
                        case RequestMsgEventType.View: //URL跳转
                            message = new RequestMsgEventView(doc);
                            break;
                        case RequestMsgEventType.TemplateSendJobFinish: //URL跳转
                            message = new RequestMsgEventTemplate(doc);
                            break;
                        default: //其他意外类型（也可以选择抛出异常）
                            throw new TypeNotExistException(string.Format("Event：{0} 在RequestMsgEventType中不存在！", msgEventStr));
                    }
                    break;
                default:
                    throw new TypeNotExistException(string.Format("MsgType：{0} 在RequestMsgType不存在！", msgtypeStr));
                        //为了能够对类型变动最大程度容错（如微信目前还可以对公众账号suscribe等未知类型，但API没有开放），建议在使用的时候catch这个异常
            }
            return message;
        }
    }
}

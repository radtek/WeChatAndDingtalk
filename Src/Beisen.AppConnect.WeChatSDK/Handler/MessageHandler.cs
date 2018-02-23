using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Beisen.AppConnect.WeChatSDK.Enum;
using Beisen.AppConnect.WeChatSDK.Handler.Request;

namespace Beisen.AppConnect.WeChatSDK.Handler
{
    /// <summary>
    /// 消息处理
    /// </summary>
    public abstract class MessageHandler
    {
        /// <summary>
        /// 请求实体
        /// </summary>
        protected RequestMsgBase RequestMessage { get; set; }

        /// <summary>
        /// 返回XML
        /// </summary>
        public string ResponseXml { get; set; }

        /// <summary>
        /// 初始化消息处理
        /// </summary>
        /// <param name="input">输入流</param>
        protected MessageHandler(Stream input)
        {
            using (XmlReader xr = XmlReader.Create(input))
            {
                var doc = XDocument.Load(xr);
                RequestMessage = MessageFactory.GetMessage(doc);
            }
        }

        /// <summary>
        /// 初始化消息处理
        /// </summary>
        /// <param name="input">输入字符串</param>
        protected MessageHandler(string input)
        {
            var doc = XDocument.Parse(input);
            Init(doc);
        }

        /// <summary>
        /// 初始化消息实例
        /// </summary>
        /// <param name="doc">XML消息</param>
        private void Init(XDocument doc)
        {
            RequestMessage = MessageFactory.GetMessage(doc);
            ResponseXml = "success";
        }

        /// <summary>
        /// 执行处理
        /// </summary>
        public void Execute()
        {
            BeforeExecute();

            //普通消息：关于重试的消息排重，推荐使用msgid排重。
            //事件： 关于重试的消息排重，推荐使用FromUserName + CreateTime 排重。
            switch (RequestMessage.MsgType)
            {
                case RequestMsgType.Text:
                    OnTextRequest(RequestMessage as RequestMsgText);
                    break;
                case RequestMsgType.Image:
                    OnImageRequest(RequestMessage as RequestMsgImage);
                    break;
                case RequestMsgType.Voice:
                    OnVoiceRequest(RequestMessage as RequestMsgVoice);
                    break;
                case RequestMsgType.Video:
                    OnVideoRequest(RequestMessage as RequestMsgVideo);
                    break;
                case RequestMsgType.ShortVideo:
                    OnShortVideoRequest(RequestMessage as RequestMsgShortVideo);
                    break;
                case RequestMsgType.Location:
                    OnLocationRequest(RequestMessage as RequestMsgLocation);
                    break;
                case RequestMsgType.Link:
                    OnLinkRequest(RequestMessage as RequestMsgLink);
                    break;
                case RequestMsgType.Event:
                    var requestMsgEvent = RequestMessage as RequestMsgEventBase;
                    switch (requestMsgEvent.Event)
                    {
                        case RequestMsgEventType.Subscribe:
                            OnEventSubscribeRequest(RequestMessage as RequestMsgEventSubscribe);
                            break;
                        case RequestMsgEventType.Unsubscribe:
                            OnEventUnsubscribeRequest(RequestMessage as RequestMsgEventUnsubscribe);
                            break;
                        case RequestMsgEventType.Scan:
                            OnEventScanRequest(RequestMessage as RequestMsgEventScan);
                            break;
                        case RequestMsgEventType.Location:
                            OnEventLocationRequest(RequestMessage as RequestMsgEventLocation);
                            break;
                        case RequestMsgEventType.Click:
                            OnEventClickRequest(RequestMessage as RequestMsgEventClick);
                            break;
                        case RequestMsgEventType.View:
                            OnEventViewRequest(RequestMessage as RequestMsgEventView);
                            break;
                        case RequestMsgEventType.TemplateSendJobFinish:
                            OnEventTemplateRequest(RequestMessage as RequestMsgEventTemplate);
                            break;
                        default:
                            throw new ArgumentException("消息事件无效：event="+ (int)(requestMsgEvent.Event));
                    }
                    break;
                default:
                    throw new ArgumentException("消息类型无效：type="+ (int)(RequestMessage.MsgType));
            }

            AfterExecute();
        }

        /// <summary>
        /// 执行处理前
        /// </summary>
        public virtual void BeforeExecute()
        {
        }

        /// <summary>
        /// 执行处理后
        /// </summary>
        public virtual void AfterExecute()
        {
        }

        /// <summary>
        /// 文本消息请求
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        protected virtual void OnTextRequest(RequestMsgText requestMessage)
        {
        }

        /// <summary>
        /// 图片消息请求
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        protected virtual void OnImageRequest(RequestMsgImage requestMessage)
        {
        }

        /// <summary>
        /// 语音消息请求
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        protected virtual void OnVoiceRequest(RequestMsgVoice requestMessage)
        {
        }

        /// <summary>
        /// 视频消息请求
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        protected virtual void OnVideoRequest(RequestMsgVideo requestMessage)
        {
        }

        /// <summary>
        /// 小视频消息请求
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        protected virtual void OnShortVideoRequest(RequestMsgShortVideo requestMessage)
        {
        }

        /// <summary>
        /// 地理位置消息请求
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        protected virtual void OnLocationRequest(RequestMsgLocation requestMessage)
        {
        }

        /// <summary>
        /// 链接消息请求
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        protected virtual void OnLinkRequest(RequestMsgLink requestMessage)
        {
        }

        /// <summary>
        /// 文本消息请求
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        protected virtual void OnEventSubscribeRequest(RequestMsgEventSubscribe requestMessage)
        {
        }

        /// <summary>
        /// 关注事件请求(扫描带参数二维码关注)
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        protected virtual void OnEventUnsubscribeRequest(RequestMsgEventUnsubscribe requestMessage)
        {
        }

        /// <summary>
        /// 扫描带参数二维码事件请求(用户已关注)
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        protected virtual void OnEventScanRequest(RequestMsgEventScan requestMessage)
        {
        }

        /// <summary>
        /// 上报地理位置事件消息请求
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        protected virtual void OnEventLocationRequest(RequestMsgEventLocation requestMessage)
        {
        }

        /// <summary>
        /// 点击菜单拉取消息时的事件推送消息请求
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        protected virtual void OnEventClickRequest(RequestMsgEventClick requestMessage)
        {
        }

        /// <summary>
        /// 点击菜单跳转链接时的事件推送消息请求
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        protected virtual void OnEventViewRequest(RequestMsgEventView requestMessage)
        {
        }

        /// <summary>
        /// 模板消息请求
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        protected virtual void OnEventTemplateRequest(RequestMsgEventTemplate requestMessage)
        {
        }
    }
}
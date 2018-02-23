using Beisen.AppConnect.WeChatSDK.Enum;

namespace Beisen.AppConnect.WeChatSDK.Handler.Response
{
    public class ResponseMsgDefault: ResponseMsgBase
    {
        public ResponseMsgDefault()
            :base(ResponseMsgType.Default)
        {
        }

        public override string ToXml()
        {
            return "success";
        }
    }
}

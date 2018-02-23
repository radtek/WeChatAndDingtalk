using Beisen.AppConnect.DingTalkSDK.Helper;
using Beisen.AppConnect.DingTalkSDK.Model;
using Beisen.AppConnect.Infrastructure.RequestUtility;

namespace Beisen.AppConnect.DingTalkSDK
{
    public static class Token
    {
        public static TokenResult GetToken(string appId, string secret)
        {
            var url = string.Format("{0}/gettoken?corpid={1}&corpsecret={2}", HostHelper.HostDingTalk, appId, secret);

            return Request.SendRequest<TokenResult>(url);
        }
    }
}

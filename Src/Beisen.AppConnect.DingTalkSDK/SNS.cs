using Beisen.AppConnect.DingTalkSDK.Helper;
using Beisen.AppConnect.DingTalkSDK.Model;
using Beisen.AppConnect.Infrastructure.RequestUtility;
using RestSharp;

namespace Beisen.AppConnect.DingTalkSDK
{
    public static class Sns
    {
        public static TokenResult GetToken(string appId, string secret)
        {
            var url = string.Format("{0}/sns/gettoken?appid={1}&appsecret={2}", HostHelper.HostDingTalk, appId, secret);

            return Request.SendRequest<TokenResult>(url);
        }

        public static PersistentCodeResult GetPersistentCode(string accessToken, string code)
        {
            var url = string.Format("{0}/sns/get_persistent_code?access_token={1}", HostHelper.HostDingTalk, accessToken);

            var json = "{\"tmp_auth_code\":\"" + code + "\"}";

            return Request.SendRequest<PersistentCodeResult>(url, Method.POST, json);
        }
    }
}

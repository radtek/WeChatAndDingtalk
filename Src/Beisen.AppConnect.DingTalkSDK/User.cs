using Beisen.AppConnect.DingTalkSDK.Helper;
using Beisen.AppConnect.DingTalkSDK.Model;
using Beisen.AppConnect.Infrastructure.RequestUtility;

namespace Beisen.AppConnect.DingTalkSDK
{
    public class User
    {
        public static UserResult GetUserInfo(string accessToken, string code)
        {
            var url = string.Format("{0}/user/getuserinfo?access_token={1}&code={2}", HostHelper.HostDingTalk, accessToken, code);

            return Request.SendRequest<UserResult>(url);
        }
    }
}

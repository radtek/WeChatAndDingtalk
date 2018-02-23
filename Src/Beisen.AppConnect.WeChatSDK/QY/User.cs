using Beisen.AppConnect.Infrastructure.RequestUtility;
using Beisen.AppConnect.WeChatSDK.Helper;
using Beisen.AppConnect.WeChatSDK.Model;

namespace Beisen.AppConnect.WeChatSDK.QY
{
    public static class User
    {
        /// <summary>
        /// 拉取用户信息(OAuth)
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="code">通过成员授权获取到的code，每次成员授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期</param>
        /// <returns></returns>
        public static OAuthQYUserInfoResult GetUserInfo(string accessToken, string code)
        {
            var url = string.Format("{0}/cgi-bin/user/getuserinfo?access_token={1}&code={2}", HostHelper.HostQY, accessToken, code);
            return Request.SendRequest<OAuthQYUserInfoResult>(url);
        }
    }
}

using Beisen.AppConnect.Infrastructure.RequestUtility;
using Beisen.AppConnect.WeChatSDK.Helper;
using Beisen.AppConnect.WeChatSDK.Model;

namespace Beisen.AppConnect.WeChatSDK.MP
{
    /// <summary>
    /// 网页授权
    /// </summary>
    public static class OAuth
    {
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="appId">公众号的唯一标识</param>
        /// <param name="secret">公众号的appsecret</param>
        /// <param name="code">code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。</param>
        /// <param name="grantType">填写为authorization_code</param>
        /// <returns></returns>
        public static OAuthAccessTokenResult GetAccessToken(string appId, string secret, string code, string grantType = "authorization_code")
        {
            var url = string.Format("{0}/sns/oauth2/access_token?appid={1}&secret={2}&code={3}&grant_type={4}", HostHelper.Host, appId, secret, code, grantType); 

            return Request.SendRequest<OAuthAccessTokenResult>(url);
        }

        /// <summary>
        /// 刷新access_token（如果需要）
        /// </summary>
        /// <param name="appId">公众号的唯一标识</param>
        /// <param name="refreshToken">填写通过access_token获取到的refresh_token参数</param>
        /// <param name="grantType">填写通过access_token获取到的refresh_token参数</param>
        /// <returns></returns>
        public static OAuthAccessTokenResult RefreshToken(string appId, string refreshToken, string grantType = "refresh_token")
        {
            var url = string.Format("{0}/sns/oauth2/refresh_token?appid={1}&grant_type={2}&refresh_token={3}", HostHelper.Host, appId, grantType, refreshToken);

            return Request.SendRequest<OAuthAccessTokenResult>(url);
        }

        /// <summary>
        /// 拉取用户信息
        /// </summary>
        /// <param name="accessToken">网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同</param>
        /// <param name="openId">用户的唯一标识</param>
        /// <param name="lang">返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语</param>
        /// <returns></returns>
        public static OAuthUserInfoResult GetUserInfo(string accessToken, string openId, string lang = "zh_CN")
        {
            var url = string.Format("{0}/sns/oauth2/userinfo?access_token={1}&Openid={2}&lang={3}", HostHelper.Host, accessToken, openId, lang);
            return Request.SendRequest<OAuthUserInfoResult>(url);
        }
    }
}
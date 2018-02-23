using Beisen.AppConnect.Infrastructure.RequestUtility;
using Beisen.AppConnect.WeChatSDK.Helper;
using Beisen.AppConnect.WeChatSDK.Model;

namespace Beisen.AppConnect.WeChatSDK.MP
{
    /// <summary>
    /// 微信凭证
    /// </summary>
    public class Token
    {
        /// <summary>
        /// 获取凭证接口
        /// </summary>
        /// <param name="grantType">获取access_token填写client_credential</param>
        /// <param name="appid">第三方用户唯一凭证</param>
        /// <param name="secret">第三方用户唯一凭证密钥，即appsecret</param>
        /// <returns></returns>
        public static TokenResult GetToken(string appid, string secret, string grantType = "client_credential")
        {
            var url = string.Format("{0}/cgi-bin/token?grant_type={1}&appid={2}&secret={3}", HostHelper.Host, grantType, appid, secret);

            return Request.SendRequest<TokenResult>(url);
        }
    }
}

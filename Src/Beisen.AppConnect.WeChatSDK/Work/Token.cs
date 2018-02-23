using Beisen.AppConnect.Infrastructure.RequestUtility;
using Beisen.AppConnect.WeChatSDK.Helper;
using Beisen.AppConnect.WeChatSDK.Model;

namespace Beisen.AppConnect.WeChatSDK.Work
{
    /// <summary>
    /// 企业凭证
    /// </summary>
    public static class Token
    {
        /// <summary>
        /// 获取凭证接口
        /// </summary>
        /// <param name="corpId">企业Id</param>
        /// <param name="corpSecret">管理组的凭证密钥</param>
        /// <returns></returns>
        public static TokenResult GetToken(string corpId, string corpSecret)
        {
            var url = string.Format("{0}/cgi-bin/gettoken?corpid={1}&corpsecret={2}", HostHelper.HostWork, corpId, corpSecret);

            return Request.SendRequest<TokenResult>(url);
        }
    }
}

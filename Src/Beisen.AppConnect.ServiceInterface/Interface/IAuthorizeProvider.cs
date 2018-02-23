using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface IAuthorizeProvider
    {
        /// <summary>
        /// 获取开放平台授权URL
        /// </summary>
        /// <param name="appAccountId"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="type">公共服务号类型。12:微信服务号，21：钉钉</param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        string GetAuthorizeUrl(string appAccountId, string redirectUrl, int type, UserCookie cookie, string loginType = null);

        /// <summary>
        /// 获取用户身份并返回回调地址
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="batch"></param>
        /// <returns></returns>

        string GetRedirectUrl(string code, string state, string batch);

        /// <summary>
        /// 校验用户身份，并返回绑定Url
        /// </summary>
        /// <param name="appAccountId"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="type"></param>
        /// <param name="cookie"></param>
        /// <returns>
        /// null：用户身份存在且状态正常
        /// 非null：返回绑定Url，直接跳转
        /// </returns>
        string GetIdentityUrl(string appAccountId, string redirectUrl, int type, UserCookie cookie, CallbackContentState state = CallbackContentState.Bind);

        /// <summary>
        /// 验证用户状态，用户接口请求验证
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        bool VerifyUser(UserCookie cookie);

        /// <summary>
        /// 校验回调域名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        bool CheckApp(int titaAppId, string url);
    }
}

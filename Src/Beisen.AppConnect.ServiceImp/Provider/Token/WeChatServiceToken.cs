using System;
using Beisen.AppConnect.Infrastructure.Exceptions;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface.Model;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    /// <summary>
    /// 微信服务号Token实例
    /// </summary>
    internal class WeChatServiceToken : TokenBase
    {
        #region 单例

        protected static readonly TokenBase _instance = new WeChatServiceToken();

        public static TokenBase Intance
        {
            get { return _instance; }
        }

        private WeChatServiceToken()
        {
        }

        #endregion

        /// <summary>
        /// 通过SDK获取Token
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        protected override TokenInfo GetTokenFromSDk(AppAccountInfo account)
        {
            var token = WeChatSDK.MP.Token.GetToken(account.AppId, account.AppSecret);
            if (string.IsNullOrWhiteSpace(token.AccessToken))
            {
                var ex = new SDKResultException(string.Format("未获取到服务号AccessToken：AppId={0},AppSecret={1},ErrCode={2},ErrMsg={3}", account.AppId, account.AppSecret, token.ErrCode, token.ErrMsg));
                AppConnectLogHelper.Error(ex);
                throw ex;
            }

            return new TokenInfo
            {
                AppId = account.AppId + account.AppSecret,
                AccessToken = token.AccessToken,
                ExpireTime = DateTime.Now.AddSeconds(token.ExpiresIn)
            };
        }

        /// <summary>
        /// 校正时间
        /// </summary>
        /// <param name="token"></param>
        protected override void AdjustExpireTime(TokenInfo token)
        {
            //服务号Token到期时间缩短10分钟，避免边界问题
            token.ExpireTime = token.ExpireTime.AddMinutes(-10);
        }
    }
}

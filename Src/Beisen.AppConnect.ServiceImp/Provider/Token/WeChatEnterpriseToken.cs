using System;
using Beisen.AppConnect.Infrastructure.Exceptions;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface.Model;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    /// <summary>
    /// 微信企业号Token实例
    /// </summary>
    internal class WeChatEnterpriseToken : TokenBase
    {
        #region 单例

        protected static readonly TokenBase _instance = new WeChatEnterpriseToken();

        public static TokenBase Intance
        {
            get { return _instance; }
        }

        private WeChatEnterpriseToken()
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
            var token = WeChatSDK.QY.Token.GetToken(account.AppId, account.AppSecret);
            if (string.IsNullOrWhiteSpace(token.AccessToken))
            {
                var ex = new SDKResultException(string.Format("未获取到企业号AccessToken：AppId={0},AppSecret={1},ErrCode={2},ErrMsg={3}", account.AppId, account.AppSecret, token.ErrCode, token.ErrMsg));
                AppConnectLogHelper.Error(ex);
                throw ex;
            }

            return new TokenInfo
            {
                AppId = account.AppId +account.AppSecret,
                AccessToken = token.AccessToken,
                ExpireTime = DateTime.Now.AddSeconds(token.ExpiresIn)
            };
        }
    }
}

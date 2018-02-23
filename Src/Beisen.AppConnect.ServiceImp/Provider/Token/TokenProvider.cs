using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.AppConnect.ServiceInterface.Model;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    /// <summary>
    /// Token
    /// </summary>
    public class TokenProvider: ITokenProvider
    {
        #region 单例

        protected static readonly ITokenProvider _instance = new TokenProvider();
        public static ITokenProvider Instance
        {
            get { return _instance; }
        }

        private TokenProvider()
        {
        }

        #endregion

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="appAccountId"></param>
        /// <param name="getNewToken"></param>
        /// <returns></returns>
        public string GetToken(int tenantId, string appAccountId, bool getNewToken = false)
        {
            var account = AppAccountProvider.Instance.Get(appAccountId);

            return GetToken(account,getNewToken);
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="account"></param>
        /// <param name="getNewToken"></param>
        /// <returns></returns>
        public string GetToken(AppAccountInfo account, bool getNewToken = false)
        {
            var tokenInstance = TokenFactory.GetTokenIntance(account);
            return tokenInstance.GetToken(account, getNewToken);
        }
    }
}

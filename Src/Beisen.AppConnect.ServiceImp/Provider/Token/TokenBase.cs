using System;
using Beisen.AppConnect.ServiceImp.Persistance;
using Beisen.AppConnect.ServiceInterface.Model;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    /// <summary>
    /// Token抽象基类
    /// </summary>
    internal abstract class TokenBase
    {
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="account"></param>
        /// <param name="getNewToken"></param>
        /// <returns></returns>
        internal string GetToken(AppAccountInfo account, bool getNewToken = false)
        {
            var tokenInfo = GetTokenFromStorage(account);
            //未存储Token或者已经过期，重新获取Token
            if (getNewToken || tokenInfo == null || tokenInfo.ExpireTime <= DateTime.Now)
            {
                tokenInfo = GetTokenFromSDk(account);
                AdjustExpireTime(tokenInfo);
                SaveToken(tokenInfo);
            }
            return tokenInfo.AccessToken;
        }

        /// <summary>
        /// 通过SDK获取Token
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        protected abstract TokenInfo GetTokenFromSDk(AppAccountInfo account);

        /// <summary>
        /// 校正过期时间
        /// </summary>
        /// <param name="token"></param>
        protected virtual void AdjustExpireTime(TokenInfo token)
        {
        }

        /// <summary>
        /// 从存储获取Token
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private static TokenInfo GetTokenFromStorage(AppAccountInfo account)
        {
            var result = TokenDao.Get(account.AppId + account.AppSecret);
            return result;
        }

        /// <summary>
        /// 保存Token
        /// </summary>
        /// <param name="token"></param>
        private static void SaveToken(TokenInfo token)
        {
            TokenDao.InsertOrUpdate(token);
        }
    }
}

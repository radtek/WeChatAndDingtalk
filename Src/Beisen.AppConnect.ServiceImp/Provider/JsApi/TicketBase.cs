using System;
using Beisen.AppConnect.ServiceImp.Persistance;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    /// <summary>
    /// Ticket抽象基类
    /// </summary>
    internal abstract class TicketBase
    {
        /// <summary>
        /// 获取Ticket
        /// </summary>
        /// <param name="account"></param>
        /// <param name="token"></param>
        /// <param name="ticketType"></param>
        /// <param name="getNewTicket"></param>
        /// <returns></returns>
        internal string GetTicket(AppAccountInfo account,string token, string ticketType= TokenType.JsApi, bool getNewTicket = false)
        {
            var tokenInfo = GetTicketFromStorage(account, ticketType);
            //未存储Ticket或者已经过期，重新获取Ticket
            if (getNewTicket || tokenInfo == null || tokenInfo.ExpireTime <= DateTime.Now)
            {
                tokenInfo = GetTicketFromSDk(account, token, ticketType);
                AdjustExpireTime(tokenInfo);
                SaveTicket(tokenInfo);
            }
            return tokenInfo.AccessToken;
        }

        /// <summary>
        /// 通过SDK获取Ticket
        /// </summary>
        /// <param name="account"></param>
        /// <param name="token"></param>
        /// <param name="ticketType"></param>
        /// <returns></returns>
        protected abstract TokenInfo GetTicketFromSDk(AppAccountInfo account, string token, string ticketType);

        /// <summary>
        /// 校正过期时间
        /// </summary>
        /// <param name="ticket"></param>
        protected virtual void AdjustExpireTime(TokenInfo ticket)
        {
        }

        /// <summary>
        /// 从存储获取Ticket
        /// </summary>
        /// <param name="account"></param>
        /// <param name="ticketType"></param>
        /// <returns></returns>
        private static TokenInfo GetTicketFromStorage(AppAccountInfo account, string ticketType)
        {
            var result = TokenDao.Get(account.AppId + account.AppSecret+ ticketType);
            return result;
        }

        /// <summary>
        /// 保存Ticket
        /// </summary>
        /// <param name="ticket"></param>
        private static void SaveTicket(TokenInfo ticket)
        {
            TokenDao.InsertOrUpdate(ticket);
        }
    }
}

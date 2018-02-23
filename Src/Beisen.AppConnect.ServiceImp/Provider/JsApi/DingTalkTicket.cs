using System;
using Beisen.AppConnect.Infrastructure.Exceptions;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.Infrastructure.Helper;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    internal class DingTalkTicket:TicketBase
    {
        #region 单例

        protected static readonly TicketBase _instance = new DingTalkTicket();

        public static TicketBase Intance
        {
            get { return _instance; }
        }

        private DingTalkTicket()
        {
        }

        #endregion

        protected override TokenInfo GetTicketFromSDk(AppAccountInfo account, string token, string ticketType)
        {
            var ticket = DingTalkSDK.JsApi.GetTicket(token);
            if (string.IsNullOrWhiteSpace(ticket.Ticket))
            {
                var ex = new SDKResultException(string.Format("未获取到钉钉Ticket：AppId={0},AppSecret={1},TicketType={2},ErrCode={3},ErrMsg={4}", account.AppId, account.AppSecret, ticketType, ticket.ErrCode, ticket.ErrMsg));
                AppConnectLogHelper.Error(ex);
                throw ex;
            }

            return new TokenInfo
            {
                AppId = account.AppId + account.AppSecret+ ticketType,
                AccessToken = ticket.Ticket,
                ExpireTime = DateTime.Now.AddSeconds(ticket.ExpiresIn)
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

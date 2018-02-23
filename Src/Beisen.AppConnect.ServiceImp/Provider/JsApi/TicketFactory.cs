using System;
using Beisen.AppConnect.ServiceInterface.Model;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    /// <summary>
    /// Ticket工厂类
    /// </summary>
    internal static class TicketFactory
    {
        /// <summary>
        /// 获取Ticket实例
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        internal static TicketBase GetTicketIntance(AppAccountInfo account)
        {
            switch (account.Type)
            {
                case 21://钉钉
                    return DingTalkTicket.Intance;
                default:
                    throw new ArgumentException("开放平台帐号类型无效[GetTicketIntance]：type=" + account.Type);
            }
        }
    }
}

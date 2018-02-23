using System;
using Beisen.AppConnect.ServiceInterface.Model;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    /// <summary>
    /// Token工厂类
    /// </summary>
    internal static class TokenFactory
    {
        /// <summary>
        /// 获取Token实例
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        internal static TokenBase GetTokenIntance(AppAccountInfo account)
        {
            switch (account.Type)
            {
                case 11://微信订阅号
                case 12://微信服务号
                    return WeChatServiceToken.Intance;
                case 13://微信企业号
                    return WeChatEnterpriseToken.Intance;
                case 14:
                    return WeChatWorkToken.Intance;
                case 21:
                    return DingTalkToken.Intance;
                default:
                    throw new ArgumentException("开放平台帐号类型无效：type=" + account.Type);
            }
        }
    }
}

using Beisen.AppConnect.ServiceInterface.Model;
using System;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    internal static class MessageFactory
    {
        internal static IMessage GetIntance(AppAccountInfo account)
        {
            switch (account.Type)
            {
                case 12://微信服务号
                    return WeChatServiceMessage.Intance;
                case 13://微信企业号
                    return WeChatEnterpriseMessage.Intance;
                case 14:
                    return WeChatWorkMessage.Intance;
                case 21:
                    return DingTalkMessage.Intance;
                default:
                    throw new ArgumentException("开放平台帐号类型无效：type=" + account.Type);
            }
        }
    }
}

using System;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    public static class CallbackStateFactory
    {
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="returnContentInfo"></param>
        /// <returns></returns>
        internal static CallbackStateBase GetIntance(CallbackContentInfo returnContentInfo)
        {
            switch (returnContentInfo.State)
            {
                case CallbackContentState.Public:
                    return CallbackPublicState.Intance;
                case CallbackContentState.Private:
                    return CallbackPrivateState.Intance;
                case CallbackContentState.PrivateAndPublic:
                    return CallbackPrivateAndPublicState.Intance;
                default:
                    throw new ArgumentException("回调状态无效：state="+(int)returnContentInfo.State);
            }
        }
    }
}

using System.Collections.Generic;
using Beisen.AppConnect.ServiceInterface.Model;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    interface IMessage
    {
        List<MessageInfo> Build(AppAccountInfo appAccount, List<int> userIds, MessageInfo message);

        MessageSendResult Send(AppAccountInfo appAccount, MessageInfo message);
    }
}

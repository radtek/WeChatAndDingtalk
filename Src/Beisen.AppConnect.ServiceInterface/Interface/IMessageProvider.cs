using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface IMessageProvider
    {
        string Send(MessageInfo message);

        int Add(MessageInfo info);

        void UpdateSendResult(int tenantId, int id, string messageId, MessageState state, string result);

        void UpdateSendResultForWeChatService(string appAccountId, int userId, string messageId, MessageState state, string result);

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        MessageInfo Get(int tenantId, int id);

        /// <summary>
        /// 重发消息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        string Resend(int tenantId, int id);
    }
}

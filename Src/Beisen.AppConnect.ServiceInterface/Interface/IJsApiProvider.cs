using Beisen.AppConnect.ServiceInterface.Model;
using AppAccountInfo = Beisen.AppConnect.ServiceInterface.Model.AppAccountInfo;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface IJsApiProvider
    {
        /// <summary>
        /// 获取Ticket
        /// </summary>
        /// <param name="appAccountId"></param>
        /// <param name="ticketType"></param>
        /// <param name="getNewToken"></param>
        /// <returns></returns>
        string GetTicket(string appAccountId, string ticketType, bool getNewToken = false);

        /// <summary>
        /// 获取Ticket
        /// </summary>
        /// <param name="account"></param>
        /// <param name="ticketType"></param>
        /// <param name="getNewToken"></param>
        /// <returns></returns>
        string GetTicket(AppAccountInfo account, string ticketType, bool getNewToken = false);

        /// <summary>
        /// 获取JSAPI配置信息
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        JsApiConfig GetJsApiConfig(string batch, string url);

    }
}

using System;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.Common.HelperObjects;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    /// <summary>
    /// Ticket
    /// </summary>
    public class JsApiProvider : IJsApiProvider
    {
        #region 单例

        protected static readonly IJsApiProvider _instance = new JsApiProvider();

        public static IJsApiProvider Instance
        {
            get { return _instance; }
        }

        private JsApiProvider()
        {
        }

        #endregion

        /// <summary>
        /// 获取Ticket
        /// </summary>
        /// <param name="appAccountId"></param>
        /// <param name="ticketType"></param>
        /// <param name="getNewToken"></param>
        /// <returns></returns>
        public string GetTicket(string appAccountId, string ticketType, bool getNewToken = false)
        {
            ArgumentHelper.AssertNotNullOrEmpty(appAccountId, "appAccountId is null or empty");
            ArgumentHelper.AssertNotNullOrEmpty(ticketType, "ticketType is null or empty");

            var account = AppAccountProvider.Instance.Get(appAccountId);

            return GetTicket(account, ticketType, getNewToken);
        }

        /// <summary>
        /// 获取Ticket
        /// </summary>
        /// <param name="account"></param>
        /// <param name="ticketType"></param>
        /// <param name="getNewToken"></param>
        /// <returns></returns>
        public string GetTicket(AppAccountInfo account, string ticketType, bool getNewToken = false)
        {
            ArgumentHelper.AssertIsTrue(account != null, "AppAccountInfo is null");
            ArgumentHelper.AssertNotNullOrEmpty(ticketType, "ticketType is null or empty");

            var token = ProviderGateway.TokenProvider.GetToken(account);
            var ticketInstance = TicketFactory.GetTicketIntance(account);
            return ticketInstance.GetTicket(account, token, ticketType, getNewToken);
        }

        /// <summary>
        /// 获取JSAPI配置信息
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public JsApiConfig GetJsApiConfig(string batch, string url)
        {
            ArgumentHelper.AssertNotNullOrEmpty(batch, "batch is null or empty");
            ArgumentHelper.AssertNotNullOrEmpty(url, "url is null or empty");

            var batchInfo = ProviderGateway.CallbackContentProvider.GetByBatchId(batch);
            var accountInfo = ProviderGateway.AppAccountProvider.Get(batchInfo.AppAccountPrivate);
            var ticket = GetTicket(accountInfo, TokenType.JsApi);
            var nonceStr = Guid.NewGuid().ToString();
            var timeStamp = DateTimeHelper.ConvertToUnixTimeStamp(DateTime.Now);
            var signature = SHA1Helper.ConverToSHA1Str(string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", ticket, nonceStr, timeStamp, url)).ToLower();

            return new JsApiConfig
            {
                Url = url,
                AgentId = accountInfo.AgentId,
                TimeStamp = timeStamp,
                AppId = accountInfo.AppId,
                Signature = signature,
                NonceStr = nonceStr
            };
        }
    }
}
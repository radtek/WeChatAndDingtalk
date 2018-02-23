using Beisen.AppConnect.DingTalkSDK.Helper;
using Beisen.AppConnect.DingTalkSDK.Model;
using Beisen.AppConnect.Infrastructure.RequestUtility;

namespace Beisen.AppConnect.DingTalkSDK
{
    public class JsApi
    {
        public static TicketResult GetTicket(string token)
        {
            var url = string.Format("{0}/get_jsapi_ticket?access_token={1}", HostHelper.HostDingTalk, token);

            return Request.SendRequest<TicketResult>(url);
        }
    }
}

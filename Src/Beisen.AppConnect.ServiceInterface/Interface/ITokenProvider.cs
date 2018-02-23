using AppAccountInfo = Beisen.AppConnect.ServiceInterface.Model.AppAccountInfo;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface ITokenProvider
    {
        string GetToken(int tenantId, string appAccountId, bool getNewToken = false);

        string GetToken(AppAccountInfo account, bool getNewToken = false);
    }
}

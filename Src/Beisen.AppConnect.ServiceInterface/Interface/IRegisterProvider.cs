using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface IRegisterProvider
    {
        bool Register(int tenantId, string mobile, int code, int codeType, string userName, string password, int inviteUserId, out string message, int registerType = 1);

        string GetBindBatchId(int tenantId, string appaccountId, int type, string redirectUrl);
    }
}

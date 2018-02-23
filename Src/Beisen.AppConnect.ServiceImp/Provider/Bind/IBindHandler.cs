using System.Collections.Generic;
using Beisen.AppConnect.ServiceInterface.Model;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    internal interface IBindHandler
    {
        BindResult Bind(Dictionary<string, string> openIds, string batch, string userName, string password, string captcha);
    }
}

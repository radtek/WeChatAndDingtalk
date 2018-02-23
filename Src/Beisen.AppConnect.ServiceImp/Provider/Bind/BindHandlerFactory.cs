using System;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    internal static class BindHandlerFactory
    {
        internal static IBindHandler GetHandler(AppUserAccountType type)
        {
            switch (type)
            {
                case AppUserAccountType.Login:
                    return LoginBindHandler.Instance;
                case AppUserAccountType.Email:
                    return EmailBindHandler.Instance;
                case AppUserAccountType.Phone:
                    return MobileBindHandler.Instance;
                default:
                    throw new ArgumentException("绑定类型无效：type=" + (int) type);
            }            
        }
    }
}

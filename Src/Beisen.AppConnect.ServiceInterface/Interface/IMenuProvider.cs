using System.Collections.Generic;
using Beisen.AppConnect.ServiceInterface.Model;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface IMenuProvider
    {
        string Create(int tenantId, MenuInfo info);

        void Update(int tenantId, MenuInfo info);

        List<MenuInfo> GetList(int tenantId);

        MenuInfo Get(string menuId);
    }
}

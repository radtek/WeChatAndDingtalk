using System;
using System.Collections.Generic;
using Beisen.AppConnect.ServiceImp.Persistance;
using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.Common.HelperObjects;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    public class MenuProvider: IMenuProvider
    {
        #region 单例

        private static readonly IMenuProvider _instance = new MenuProvider();
        public static IMenuProvider Instance
        {
            get { return _instance; }
        }

        private MenuProvider()
        {
        }

        #endregion

        public string Create(int tenantId, MenuInfo info)
        {
            ArgumentHelper.AssertIsTrue(info != null, "MenuInfo is null");
            ArgumentHelper.AssertNotNullOrEmpty(info.Name, "MenuInfo.Name is null or empty");
            ArgumentHelper.AssertIsTrue(info.TenantId > 0, "MenuInfo.TenantId is 0");
            ArgumentHelper.AssertNotNullOrEmpty(info.AppAccountId, "MenuInfo.AppAccountId is null or empty");
            ArgumentHelper.AssertIsTrue(info.BeisenAppId > 0, "MenuInfo.BeisenAppId is 0");
            ArgumentHelper.AssertNotNullOrEmpty(info.Url, "MenuInfo.Url is null or empty");

            info.MenuId = Guid.NewGuid().ToString();

            MenuDao.Insert(info);

            return info.MenuId;
        }

        public void Update(int tenantId, MenuInfo info)
        {
            ArgumentHelper.AssertIsTrue(info != null, "MenuInfo is null");
            ArgumentHelper.AssertNotNullOrEmpty(info.MenuId, "MenuInfo.MenuId is null or empty");
            ArgumentHelper.AssertNotNullOrEmpty(info.Name, "MenuInfo.Name is null or empty");
            ArgumentHelper.AssertIsTrue(info.TenantId > 0, "MenuInfo.TenantId is 0");
            ArgumentHelper.AssertNotNullOrEmpty(info.AppAccountId, "MenuInfo.AppAccountId is null or empty");
            ArgumentHelper.AssertIsTrue(info.BeisenAppId > 0, "MenuInfo.BeisenAppId is 0");
            ArgumentHelper.AssertNotNullOrEmpty(info.Url, "MenuInfo.Url is null or empty");

            MenuDao.Update(info);
        }

        public List<MenuInfo> GetList(int tenantId)
        {
            ArgumentHelper.AssertIsTrue(tenantId>0, "tenantId is 0");

            return MenuDao.GetList(tenantId);
        }

        public MenuInfo Get(string menuId)
        {
            ArgumentHelper.AssertNotNullOrEmpty(menuId, "menuId is null or empty");

            return MenuDao.Get(menuId);
        }
    }
}

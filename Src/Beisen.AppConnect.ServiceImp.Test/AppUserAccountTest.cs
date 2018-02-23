using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Constants;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.Common.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Beisen.MultiTenant.Model;
using System.Linq;
using System.Collections.Generic;
using Beisen.Data;
using System;

namespace Beisen.AppConnect.ServiceImp.Test
{
    [TestClass]
    public class AppUserAccountTest
    {
        [TestMethod]
        public void TestAdd()
        {
            var appUserAccountInfo = new AppUserAccountInfo();
            appUserAccountInfo.AppId = "dingd8afaa8e8eb609cf35c2f4657eb6378f";
            appUserAccountInfo.OpenId = "manager5176";
            appUserAccountInfo.TenantId = 100002;
            appUserAccountInfo.UserId = 112737565;
            appUserAccountInfo.BeisenAccount = "sunzhenyong666@bs.com";
            appUserAccountInfo.Type = AppUserAccountType.Login;
            appUserAccountInfo.State = AppUserAccountState.Activated;

            var id = ProviderGateway.AppUserAccountProvider.AddOrUpdate(appUserAccountInfo.TenantId, appUserAccountInfo);

            Assert.IsTrue(id > 0);
        }
        [TestMethod]
        public void Unbind()
        {
            string ids = "b7a0a373-5880-4f20-b1c6-ce246111d816";
            ProviderGateway.AppUserAccountProvider.UnBind(100002, 112737565, ids);
        }

        [TestMethod]
        public void DeletedUIBuilderComponentTest()
        {
            ApplicationContext.Current.ApplicationName = AppUserConstants.ApplicationName;
            ApplicationContext.Current.TenantId = 100002;
            ApplicationContext.Current.UserId = 112737565;
            var aa = CloudDataHelper.GetEntityAllList(AppUserConstants.MetaName, 100002).ToList();
            var oidList = CloudDataHelper.GetEntityAllList(AppUserConstants.MetaName, 100002).Select(s => s.ID.ToString()).ToArray();
            CloudDataHelper.Delete(AppUserConstants.MetaName, 100002, oidList.ToArray());

            //  var oidList = CloudDataHelper.GetEntityAllList(UPaasPortalApp.UIBuilderComponentMetaName, TenantId).ToArray();
            // CloudDataHelper.Delete(AppUserConstants.MetaName, 100002, "1cfedd2b-2035-46c3-8a30-9e126ed681c4");
        }
        #region DeletedAllTenantId
        [TestMethod]
        public void DeletedAllTenantId()
        {
            var appUserAccountInfos = GetAppUserAccountInSql();
            if (appUserAccountInfos != null && appUserAccountInfos.Count > 0)
            {
                var tenantIds = GetTenantId(appUserAccountInfos);
                if (tenantIds != null && tenantIds.Count > 0)
                {
                    foreach (var tenantId in tenantIds)
                    {
                        ApplicationContext.Current.ApplicationName = AppUserConstants.ApplicationName;
                        ApplicationContext.Current.TenantId = tenantId;
                        ApplicationContext.Current.UserId = 112737565;
                        var oidList = CloudDataHelper.GetEntityAllList(AppUserConstants.MetaName, tenantId).Select(s => s.ID.ToString()).ToArray();
                        CloudDataHelper.Delete(AppUserConstants.MetaName, tenantId, oidList.ToArray());
                    }
                }
            }
        }
        private static List<int> GetTenantId(List<AppUserAccountInfo> appUserAccountInfos)
        {
            if (appUserAccountInfos != null && appUserAccountInfos.Count > 0)
            {
                var tenantIds = appUserAccountInfos.Select(s => s.TenantId).Distinct().ToList();
                return tenantIds;
            }
            else
            {
                return null;
            }
        }
        private static List<AppUserAccountInfo> GetAppUserAccountInSql()
        {
            List<AppUserAccountInfo> appUserAccountInfos = new List<AppUserAccountInfo>();
            Database database = Database.GetDatabase("BeisenUserSecurity");
            SafeProcedure.ExecuteAndGetInstanceList(database, "[dbo].[AppUserAccount_GetAll]",
           delegate (IRecord record, int entity)
           {
               AppUserAccountInfo appUserAccountInfo = new AppUserAccountInfo();
               appUserAccountInfo.Id = record.GetOrDefault<int>("Id", 0);
               appUserAccountInfo.AppId = record.GetOrDefault<string>("AppId", "");
               appUserAccountInfo.OpenId = record.GetOrDefault<string>("OpenId", "");
               appUserAccountInfo.TenantId = record.GetOrDefault<int>("TenantId", 0);
               appUserAccountInfo.UserId = record.GetOrDefault<int>("UserId", 0);
               appUserAccountInfo.BeisenAccount = record.GetOrDefault<string>("BeisenAccount", "");
               appUserAccountInfo.TypeNotEnum = record.GetOrDefault<Int16>("Type", 0);
               appUserAccountInfo.StateNotEnum = record.GetOrDefault<Int16>("State", 0);
               appUserAccountInfo.ActivateTime = record.GetOrDefault<DateTime>("ActivateTime", DateTime.Now);
               appUserAccountInfo.CreateTime = record.GetOrDefault<DateTime>("CreateTime", DateTime.Now);
               appUserAccountInfo.UnbindTime = record.GetOrDefault<DateTime>("UnbindTime", DateTime.Now);
               appUserAccountInfo.MasterAccountId = record.GetOrDefault<int>("MasterAccountId", 0);
               appUserAccountInfos.Add(appUserAccountInfo);
           });
            return appUserAccountInfos;
        }
        #endregion

        [TestMethod]
        public void TestGetById()
        {
            var appUserAccountInfo = ProviderGateway.AppUserAccountProvider.GetById(10001, 1);

            Assert.IsNotNull(appUserAccountInfo);
        }

        [TestMethod]
        public void TestGetByOpenId()
        {
            var appUserAccountInfo = ProviderGateway.AppUserAccountProvider.GetByOpenId("wxdcfd8d067c353bed", "oeJYSwN7X6pBUinvtRnF809xhEuo-ppppppppppppppppppppppppppppppppppppppppppppppppppp");

            Assert.IsNotNull(appUserAccountInfo);
        }

        [TestMethod]
        public void TestGetByUserId()
        {
            var appUserAccountInfo = ProviderGateway.AppUserAccountProvider.GetByUserId(10001, 1001, "wxdcfd8d067c353bed");

            Assert.IsNotNull(appUserAccountInfo);
        }

        [TestMethod]
        public void TestUpdateState()
        {
            ProviderGateway.AppUserAccountProvider.UpdateState(10001, 1, AppUserAccountState.Unbind);
            var appUserAccountInfo = ProviderGateway.AppUserAccountProvider.GetById(10001, 1);

            Assert.IsTrue(appUserAccountInfo.State == AppUserAccountState.Unbind);

            ProviderGateway.AppUserAccountProvider.UpdateState(10001, 1, AppUserAccountState.Activated);
            var appUserAccountInfo2 = ProviderGateway.AppUserAccountProvider.GetById(10001, 1);

            Assert.IsTrue(appUserAccountInfo2.State == AppUserAccountState.Activated);
        }

        [TestMethod]
        public void TestGetCloudList()
        {
            int total;
            var result = ProviderGateway.AppUserAccountProvider.GetCloudList(100001, "wxdcfd8d067c353bed", 15, 0, out total);
        }
        [TestMethod]
        public void TestGetSign()
        {
            var str = ProviderGateway.AppUserAccountProvider.GetSginQueryV2("ding9458946fc068998435c2f4657eb6378f", "manager1969", 1);
        }
    }
}

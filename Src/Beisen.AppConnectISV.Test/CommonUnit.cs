using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Beisen.AppConnectISV.Infrastructure;
using Beisen.AppConnectISV.BusinessCore;
using Beisen.AppConnectISV.ServiceImp;
using Beisen.AppConnectISV.Model;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Collections.Generic;
using Beisen.MultiTenant.Model;
using Beisen.AppConnectISV.Model.BusinessEnum;
using Beisen.AppConnectISV.ServiceImp.CoreProvider;
using Beisen.AppConnectISV.ServiceImp;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnectISV.BusinessCore.DingDing;

namespace Beisen.AppConnectISV.Test
{
    [TestClass]
    public class CommonUnit : TestInitBase
    {
        /// <summary>
        /// 远程配置测试
        /// </summary>
        [TestMethod]
        public void ConfigInfo()
        {

            var appkey = AppConnectTitaAppHostMappingConfig.AppKeyCache[909];

            var dd = RedisKeyInfo.SuiteTicket;
            var dddd = Redis.Instance.GetRedis(dd);
            string mTok12en = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.Redis, AppconnectConfigConst.Redis_KeySpace);
            var str = RedisInfo.KeySpace;
            string mToken = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.ISV, AppconnectConfigConst.ISV_Token);
            string mSuiteKey = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.ISV, AppconnectConfigConst.ISV_SuiteKey);
            string mEncodingAesKey = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.ISV, AppconnectConfigConst.ISV_EncodingAesKey);
            ///////////////////////////
            var enablePermission = AppconnectConfig.Instance;//调用Instance，构造函数获取不到值
        }
        /// <summary>
        /// Redis
        /// </summary>
        [TestMethod]
        public void TestRedis()
        {
            var corpJsApiTicketKey = RedisKeyInfo.CorpJsApiTicket("dingd8afaa8e8eb609cf35c2f4657eb6378f");
            LogHelper.Instance.Dump(string.Format("DeletedCorpJsApiTicketKey:{0}", corpJsApiTicketKey), LogType.Debug);
            Redis.Instance.DelRedis(corpJsApiTicketKey);

            var redisKey = "Ticket_111";
            var redisValue = "2222";
            var ss = Redis.Instance.GetRedis("Ticket_suiteyozri0tvgrqb6iyb");
            Redis.Instance.SetRedis(redisKey, redisValue);
            var getRedisValue = Redis.Instance.GetRedis(redisKey);

            string deleteRedisKey = "suitet8op9wuow14k7113_dingb89b7c15fba4016235c2f4657eb6378f";
        }
        [TestMethod]
        public void GetRedis()
        {
            var tit = Redis.Instance.GetRedis("TmpAuthCode_suiteyozri0tvgrqb6iyb");
            var ss = Redis.Instance.GetRedis("AuthCorpInfo_suiteyozri0tvgrqb6iyb_dingd8afaa8e8eb609cf35c2f4657eb6378f");
        }
        [TestMethod]
        public void SavaUserInfo()
        {
            List<string> d = new List<string>();
            var dd = string.Join(",", d);
            AccountLoginInfo accountLoginInfo = new AccountLoginInfo();
            accountLoginInfo.TenantId = ISVInfo.ISVSystemTenantId;
            accountLoginInfo.UserId = ISVInfo.ISVSystemUserId;
            accountLoginInfo.UserType = 1;

            DingTalkUserInfo dingTalkUserInfoCookie = new DingTalkUserInfo();
            dingTalkUserInfoCookie.DingTalkUserId = "sunzhenyong";
            dingTalkUserInfoCookie.CorpId = "Sun";
            // var result = ISVLoginProvider.Instance.GetSystemUserInfo(dingTalkUserInfoCookie.CorpId, dingTalkUserInfoCookie.DingTalkUserId);
            var result = ISVLoginCoreProvider.Instance.GetSystemUserInfo("dingd8afaa8e8eb609cf35c2f4657eb6378f", "manager5176");
            var currentUserInfo = ISVLoginProvider.Instance.CurrentUserInfo(accountLoginInfo, dingTalkUserInfoCookie);
            if (currentUserInfo != null)
            {
                var status = Convert.ToInt32(currentUserInfo["Status"]);
                if (status != 1)
                {
                    Cookie.ClearDingTalkUserInfo();
                    Cookie.ClearISVLoginInfo();
                }
            }
            else
            {
                ISVLoginProvider.Instance.SaveUserInfo(accountLoginInfo);
            }
            //判断账号是否激活,未激活清楚Cookie


        }
        /// <summary>
        /// Cookie
        /// </summary>
        [TestMethod]
        public void TestCookie()
        {
            //
            // HttpContext.Current.Request.Cookies.
            //  HttpContext.Current.Items.Contains("AppAccountId")
            var loginInfo = Cookie.GetISVAuthInfo();
            loginInfo.CorpId = "eee";
            loginInfo.AppId = "ddd";
            loginInfo.LoginType = 1;
            loginInfo.BusinessRedirectUrl = "ddd";
            Cookie.SetISVAuthInfo(loginInfo);

            //LoginInfo loginInfo = new LoginInfo();
            //loginInfo.ISLogin = false;
            //loginInfo.objectId = string.Empty;
            //Cookie.SetLoginInfo(loginInfo);
            //var cookie = Cookie.GetLoginInfo();
            //
            DingTalkUserInfo dingTalkUserInfo = new DingTalkUserInfo();
            dingTalkUserInfo.DingTalkUserId = "111";
            dingTalkUserInfo.CorpId = "222";
            Cookie.SetDingTalkUserInfo(dingTalkUserInfo);

            Cookie.ClearDingTalkUserInfo();
        }

        #region 多租赁操作
        [TestMethod]
        public void MulTenant_Deleted()
        {
            Init_Test();

            //var objectId = SavaUserInfoMapping(TenantId, CurrentUserId);
            //var value = CloudData.GetEntityListForIds(AppconnectConst.UserInfoMappingMetaName, TenantId, objectId.Split(','));


            // 删除企业信息
            var oidList22 = CloudData.GetEntityAllList(AppconnectConst.CorpMetaName, ISVInfo.ISVSystemTenantId).Select(s => s.ID.ToString()).ToArray();
            CloudData.Delete(AppconnectConst.CorpMetaName, ISVInfo.ISVSystemTenantId, oidList22.ToArray());



            var oidList = CloudData.GetEntityAllList(AppconnectConst.UserInfoMappingMetaName, TenantId).Select(s => s.ID.ToString()).ToArray();
            CloudData.Delete(AppconnectConst.UserInfoMappingMetaName, TenantId, oidList.ToArray());

            var oidListAll = CloudData.GetEntityAllList(AppconnectConst.UserInfoMappingMetaName, ISVInfo.ISVSystemTenantId).Select(s => s.ID.ToString()).ToArray();
            CloudData.Delete(AppconnectConst.UserInfoMappingMetaName, ISVInfo.ISVSystemTenantId, oidListAll.ToArray());

            //删除单个数据
            string[] str = { "d3777f5d-e74d-487e-b334-4fc1c5459eda" };
            CloudData.Delete(AppconnectConst.UserInfoMappingMetaName, ISVInfo.ISVSystemTenantId, str);

        }
        [TestMethod]
        public void MulTenant_AddUserInfo()
        {
            Init_Test();
            SavaUserInfoMapping(TenantId, CurrentUserId);
        }
        private string SavaUserInfoMapping(int tenantId, int userId)
        {
            List<ObjectData> objectDatas = new List<ObjectData>();
            var metaObject = CloudData.GetMetaObject(tenantId, AppconnectConst.UserInfoMappingMetaName);
            ObjectData objectData = new ObjectData(metaObject);
            objectData.CreatedBy = userId;
            objectData.CreatedTime = DateTime.Now;
            objectData.ID = Guid.NewGuid();
            objectData.Owner = userId;
            objectData["StdIsDeleted"] = false;
            objectData["SuiteKey"] = ISVInfo.SuiteKey;
            objectData["ISVTenantId"] = tenantId;
            objectData["StaffId"] = userId;
            objectData["UserType"] = 1;
            objectData["MappingUserId"] = 111;
            objectData["CorpId"] = "dingd8afaa8e8eb609cf35c2f4657eb6378f";
            objectData["Status"] = (int)ActivateStatus.Activated;
            objectDatas.Add(objectData);
            CloudData.Add(metaObject, objectDatas);
            return objectData.ID.ToString();

        }
        #endregion

        #region 数据源测试
        [TestMethod]
        public void DataSourceTest()
        {
            //HttpResponseMessage ttt = DataSourceProvider.Instance.GetObjectListDataSource();

            var sss = UserInfoMappingProvider.Instance.GetUserInfoMapping(100002, new List<int> { 112737565, 112664957 }, "ee9334f2-fe08-4660-809a-f5093a24c650");
        }
        #endregion

        #region  V2签名
        [TestMethod]
        public void TestGetSign()
        {
            var sign = ISVLoginProvider.Instance.GetTitaSsoSignV2(100002, 112664957, 1);

            //var str = ItalentOAuthHelper.GetSginV2(100002, 112664957, 1, timeStamp, 1);
        }
        #endregion


        #region 人员信息
        [TestMethod]
        public void Staff()
        {
            Init_Test();
            var staff = Account.Instance.GetStaffNameById(100012, 112855852);
        }
        #endregion

        #region Dingtakl Method
        [TestMethod]
        public void Dingtakk()
        {
            DingDingMethod.Instance.GetAgentId("ding23efe085c9d5886f35c2f4657eb6378f", "4778");
        }
        #endregion

    }
}

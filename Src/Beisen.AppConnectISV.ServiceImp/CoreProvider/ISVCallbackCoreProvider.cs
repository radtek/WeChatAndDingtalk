using Beisen.AppConnectISV.BusinessCore.DingDing;
using Beisen.AppConnectISV.Infrastructure;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Beisen.AppConnectISV.BusinessCore;
using Beisen.MultiTenant.Model;
using Beisen.Common.Context;
using Beisen.AppConnectISV.Model;
using Beisen.SearchV3.DSL.Filters;

namespace Beisen.AppConnectISV.ServiceImp
{
    public class ISVCallbackCoreProvider
    {
        #region Singleton 
        static readonly ISVCallbackCoreProvider _Instance = new ISVCallbackCoreProvider();
        public static ISVCallbackCoreProvider Instance
        {
            get
            {
                return _Instance;
            }
        }
        static object _lock = new object();
        #endregion

        #region SaveTicket
        public void SaveTicket(string suiteTicket)
        {
            Redis.Instance.SetRedis(RedisKeyInfo.SuiteTicket, suiteTicket);
            LogHelper.Instance.Dump(string.Format("存入Redis,Key{0},Value{1}", RedisKeyInfo.SuiteTicket, suiteTicket), LogType.Debug);
        }
        #endregion

        #region Activate_Suite
        public void Activate_Suite(string redisTmpAuthCode_Value)
        {
            lock (_lock)
            {
                var suiteTmpAuthCode = Redis.Instance.GetRedis(RedisKeyInfo.SuiteTmpAuthCode);
                if (!string.IsNullOrWhiteSpace(suiteTmpAuthCode))
                {
                    if (suiteTmpAuthCode == redisTmpAuthCode_Value)
                    {
                        LogHelper.Instance.Dump(string.Format("重复的临时码"), LogType.Debug);
                        return;
                    }
                }
                Redis.Instance.SetRedis(RedisKeyInfo.SuiteTmpAuthCode, redisTmpAuthCode_Value);
                LogHelper.Instance.Dump(string.Format("存入Redis,Key{0},Value{1}", RedisKeyInfo.SuiteTmpAuthCode, redisTmpAuthCode_Value), LogType.Debug);
                //请求API
                var suite_Ticket = Redis.Instance.GetRedis(RedisKeyInfo.SuiteTicket);
                //suiteAccessToken 
                var suiteAccessToken = DingDingMethod.Instance.GetSuiteAccessToken();
                LogHelper.Instance.Dump(string.Format("Activate_Suite Method:SuiteAccessToken:{0}", suiteAccessToken), LogType.Debug);
                //永久授权码 
                var permanentCode_Result = DingDingMethod.Instance.GetPermanent_Code(redisTmpAuthCode_Value, suiteAccessToken);
                LogHelper.Instance.Dump(string.Format("Activate_Suite Method:permanentCode_Result:{0}", JsonConvert.SerializeObject(permanentCode_Result)), LogType.Debug);

                //获取企业Token  
                var corpAccessToken = DingDingMethod.Instance.GetCorpAccessToken(permanentCode_Result.AuthCorpInfo.Corpid);
                LogHelper.Instance.Dump(string.Format("Activate_Suite Method:corpAccessToken:{0}", JsonConvert.SerializeObject(corpAccessToken)), LogType.Debug);
                //激活套件
                DingDingMethod.Instance.Activate_Suite(permanentCode_Result.AuthCorpInfo.Corpid, permanentCode_Result.Permanent_Code, suiteAccessToken);

                //永久授权码存入多租赁
                LogHelper.Instance.Dump(string.Format("存入多租赁", JsonConvert.SerializeObject(permanentCode_Result)), LogType.Debug);
                SavePermanentCode(permanentCode_Result);
                LogHelper.Instance.Dump(string.Format("存入多租赁成功"), LogType.Debug);

                //获取AuthCorpInfo :  激活成功后在获取授权企业信息，不然报错
                var authCorpInfo = DingDingMethod.Instance.GetAuthCorpInfo(permanentCode_Result.AuthCorpInfo.Corpid);
            }
        }
        #endregion

        #region Suite_Relieve
        public void Suite_Relieve(string authCorpId)
        {
            var redisKey = RedisKeyInfo.CorpPermanentCode(authCorpId);
            var permanentCode_Result = Redis.Instance.GetRedis<PermanentCode_Result>(redisKey);
            if (permanentCode_Result != null)
            {
                Redis.Instance.DelRedis(redisKey);
                DeletedPermanentCode(authCorpId);
            }

            var corpJsApiTicketKey = RedisKeyInfo.CorpJsApiTicket(authCorpId);
            LogHelper.Instance.Dump(string.Format("DeletedCorpJsApiTicketKey:{0}", corpJsApiTicketKey), LogType.Debug);
            Redis.Instance.DelRedis(corpJsApiTicketKey);

            var corpTokenKey = RedisKeyInfo.CorpToken(authCorpId);
            LogHelper.Instance.Dump(string.Format("DeletedCorpToken:{0}", corpTokenKey), LogType.Debug);
            Redis.Instance.DelRedis(corpTokenKey);
            var authCorpInfo = RedisKeyInfo.AuthCorpInfo(authCorpId);
            LogHelper.Instance.Dump(string.Format("AuthCorpInfoKey:{0}", authCorpInfo), LogType.Debug);
            Redis.Instance.DelRedis(authCorpInfo);
        }
        #endregion

        #region Save Permanent_Code
        public void SavePermanentCode(PermanentCode_Result model)
        {
            var tenantId = ISVInfo.ISVSystemTenantId;
            var userId = ISVInfo.ISVSystemUserId;
            List<ObjectData> objectDatas = new List<ObjectData>();
            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = tenantId;
            ApplicationContext.Current.UserId = userId;

            var metaObject = CloudData.GetMetaObject(tenantId, AppconnectConst.CorpMetaName);
            ObjectData objectData = new ObjectData(metaObject);
            objectData.CreatedBy = userId;
            objectData.CreatedTime = DateTime.Now;
            objectData.ModifiedBy = userId;
            objectData.ModifiedTime = DateTime.Now;
            objectData.ID = Guid.NewGuid();
            objectData.Owner = userId;
            objectData["StdIsDeleted"] = false;
            objectData["SuiteKey"] = ISVInfo.SuiteKey;
            objectData["PermanentCode"] = model.Permanent_Code;
            objectData["CorpId"] = model.AuthCorpInfo.Corpid;
            objectData["CorpName"] = model.AuthCorpInfo.Corp_name;
            objectDatas.Add(objectData);
            CloudData.Add(metaObject, objectDatas);
        }
        #endregion

        #region Deleted Permanent_Code
        public void DeletedPermanentCode(string corpId)
        {
            var tenantId = ISVInfo.ISVSystemTenantId;
            var userId = ISVInfo.ISVSystemUserId;
            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = tenantId;
            ApplicationContext.Current.UserId = userId;
            var filter = new BooleanFilter()
               .Must(new TermsFilter(AppconnectConst.CorpCorpId, corpId))
               .Must(new TermsFilter(AppconnectConst.CorpSuiteKey, ISVInfo.SuiteKey));
            var data = CloudData.GetEntityAllList(AppconnectConst.CorpMetaName, tenantId, filter).FirstOrDefault();
            if (data != null)
            {
                LogHelper.Instance.Dump(string.Format("DeletedPermanentCode :{0}", JsonConvert.SerializeObject(data)), LogType.Debug);
                CloudData.LogicDeleteDataObject(AppconnectConst.CorpMetaName, tenantId, data);
            }
        }
        #endregion

    }

}

using Beisen.AppConnectISV.BusinessCore;
using Beisen.AppConnectISV.Infrastructure;
using Beisen.AppConnectISV.Model;
using Beisen.Common.Context;
using Beisen.MultiTenant.Model;
using Beisen.SearchV3.DSL.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.ServiceImp
{
    public class DataSourceProvider
    {
        #region Singleton 
        static readonly DataSourceProvider _Instance = new DataSourceProvider();
        public static DataSourceProvider Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion
        public HttpResponseMessage GetObjectListDataSource()
        {
            try
            {
                var tenantId = ISVInfo.ISVSystemTenantId;
                var userId = ISVInfo.ISVSystemUserId;

                ApplicationContext.Current.TenantId = tenantId;
                ApplicationContext.Current.UserId = userId;
                ApplicationContext.Current.ApplicationName = "AppConnect";

                var filter = new BooleanFilter()
                 .Must(new TermsFilter(AppconnectConst.CorpSuiteKey, ISVInfo.SuiteKey));
                var corps = CloudData.GetEntityAllList(AppconnectConst.CorpMetaName, ISVInfo.ISVSystemTenantId, filter).ToList();

                var corpData = new CorpData();
                if (corps.Any())
                {
                    var corpList = new List<CorpResult>();
                    foreach (var corp in corps)
                    {
                        CorpResult corpResult = new CorpResult();
                        corpResult.CorpName = Convert.ToString(corp["CorpName"]);
                        corpResult.CorpId = Convert.ToString(corp["CorpId"]);
                        corpList.Add(corpResult);
                    }
                    corpData.CorpList = corpList;
                }
                LogHelper.Instance.Dump(string.Format("GetObjectListDataSource：{0},TenantID{1},UserId{2},SuiteKey{3}", JsonConvert.SerializeObject(corpData), tenantId, userId, ISVInfo.SuiteKey), LogType.Debug);
                return Json.Instance.toJson(corpData);

            }
            catch (Exception ex)
            {
                HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent("GetObjectListDataSource请求失败!" + JsonConvert.SerializeObject(ex), Encoding.GetEncoding("UTF-8"), "application/json") };
                LogHelper.Instance.Error(DateTime.Now + ex.Message, ex);
                return result;
            }
        }
    }
}

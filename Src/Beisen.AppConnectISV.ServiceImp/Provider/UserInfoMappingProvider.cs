using Beisen.AppConnectISV.BusinessCore;
using Beisen.AppConnectISV.Infrastructure;
using Beisen.AppConnectISV.Model;
using Beisen.AppConnectISV.Model.BusinessEnum;
using Beisen.AppConnectISV.Model.BusinessModel;
using Beisen.AppConnectISV.Model.HttpModel;
using Beisen.SearchV3.DSL.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.ServiceImp
{
    public class UserInfoMappingProvider
    {
        #region Singleton 
        static readonly UserInfoMappingProvider _Instance = new UserInfoMappingProvider();
        public static UserInfoMappingProvider Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion

        public ApiResult GetUserInfoMapping(int tenantId, List<int> userIds, string appAccountId)
        {
            UserInfoResult result = new UserInfoResult();
            try
            {
                result.UserList = new List<UserInfoModel>();
                if (string.IsNullOrEmpty(appAccountId))
                {
                    result.UserList = getISVInfo(userIds);
                }
                else
                {
                    result.UserList.AddRange(getISVInfo(userIds));
                    result.UserList.AddRange(getCorpInfo(tenantId, userIds, appAccountId));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(DateTime.Now + "方法：GetUserInfoMapping" + ex.Message, ex);
            }

            return result;
        }

        public List<UserInfoModel> getISVInfo(List<int> userIds)
        {
            List<UserInfoModel> result = new List<UserInfoModel>();

            var filter = new BooleanFilter()
                    .Must(new TermsFilter(AppconnectConst.UserInfoMapping_SuiteKey, ISVInfo.SuiteKey))
                    .Must(new TermsFilter(AppconnectConst.UserInfoMapping_StaffId, userIds.ToArray()))
                    .Must(new TermFilter(AppconnectConst.UserInfoMapping_Status, (int)ActivateStatus.Activated));

            var userInfoMappingList = CloudData.GetEntityAllList(AppconnectConst.UserInfoMappingMetaName, ISVInfo.ISVSystemTenantId, filter);

            foreach (var item in userInfoMappingList)
            {
                var userInfo = new UserInfoModel
                {
                    TenantId = Convert.ToInt32(item["ISVTenantId"]),
                    StaffId = Convert.ToInt32(item["StaffId"]),
                    MappingUserId = Convert.ToString(item["MappingUserId"]),
                    Type = (int)CorpType.ISV,
                    MappingType = ((int)MappingType.DingTalk).ToString()
                };
                result.Add(userInfo);
            }

            return result;
        }

        public List<UserInfoModel> getCorpInfo(int tenantId, List<int> userIds, string appAccountId)
        {
            var appaccount = HttpRequestDao.GetAppIdByAppAccountId(appAccountId).FirstOrDefault();

            var appUserAccount = HttpRequestDao.GetListByUserId(tenantId, string.Join(",", userIds.ToArray()), appaccount.AppId);
            appUserAccount.ForEach(n => n.Type = (int)CorpType.Corp);
            appUserAccount.ForEach(n => n.MappingType = Convert.ToString(appaccount.Type));

            return appUserAccount;
        }

    }
}

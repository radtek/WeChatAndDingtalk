using System;
using System.Collections.Generic;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.BeisenUser.Model;
using Beisen.BeisenUser.ServiceImp;
using Beisen.BeisenUser.ServiceImp.Provider;

namespace Beisen.AppConnect.ServiceImp.Provider.Proxy
{
    public class BeisenUserProxy
    {
        public static string GetUserEmail(int userId)
        {
            try
            {
                return UserLiteProvider.Instance.GetUserLiteByID(userId).Email;
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.Error(string.Format("BeisenUserProxy.GetUserEmail error:userid={0},message:{1}", userId, ex.Message));
                throw;
            }
        }

        public static int GetTenantId(int userId)
        {
            try
            {
                return UserLiteProvider.Instance.GetUserLiteByID(userId).TenantID;
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.Error(string.Format("BeisenUserProxy.GetTenantId error:userid={0},message:{1}", userId, ex.Message));
                throw;
            }
        }

        public static int ValidateByEmail(string email, string password)
        {
            try
            {
                return BeisenUserGateway.UserSecurityProvider.ValidateByEmai(email, password, false);
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.Error(string.Format("BeisenUserProxy.ValidateByEmail error:email={0},message:{1}", email, ex.Message));
                throw;
            }
        }

        public static int GetSecurityByUserName(string userName)
        {
            try
            {
                return BeisenUserGateway.UserSecurityProvider.GetSecurityByUserName(userName);
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.Error(string.Format("BeisenUserProxy.GetUserEmail error:userName={0},message:{1}", userName, ex.Message));
                throw;
            }
        }

        public static List<UserLite> GetUserLiteByUserIds(int tenantId, List<int> userIds)
        {
            try
            {
                return UserLiteProvider.Instance.GetUserLiteByUserIds(tenantId, userIds);
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.Error(string.Format("BeisenUserProxy.GetUserLiteByUserIds error:tenantId={0},userid={1},message:{2}", tenantId, string.Join(",", userIds), ex.Message));
                throw;
            }
        }
    }
}

using Beisen.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Tool.Common
{
    public class BeisenUserDao
    {
        [Obsolete("这是给薪酬用的，咱不用",true)]
        public static List<int> GetAllCompensationTenantIds()
        {
            Database database = Database.GetDatabase("BeisenTenantSecurity");
            var tenantIds = new HashSet<int>();

            SafeProcedure.ExecuteAndGetInstanceList(database, "dbo.Auth_GetTenantIdsByProductID",
           delegate (IParameterSet parameters)
           {
               parameters.AddWithValue("@productID", "DC4EED15-D7D3-42B1-AC3D-CEA68A0A95B2");
           },
           delegate (IRecord record, int entity)
           {
               int dbTenantId = record.GetOrDefault<int>("TenantID", 0);
               if (dbTenantId > 0)
                   tenantIds.Add(dbTenantId);
           });

            return tenantIds.Distinct().ToList();
        }

        public static List<int> GetAllTenantIds()
        {
            Database database = Database.GetDatabase("BeisenTenantSecurity");
            var tenantIds = new HashSet<int>();

            SafeProcedure.ExecuteAndGetInstanceList(database, "dbo.Auth_GetTenantAll",
           delegate (IRecord record, int entity)
           {
               int dbTenantId = record.GetOrDefault<int>("ID", 0);
               if (dbTenantId > 0)
                   tenantIds.Add(dbTenantId);
           });

            return tenantIds.Distinct().ToList();
        }

        public static void SetContext(int tenantId)
        {
            Beisen.Common.Context.ApplicationContext.Current.ApplicationName = "AppConnect";
            Beisen.Common.Context.ApplicationContext.Current.UserId = 100000;
            Beisen.Common.Context.ApplicationContext.Current.TenantId = tenantId;
        }

        public static List<int> GetTenantIdList(string inputText, ref string message)
        {
            var result = new List<int>();
            if (string.IsNullOrWhiteSpace(inputText))
            {
                result = GetAllTenantIds();
            }
            else
            {
                try
                {
                    var tenantIdstr = inputText.Replace("，", ",").Trim();
                    result = tenantIdstr.Split(',').ToList().ConvertAll(n => Convert.ToInt32(n));
                }
                catch
                {
                    message = "tenantID格式错误，请检查后再试！";
                }
            }
            return result;
        }
    }
}

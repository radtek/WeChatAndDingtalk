using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.ESB.Client;
using Beisen.UserFramework.Models;
using Beisen.UserFramework.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    public class TenantProvider : ITenantProvider
    {
        #region 单例

        private static readonly ITenantProvider _instance = new TenantProvider();
        public static ITenantProvider Instance
        {
            get { return _instance; }
        }

        private TenantProvider()
        {
        }
        #endregion

        public string GetTenantLogo(int tenantId, string isvId = "10002")
        {
            string logoUrl = string.Empty;
            var domain = AppConnectDomainConfig.Cache;
            if (domain != null && domain.ContainsKey(isvId))
            {
                logoUrl = domain[isvId].LoginedLogo;
            }

            var provider = TenantService.Instance;

            var args = new ObjectGetByIdsArgs()
            {
                OperatorId = 10000,
                Ids = new int[] { tenantId }
            };
            var tenants = provider.GetTenantByIds(args);
            if (tenants != null && tenants.Items != null && tenants.Items.Any())
            {
                var tenantDto = tenants.Items[0];
                if (!string.IsNullOrEmpty(tenantDto.LogoUrl))
                {
                    var imgArgs = new GetImageUrlByDfsPathArgs()
                    {
                        TenantId = tenantId,
                        OperatorId = 10000,
                        DfsPath = tenantDto.LogoUrl,
                        ImageSizeTypes = new List<int>() { 7 }
                    };
                    var imgs = provider.GetImageUrlByDfsPath(imgArgs);
                    if (imgs.State != null && imgs.State.Any())
                    {
                        tenantDto.LogoUrl = imgs.State[0].LogoUrl;
                    }
                }
                return string.IsNullOrEmpty(tenantDto.LogoUrl) ? logoUrl : tenantDto.LogoUrl;
            }

            return logoUrl;
        }
    }
}

using Beisen.AppConnect.Infrastructure.Enums;
using Beisen.AppConnect.ServiceImp.Provider;
using Beisen.AppConnect.ServiceImp.RabbitMQ.Message;
using Beisen.AppConnect.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.RabbitMQ.Handler
{
    public class DimissionRecordExcuter : RabbitMqHandler
    {
        AccountStaffMessage accountStaffMessage;

        public DimissionRecordExcuter(AccountStaffMessage accountStaffMessage)
        {
            this.accountStaffMessage = accountStaffMessage;
        }

        public override void Excute()
        {
            var tenantId = accountStaffMessage.TenantId;
            var userId = accountStaffMessage.UserId;
            //实现account人员离职后 appconnect的绑定数据删除
            ProviderGateway.AppUserAccountProvider.DeleteAppUser(tenantId, userId);
            UserInfoMappingProvider.Instance.UnBindInfo(tenantId, userId);
        }
    }
}

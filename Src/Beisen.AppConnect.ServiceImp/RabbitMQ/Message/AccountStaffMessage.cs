using Beisen.AppConnect.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.RabbitMQ.Message
{
    [DataContract]
    public class AccountStaffMessage
    {
        [DataMember]
        public int TenantId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember(Name = "notityType")]
        public AccountNotifyType NotifyType { get; set; }
    }
}

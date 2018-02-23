using System;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceInterface.Model
{
    public class AppUserAccountInfo
    {
        public int Id { get; set; }

        public string AppId { get; set; }

        public string OpenId { get; set; }

        public int TenantId { get; set; }

        public int UserId { get; set; }

        public string BeisenAccount { get; set; }

        public AppUserAccountType Type { get; set; }
        public int TypeNotEnum { get; set; }

        public AppUserAccountState State { get; set; }
        public int StateNotEnum { get; set; }


        public int MasterAccountId { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }

        public DateTime? ActivateTime { get; set; }

        public DateTime? UnbindTime { get; set; }
    }
}

using System;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceInterface.Model
{
    public class AppUserForCloud
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public AppUserAccountState State { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ActivateTime { get; set; }

        public DateTime? UnbindTime { get; set; }
    }
}

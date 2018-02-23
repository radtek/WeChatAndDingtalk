using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceInterface.Model
{
    public class BindBatchInfo
    {
        public int Id { get; set; }

        public int TenantId { get; set; }


        public int AppUserAccountId { get; set; }

        public string BeisenAccount { get; set; }

        public string Batch { get; set; }


        public AppUserAccountType Type { get; set; }

        public BindBatchState State { get; set; }
    }
}

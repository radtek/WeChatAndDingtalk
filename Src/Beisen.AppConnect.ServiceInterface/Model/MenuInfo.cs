namespace Beisen.AppConnect.ServiceInterface.Model
{
    public class MenuInfo
    {
        public int Id { get; set; }

        public string MenuId { get; set; }

        public string Name { get; set; }

        public int TenantId { get; set; }

        public string AppAccountId { get; set; }

        public int BeisenAppId { get; set; }

        public string Url { get; set; }

        public int CreateBy { get; set; }

        public int ModifyBy { get; set; }

    }
}

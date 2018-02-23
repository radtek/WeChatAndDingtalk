namespace Beisen.AppConnect.Web.WebSite.Models
{
    public class QrCodeStateResult
    {
        public int State { get; set; }

        public int TenantId { get; set; }

        public int UserId { get; set; }

        public string SignQuery { get; set; }
    }
}

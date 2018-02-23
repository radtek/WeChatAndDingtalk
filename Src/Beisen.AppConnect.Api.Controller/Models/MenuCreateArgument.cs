namespace Beisen.AppConnect.Api.Controller.Models
{
    public class MenuCreateArgument
    {
        public string name { get; set; }

        public int tenant_id { get; set; }

        public string appaccount_id { get; set; }

        public string tag { get; set; }

        public int beisen_app_id { get; set; }

        public string url { get; set; }
    }
}

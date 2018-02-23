namespace Beisen.AppConnect.Api.Controller.Models
{
    public class GenerateQrCodeArgument
    {
        public string appaccount_id
        {
            get { return _AppAccountId; }
            set { _AppAccountId = value; }
        }

        public int tenant_id
        {
            get { return _TenantId; }
            set { _TenantId = value; }
        }

        public int type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        public int app_id
        {
            get { return _AppId; }
            set { _AppId = value; }
        }

        public int size
        {
            get { return _Size; }
            set { _Size = value; }
        }


        private string _AppAccountId = "";

        private int _TenantId = 0;

        private int _Type = 12;

        private int _AppId = 100;

        private int _Size = 320;
    }
}

namespace Beisen.AppConnect.ServiceInterface.Model
{
    public class BindResult
    {
        public int Result { get; set; }

        public string Message { get; set; }

        public bool NeedCaptcha { get; set; }

        public string RedirectUrl { get; set; }
    }
}

namespace Beisen.AppConnectISV.Infrastructure
{

    public static class LogHelperExtension
    {
        public static void DumpWithPage(this LogHelper helper, string message, int length, LogType logtype = LogType.Debug)
        {
            if (length <= 0 || string.IsNullOrEmpty(message)) return;
            int strLength = message.Length;
            for (int i = 0; i < strLength / length + 1; i++)
            {
                if (i == strLength / length) LogHelper.Instance.Dump(message.Substring(i * length, strLength - i * length - 1), logtype);
                else LogHelper.Instance.Dump(message.Substring(i * length, length - 1), logtype);
            }
        }
    }

}
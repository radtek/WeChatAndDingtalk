using Beisen.AppConnect.Infrastructure.Configuration;

namespace Beisen.AppConnect.DingTalkSDK.Helper
{
    internal class HostHelper
    {
        /// <summary>
        /// 获取钉钉域名
        /// </summary>
        public static string HostDingTalk = AppConnectHostConfig.Cache[21];
    }
}

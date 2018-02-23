using Beisen.AppConnect.Infrastructure.Configuration;

namespace Beisen.AppConnect.WeChatSDK.Helper
{
    /// <summary>
    /// 微信公众平台域名
    /// 开发者可以根据自己的服务器部署情况，选择最佳的接入点（延时更低，稳定性更高）。
    /// 除此之外，可以将其他接入点用作容灾用途，当网络链路发生故障时，可以考虑选择备用接入点来接入。
    /// </summary>
    internal class HostHelper
    {
        /// <summary>
        /// 获取服务号域名
        /// </summary>
        /// <returns></returns>
        public static string Host = AppConnectHostConfig.Cache[12];

        /// <summary>
        /// 获取企业号域名
        /// </summary>
        public static string HostQY = AppConnectHostConfig.Cache[13];

        /// <summary>
        /// 获取企业微信域名
        /// </summary>
        public static string HostWork = AppConnectHostConfig.Cache[14];

        /// <summary>
        /// 获取钉钉域名
        /// </summary>
        public static string HostDingTalk= AppConnectHostConfig.Cache[21];
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Beisen.Configuration;

namespace Beisen.AppConnect.Infrastructure.Configuration
{
    /// <summary>
    /// 回调地址
    /// </summary>
    [Serializable, XmlRoot("AppConnectRedirectUrl")]
    public class AppConnectRedirectUrlConfig : BaseConfig<AppConnectRedirectUrlConfig>
    {
        internal static object SyncRoot = new object();

        /// <summary>
        /// 配置文件缓存
        /// </summary>
        public static HashSet<string> Cache = new HashSet<string>();

        /// <summary>
        /// 回调地址列表
        /// </summary>
        [XmlArrayItem("Add")]
        [XmlArray("RedirectUrls")]
        public List<string> RedirectUrls { get; set; }

        /// <summary>
        /// 初始化域名配置文件
        /// </summary>
        static AppConnectRedirectUrlConfig()
        {
            LoadCache(Instance);
            ConfigChanged += RedirectUrlConfigChanged;
        }

        /// <summary>
        /// 配置文件变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void RedirectUrlConfigChanged(object sender, EventArgs e)
        {
            LoadCache(Instance);
        }

        /// <summary>
        /// 加载缓存
        /// </summary>
        /// <param name="config"></param>
        private static void LoadCache(AppConnectRedirectUrlConfig config)
        {
            if (config == null || config.RedirectUrls == null || config.RedirectUrls.Count == 0)
            {
                return;
            }
            lock (SyncRoot)
            {
                Cache.Clear();
                foreach (var url in config.RedirectUrls)
                {
                    Cache.Add(url.ToUpperInvariant());
                }
            }
        }
    }
}

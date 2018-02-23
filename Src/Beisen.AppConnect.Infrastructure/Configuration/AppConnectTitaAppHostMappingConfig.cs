using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Beisen.Configuration;

namespace Beisen.AppConnect.Infrastructure.Configuration
{
    [Serializable, XmlRoot("AppConnectTitaAppHostMapping")]
    public class AppConnectTitaAppHostMappingConfig : BaseConfig<AppConnectTitaAppHostMappingConfig>
    {
        internal static object SyncRoot = new object();

        /// <summary>
        /// 配置文件缓存
        /// </summary>
        public static Dictionary<int, string> AppKeyCache = new Dictionary<int, string>();

        /// <summary>
        /// 配置文件缓存
        /// </summary>
        public static Dictionary<int, HashSet<string>> HostCache = new Dictionary<int, HashSet<string>>();

        /// <summary>
        /// App列表
        /// </summary>
        [XmlArrayItem("App")]
        [XmlArray("TitaApps")]
        public List<AppDetail> TitaApps { get; set; }

        /// <summary>
        /// 初始化域名配置文件
        /// </summary>
        static AppConnectTitaAppHostMappingConfig()
        {
            LoadCache(Instance);
            ConfigChanged += TitaAppHostMappingConfigChanged;
        }

        /// <summary>
        /// 配置文件变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TitaAppHostMappingConfigChanged(object sender, EventArgs e)
        {
            LoadCache(Instance);
        }

        /// <summary>
        /// 加载缓存
        /// </summary>
        /// <param name="config"></param>
        private static void LoadCache(AppConnectTitaAppHostMappingConfig config)
        {
            if (config == null || config.TitaApps == null || config.TitaApps.Count == 0)
            {
                return;
            }
            lock (SyncRoot)
            {
                AppKeyCache.Clear();
                HostCache.Clear();
                foreach (var app in config.TitaApps)
                {
                    if (app.AppId > 0)
                    {
                        if (!AppKeyCache.ContainsKey(app.AppId) && (!string.IsNullOrWhiteSpace(app.AppKey) || app.AppId == 1))//兼容：tita产品appId=1且无appkey
                        {
                            AppKeyCache.Add(app.AppId, app.AppKey);
                        }
                        if (!string.IsNullOrWhiteSpace(app.Host))
                        {
                            if (!HostCache.ContainsKey(app.AppId))
                            {
                                HostCache.Add(app.AppId, new HashSet<string> {app.Host.ToUpperInvariant()});
                            }
                            else
                            {
                                HostCache[app.AppId].Add(app.Host.ToUpperInvariant());
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// App明细
        /// </summary>
        [Serializable]
        public class AppDetail
        {
            /// <summary>
            /// AppId
            /// </summary>
            [XmlAttribute]
            public int AppId { get; set; }

            /// <summary>
            /// AppKey
            /// </summary>
            [XmlAttribute]
            public string AppKey { get; set; }

            /// <summary>
            /// AppKey
            /// </summary>
            [XmlAttribute]
            public string Host { get; set; }
        }
    }
}

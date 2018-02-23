using Beisen.Configuration;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Beisen.AppConnect.Infrastructure.Configuration
{
    [Serializable, XmlRoot("AppConnectCacheVersion")]
    public class AppConnectCacheVersionConfig : BaseConfig<AppConnectCacheVersionConfig>
    {
        internal static object SyncRoot = new object();
        /// <summary>
        /// 配置文件缓存
        /// </summary>
        public static Dictionary<string, CacheVersion> Cache = new Dictionary<string, CacheVersion>();

        [XmlArrayItem("Cache")]
        [XmlArray("Caches")]
        public List<CacheVersion> Caches { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        static AppConnectCacheVersionConfig()
        {
            LoadCache(Instance);
            ConfigChanged += AppConnectCacheVersionConfigChanged;
        }

        /// <summary>
        /// 配置文件变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AppConnectCacheVersionConfigChanged(object sender, EventArgs e)
        {
            LoadCache(Instance);
        }

        /// <summary>
        /// 加载缓存
        /// </summary>
        /// <param name="config"></param>
        private static void LoadCache(AppConnectCacheVersionConfig config)
        {
            if (config == null || config.Caches == null || config.Caches.Count == 0)
            {
                return;
            }
            lock (SyncRoot)
            {
                Cache.Clear();
                foreach (var cacheItem in config.Caches)
                {
                    if (!Cache.ContainsKey(cacheItem.Name))
                    {
                        Cache.Add(cacheItem.Name, cacheItem);
                    }
                }
            }
        }

    }

    public class CacheVersion
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Version")]
        public int Version { get; set; }

        [XmlAttribute("Enable")]
        public string Enable { get; set; }
    }
}

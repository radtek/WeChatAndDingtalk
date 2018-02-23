using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Beisen.Configuration;

namespace Beisen.AppConnect.Infrastructure.Configuration
{
    /// <summary>
    /// Provider配置文件
    /// </summary>
    [Serializable, XmlRoot("AppConnectProvider")]
    public class AppConnectProviderConfig : BaseConfig<AppConnectProviderConfig>
    {
        internal static object SyncRoot = new object();

        /// <summary>
        /// 配置文件缓存
        /// </summary>
        public static Dictionary<string, AppConnectProvider> Cache = new Dictionary<string, AppConnectProvider>();

        /// <summary>
        /// Provider配置列表
        /// </summary>
        [XmlArrayItem("Add")]
        [XmlArray("Providers")]
        public List<AppConnectProvider> Providers { get; set; }

        /// <summary>
        /// 初始化Provider配置文件
        /// </summary>
        static AppConnectProviderConfig()
        {
            LoadCache(Instance);
            ConfigChanged += AppConnectProviderConfigChanged;
        }

        /// <summary>
        /// 配置文件变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AppConnectProviderConfigChanged(object sender, EventArgs e)
        {
            LoadCache(Instance);
        }

        /// <summary>
        /// 加载缓存
        /// </summary>
        /// <param name="config"></param>
        private static void LoadCache(AppConnectProviderConfig config)
        {
            if (config == null || config.Providers == null || config.Providers.Count == 0)
            {
                return;
            }
            lock (SyncRoot)
            {
                Cache.Clear();
                foreach (var provider in config.Providers)
                {
                    if (!Cache.ContainsKey(provider.Name))
                    {
                        Cache.Add(provider.Name, provider);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Provider明细
    /// </summary>
    [Serializable]
    public class AppConnectProvider
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Provider类型
        /// </summary>
        [XmlAttribute]
        public string TypeName { get; set; }

    }
}
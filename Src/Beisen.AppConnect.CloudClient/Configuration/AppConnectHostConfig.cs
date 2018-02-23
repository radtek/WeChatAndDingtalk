using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Beisen.Configuration;

namespace Beisen.AppConnect.CloudClient.Configuration
{
    /// <summary>
    /// 域名配置文件
    /// </summary>
    [Serializable, XmlRoot("AppConnectHost")]
    public class AppConnectHostConfig : BaseConfig<AppConnectHostConfig>
    {
        internal static object SyncRoot = new object();

        /// <summary>
        /// 配置文件缓存
        /// 0：Web站点
        /// 1：RestApi站点
        /// </summary>
        public static Dictionary<int, string> Cache = new Dictionary<int, string>();

        /// <summary>
        /// 域名列表
        /// </summary>
        [XmlArrayItem("Host")]
        public List<HostDetail> Hosts { get; set; }

        /// <summary>
        /// 初始化域名配置文件
        /// </summary>
        static AppConnectHostConfig()
        {
            LoadCache(Instance);
            ConfigChanged += HostConfigChanged;
        }

        /// <summary>
        /// 配置文件变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void HostConfigChanged(object sender, EventArgs e)
        {
            LoadCache(Instance);
        }

        /// <summary>
        /// 加载缓存
        /// </summary>
        /// <param name="config"></param>
        private static void LoadCache(AppConnectHostConfig config)
        {
            if (config == null || config.Hosts == null || config.Hosts.Count == 0)
            {
                return;
            }
            lock (SyncRoot)
            {
                Cache.Clear();
                foreach (var host in config.Hosts)
                {
                    if (!Cache.ContainsKey(host.Type))
                    {
                        Cache.Add(host.Type, host.Value);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 域名明细
    /// </summary>
    [Serializable]
    public class HostDetail
    {
        /// <summary>
        /// 账户类型
        /// </summary>
        [XmlAttribute]
        public int Type { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        [XmlAttribute]
        public string Value { get; set; }
    }
}

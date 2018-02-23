using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Beisen.Configuration;

namespace Beisen.AppConnect.Infrastructure.Configuration
{
    /// <summary>
    /// 第三方账户类型配置文件
    /// </summary>
    [Serializable, XmlRoot("AppConnectAppAccountType")]
    public class AppConnectAppAccountTypeConfig : BaseConfig<AppConnectAppAccountTypeConfig>
    {
        internal static object SyncRoot = new object();

        /// <summary>
        /// 配置文件缓存
        /// </summary>
        public static Dictionary<int, string> Cache = new Dictionary<int, string>();

        /// <summary>
        /// 第三方账户类型列表
        /// </summary>
        [XmlArrayItem("AppAccountType")]
        public List<TypeDetail> AppAccountTypes { get; set; }

        /// <summary>
        /// 初始化第三方账户类型配置文件
        /// </summary>
        static AppConnectAppAccountTypeConfig()
        {
            LoadCache(Instance);
            ConfigChanged += AppAccountTypeConfigChanged;
        }

        /// <summary>
        /// 配置文件变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AppAccountTypeConfigChanged(object sender, EventArgs e)
        {
            LoadCache(Instance);
        }

        /// <summary>
        /// 加载缓存
        /// </summary>
        /// <param name="config"></param>
        private static void LoadCache(AppConnectAppAccountTypeConfig config)
        {
            if (config == null || config.AppAccountTypes == null || config.AppAccountTypes.Count == 0)
            {
                return;
            }
            lock (SyncRoot)
            {
                Cache.Clear();
                foreach (var type in config.AppAccountTypes)
                {
                    if (!Cache.ContainsKey(type.Type))
                    {
                        Cache.Add(type.Type, type.Descript);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 第三方账户类型明细
    /// </summary>
    [Serializable]
    public class TypeDetail
    {
        /// <summary>
        /// 账户类型
        /// </summary>
        [XmlAttribute]
        public int Type { get; set; }

        /// <summary>
        /// 账户类型描述
        /// </summary>
        [XmlAttribute]
        public string Descript { get; set; }
    }
}

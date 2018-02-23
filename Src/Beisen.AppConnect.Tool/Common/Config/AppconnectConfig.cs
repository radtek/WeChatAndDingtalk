using Beisen.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Beisen.AppConnect.Tool.Common.Config
{
    [Serializable, XmlRoot("AppconnectConfig")]
    public class AppconnectConfig : BaseConfig<AppconnectConfig>
    {
        public static Logging.LogWrapper logger = new Logging.LogWrapper();
        private static readonly object LockThis = new object();
        /// <summary>
        /// 配置文件缓存
        /// </summary>
        private static Dictionary<string, Dictionary<string, Add>> appconnectConfigInfo = new Dictionary<string, Dictionary<string, Add>>();
        /// <summary>
        /// 根据NodeType和Key从远程配置中读取配置信息
        /// </summary>
        /// <typeparam name="T">返回值的类型</typeparam>
        /// <param name="nodeType">节点类型</param>
        /// <param name="key">节点的Key</param>
        /// <returns>节点的Value</returns>
        public static T GetAppconnectConfigInfo<T>(string nodeType, string key)
        {
            if (appconnectConfigInfo.Count <= 0 || !appconnectConfigInfo.ContainsKey(nodeType))
            {
                logger.ErrorFormat(string.Format("为获取到远程配置信息AppconnectConfig,NodeType{0}", nodeType));
                return default(T);
            }
            if (!appconnectConfigInfo[nodeType].ContainsKey(key))
            {
                logger.ErrorFormat(string.Format("为获取到远程配置信息AppconnectConfig,NodeType{0}, Key:{1}", nodeType, key));
                return default(T);
            }
            Add item = appconnectConfigInfo[nodeType][key];
            object res;
            switch (typeof(T).ToString())
            {
                case "System.String":
                    res = item.Value;
                    break;
                case "System.Int32":
                    res = int.Parse(item.Value);
                    break;
                case "System.Boolean":
                    res = Convert.ToBoolean(item.Value);
                    break;
                default:
                    res = item.Value;
                    break;
            }
            return (T)(res);
        }
        #region 远程配置初始化
        /// <summary>
        /// 初始化请求模板配置文件
        /// </summary>
        static AppconnectConfig()
        {
            LoadCache(Instance);
            ConfigChanged += RequestConfigChanged;
        }
        /// <summary>
        /// 配置文件变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void RequestConfigChanged(object sender, EventArgs e)
        {
            LoadCache(Instance);
        }
        /// <summary>
        /// 加载缓存
        /// </summary>
        /// <param name="config"></param>
        internal static void LoadCache(AppconnectConfig config)
        {
            if (config == null || config.Appconnects == null && config.Appconnects.Count == 0)
            {
                return;
            }
            lock (LockThis)
            {
                appconnectConfigInfo.Clear();
                foreach (var appSetting in config.Appconnects)
                {
                    var addCollection = new Dictionary<string, Add>();
                    foreach (var add in appSetting.Add)
                    {
                        var addDetail = new Add();
                        addDetail.Key = add.Key;
                        addDetail.Value = add.Value;
                        addDetail.Type = add.Type;
                        addCollection.Add(add.Key, addDetail);
                    }
                    appconnectConfigInfo.Add(appSetting.Type, addCollection);
                }
            }
        }
        #endregion

        #region XML Informatica
        /// <summary>
        ///根据XML标签AppSetting获取远程配置AppSetting集合
        /// </summary>
        [XmlArrayItem("Appconnect")]
        public List<Appconnect> Appconnects;
        /// <summary>
        /// AppSetting节点
        /// </summary>
        [Serializable]
        public class Appconnect
        {
            [XmlAttribute]
            public string Type { get; set; }
            [XmlElement]
            public List<Add> Add { get; set; }
        }
        /// <summary>
        /// Add节点
        /// </summary>
        [Serializable]
        public class Add
        {
            [XmlAttribute("Key")]
            public string Key { get; set; }

            [XmlAttribute("Type")]
            public string Type { get; set; }

            [XmlAttribute("Value")]
            public string Value { get; set; }
        }
        #endregion
    }
}

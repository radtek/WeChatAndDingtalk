using Beisen.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Beisen.AppConnect.Infrastructure.Configuration
{
    [Serializable, XmlRoot("ISVSettingsConfig")]
    public class AppConnectISVSettingsConfig : BaseConfig<AppConnectISVSettingsConfig>
    {
        internal static object SyncRoot = new object();

        /// <summary>
        /// 配置文件缓存
        /// </summary>
        public static Dictionary<string, ISVSetting> Cache = new Dictionary<string, ISVSetting>();

        private ISVSettings _ISVSettings = new ISVSettings();
        [XmlArray("ISVSettings")]
        [XmlArrayItem("ISVSetting")]
        public ISVSettings ISVSettings
        {
            get { return _ISVSettings; }
            set { _ISVSettings = value; }
        }

        /// <summary>
        /// 初始化Provider配置文件
        /// </summary>
        static AppConnectISVSettingsConfig()
        {
            LoadCache(Instance);
            ConfigChanged += AppConnectISVSettingsConfigChanged;
        }

        /// <summary>
        /// 配置文件变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AppConnectISVSettingsConfigChanged(object sender, EventArgs e)
        {
            LoadCache(Instance);
        }

        /// <summary>
        /// 加载缓存
        /// </summary>
        /// <param name="config"></param>
        private static void LoadCache(AppConnectISVSettingsConfig config)
        {
            if (config == null || config.ISVSettings == null || config.ISVSettings.Count == 0)
            {
                return;
            }
            lock (SyncRoot)
            {
                Cache.Clear();
                foreach (var isvSetting in config.ISVSettings)
                {
                    if (!Cache.ContainsKey(isvSetting.Id))
                    {
                        Cache.Add(isvSetting.Id, isvSetting);
                    }
                }
            }
        }
    }

    [Serializable]
    public class ISVSettings : KeyedCollection<string, ISVSetting>
    {
        protected override string GetKeyForItem(ISVSetting item)
        {
            return item.Id;
        }
    }

    [Serializable]
    public class ISVSetting
    {
        /// <summary>
        /// ISV标识
        /// </summary>
        [XmlAttribute("Id")]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 短信通道
        /// </summary>
        [XmlAttribute("SMSChannelId")]
        public string SMSChannelId { get; set; }


        [XmlArray("SMSTemplates")]
        [XmlArrayItem("SMSTemplate")]
        public SMSTemplates SMSTemplates { get; set; }
    }

    [Serializable]
    public class SMSTemplates : KeyedCollection<string, ISVSMSTemplate>
    {
        protected override string GetKeyForItem(ISVSMSTemplate item)
        {
            return item.Type;
        }
    }

    [Serializable]
    public class ISVSMSTemplate
    {
        [XmlAttribute("Type")]
        public string Type { get; set; }

        [XmlAttribute("Content")]
        public string Content { get; set; }

        [XmlAttribute("Message")]
        public string Message { get; set; }

        /// <summary>
        /// 短信通道
        /// </summary>
        [XmlAttribute("SMSChannelId")]
        public string SMSChannelId { get; set; }

    }
}

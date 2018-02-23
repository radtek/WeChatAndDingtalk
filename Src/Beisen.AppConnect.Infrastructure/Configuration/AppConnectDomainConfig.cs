using Beisen.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Beisen.AppConnect.Infrastructure.Configuration
{
    [Serializable, XmlRoot("TitaDomainConfig")]
    public class AppConnectDomainConfig : BaseConfig<AppConnectDomainConfig>
    {
        internal static object SyncRoot = new object();

        /// <summary>
        /// 配置文件缓存
        /// </summary>
        public static Dictionary<string, DomainConfig> Cache = new Dictionary<string, DomainConfig>();

        [XmlArrayItem("Domain")]
        [XmlArray("Domains")]
        public List<DomainConfig> Domains { get; set; }

        /// <summary>
        /// 初始化Provider配置文件
        /// </summary>
        static AppConnectDomainConfig()
        {
            LoadCache(Instance);
            ConfigChanged += AppConnectDomainConfigChanged;
        }

        /// <summary>
        /// 配置文件变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AppConnectDomainConfigChanged(object sender, EventArgs e)
        {
            LoadCache(Instance);
        }

        /// <summary>
        /// 加载缓存
        /// </summary>
        /// <param name="config"></param>
        private static void LoadCache(AppConnectDomainConfig config)
        {
            if (config == null || config.Domains == null || config.Domains.Count == 0)
            {
                return;
            }
            lock (SyncRoot)
            {
                Cache.Clear();
                foreach (var domain in config.Domains)
                {
                    if (!Cache.ContainsKey(domain.ISVId.ToString()))
                    {
                        Cache.Add(domain.ISVId.ToString(), domain);
                    }
                }
            }
        }
    }

    public class DomainConfig
    {
        [XmlAttribute("ISVId")]
        public int ISVId { get; set; }

        [XmlAttribute("Name")]
        public string Domain { get; set; }

        [XmlAttribute("TopName")]
        public string TopName { get; set; }

        [XmlAttribute("Back")]
        public string LoginBack { get; set; }

        [XmlAttribute("Login")]
        public string Login { get; set; }

        [XmlAttribute("Icon")]
        public string Icon { get; set; }

        [XmlAttribute("UnloginLogo")]
        public string UnloginLogo { get; set; }

        [XmlAttribute("LoginedLogo")]
        public string LoginedLogo { get; set; }

        [XmlAttribute("Title")]
        public string Title { get; set; }

        [XmlAttribute("LoginRegister")]
        public bool LoginRegister { get; set; }

        [XmlAttribute("RegistrationAgreement")]
        public bool RegistrationAgreement { get; set; }
    }
}

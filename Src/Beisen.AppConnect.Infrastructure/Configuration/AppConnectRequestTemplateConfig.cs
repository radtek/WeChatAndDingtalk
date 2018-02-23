using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Beisen.Configuration;

namespace Beisen.AppConnect.Infrastructure.Configuration
{
    /// <summary>
    /// 请求模板配置文件
    /// </summary>
    [Serializable, XmlRoot("AppConnectRequestTemplate")]
    public class AppConnectRequestTemplateConfig : BaseConfig<AppConnectRequestTemplateConfig>
    {
        internal static object SyncRoot = new object();

        /// <summary>
        /// 配置文件缓存
        /// </summary>
        public static Dictionary<int, Dictionary<string, ApiDetail>> Cache = new Dictionary<int, Dictionary<string, ApiDetail>>();

        /// <summary>
        /// 请求模板列表
        /// </summary>
        [XmlArrayItem("AppAccount")]
        public List<AppAccountDetail> AppAccounts { get; set; }

        /// <summary>
        /// 初始化请求模板配置文件
        /// </summary>
        static AppConnectRequestTemplateConfig()
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
        internal static void LoadCache(AppConnectRequestTemplateConfig config)
        {
            if (config == null || config.AppAccounts == null || config.AppAccounts.Count==0)
            {
                return;
            }
            lock (SyncRoot)
            {
                Cache.Clear();
                foreach (var appAccount in config.AppAccounts)
                {
                    var apiDetailDic = new Dictionary<string, ApiDetail>();
                    foreach (var api in appAccount.Apis)
                    {
                        var apiDetail = new ApiDetail();
                        apiDetail.Name = api.Name.ToUpperInvariant();
                        apiDetail.Url = api.Url;
                        apiDetail.Method = api.Method;
                        apiDetail.RequestBody = api.RequestBody;
                        apiDetail.ResponseMapping = new Dictionary<string, string>();
                        foreach (var response in api.ResponseMapping)
                        {
                            apiDetail.ResponseMapping.Add(response.Key.ToUpperInvariant(), response.Value);
                        }
                        apiDetailDic.Add(api.Name.ToUpperInvariant(), apiDetail);
                    }
                    Cache.Add(appAccount.Type, apiDetailDic);
                }
            }
        }
    }

    [Serializable]
    public class AppAccountDetail
    {
        [XmlAttribute]
        public int Type { get; set; }

        [ XmlArrayItem("Api")]
        public List<ApiConfig> Apis { get; set; }
    }

    [Serializable]
    public class ApiConfig
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement]
        public string Url { get; set; }

        [XmlElement]
        public string Method { get; set; }

        [XmlElement]
        public string RequestBody { get; set; }

        [XmlArray("ResponseMapping"),XmlArrayItem("Mapping")]
        public List<MappingConfig> ResponseMapping { get; set; }

    }

    [Serializable]
    public class MappingConfig
    {
        [XmlAttribute]
        public string Key { get; set; }

        [XmlAttribute]
        public string Value { get; set; }
    }

    /// <summary>
    /// 接口信息明细
    /// </summary>
    public class ApiDetail
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 接口URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 接口Method
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 请求Body
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// 返回值映射
        /// </summary>
        public Dictionary<string, string> ResponseMapping { get; set; }
    }
}

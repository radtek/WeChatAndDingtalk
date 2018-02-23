using System;
using System.Collections.Concurrent;
using System.Reflection;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.Infrastructure.Helper;

namespace Beisen.AppConnect.Infrastructure
{
    /// <summary>
    /// 获取provider实例的工厂
    /// </summary>
    public static class ProviderFactory
    {
        /// <summary>
        /// provider仓库
        /// </summary>
        private static readonly ConcurrentDictionary<string, object> Repositories = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 创建provider实例
        /// </summary>
        /// <typeparam name="TProvider"></typeparam>
        /// <returns></returns>
        public static TProvider GetProvider<TProvider>() where TProvider : class
        {
            try
            {
                string typeName = typeof (TProvider).FullName;

                AppConnectProvider providerConfig = GetProviderConfig(typeName);

                if (!Repositories.ContainsKey(providerConfig.TypeName))
                {
                    Type providerType = Type.GetType(providerConfig.TypeName);
                    if (providerType == null)
                    {
                        throw new ArgumentException(typeName, typeName + " provider is null ");
                    }
                    var bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty;
                    var provider = providerType.InvokeMember("Instance", bindingFlags, null, null, null) as TProvider;
                    //var provider = Activator.CreateInstance(providerType) as TProvider;
                    Repositories[providerConfig.TypeName] = provider;
                    return provider;
                }
                return Repositories[providerConfig.TypeName] as TProvider;
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.Error("创建provider实例出错", ex);
                return null;
            }
        }

        /// <summary>
        /// 根据类型名获取配置文件
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private static AppConnectProvider GetProviderConfig(string typeName)
        {
            if (AppConnectProviderConfig.Cache == null || AppConnectProviderConfig.Cache.Count == 0)
            {
                throw new ArgumentException("AppConnectProviderConfig Error");
            }
            if (!AppConnectProviderConfig.Cache.ContainsKey(typeName))
            {
                throw new ArgumentException(typeName, string.Format("{0} providerConfig is null ", typeName));
            }
            return AppConnectProviderConfig.Cache[typeName];
        }

        
    }
}

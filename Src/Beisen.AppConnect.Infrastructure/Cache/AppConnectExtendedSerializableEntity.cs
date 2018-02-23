using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.EntityFramework;
using MySpace.Common.IO;

namespace Beisen.AppConnect.Infrastructure.Cache
{
    public abstract class AppConnectExtendedSerializableEntity<T> : ExtendedSerializableEntity, IAppConnectExtendedSerializableEntity
       where T : ExtendedSerializableEntity
    {
        /// <summary>
        /// 缓存应用程序名
        /// </summary>
        public string CacheName { get { return typeof(T).FullName; } }

        /// <summary>
        /// 是否启用DataRelay缓存
        /// </summary>
        public bool IsEnable
        {
            get
            {
                if (AppConnectCacheVersionConfig.Cache.ContainsKey(CacheName))
                {
                    return bool.Parse(AppConnectCacheVersionConfig.Cache[CacheName].Enable);
                }
                return false;
            }
        }

        /// <summary>
        /// 缓存版本
        /// </summary>
        public override int CurrentVersion
        {
            get
            {
                return AppConnectCacheVersionConfig.Cache[CacheName].Version;
            }
        }

        public override void Deserialize(IPrimitiveReader reader, int version)
        {
            if (AppConnectCacheVersionConfig.Cache.ContainsKey(CacheName))
            {
                var cache = AppConnectCacheVersionConfig.Cache[CacheName];
                if (bool.Parse(cache.Enable) && cache.Version == version)
                {
                    Deserialize(reader);
                }
            }
        }

        public new abstract void Deserialize(IPrimitiveReader reader);
    }
}
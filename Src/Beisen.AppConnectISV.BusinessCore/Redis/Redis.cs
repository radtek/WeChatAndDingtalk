using System;
using System.Text;
using Beisen.RedisV2.Provider;
using Newtonsoft.Json;
using Beisen.AppConnectISV.Infrastructure;

namespace Beisen.AppConnectISV.BusinessCore
{
    public class Redis
    {
        #region Singleton
        static readonly Redis _Instance = new Redis();
        public static Redis Instance
        {
            get
            {
                return _Instance;
            }
        }
        #endregion

        /// <summary>
        /// KeySpace
        /// </summary>
        private string _KeySpace = RedisInfo.KeySpace;
        private int _TenantId = RedisInfo.ISVTenantId;

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetRedis(string key)
        {
            try
            {
                using (var redis = new RedisNativeProviderV2(_KeySpace, _TenantId))
                {
                    var value = redis.Get(key);
                    if (value != null && value.Length != 0)
                    {
                        return Encoding.UTF8.GetString(value);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(string.Format("GetRedis error:key={0}", key), ex);
            }
            return null;
        }
        public T GetRedis<T>(string key)
        {
            try
            {
                using (var redis = new RedisNativeProviderV2(_KeySpace, _TenantId))
                {
                    var value = redis.Get(key);
                    if (value != null && value.Length != 0)
                    {
                        return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(value));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(string.Format("泛型GetRedis error:key={0}", key), ex);
            }
            return default(T);
        }
        /// <summary>
        /// 写入Redis
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="exSeconds">缓存时间</param>
        public void SetRedis(string key, string value, int exSeconds)
        {
            try
            {
                using (var redis = new RedisNativeProviderV2(_KeySpace, _TenantId))
                {
                    var bytes = Encoding.UTF8.GetBytes(value);
                    redis.SetEx(key, exSeconds, bytes);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(string.Format("SetRedis error:key={0},value={1},exSeconds={2}", key, value, exSeconds), ex);
            }
        }

        /// <summary>
        /// 写入Redis
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetRedis(string key, string value)
        {
            try
            {
                using (var redis = new RedisNativeProviderV2(_KeySpace, _TenantId))
                {
                    var bytes = Encoding.UTF8.GetBytes(value);
                    redis.Set(key, bytes);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(string.Format("SetRedis error:key={0},value={1}", key, value), ex);
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsExist(string key)
        {
            try
            {
                using (var redis = new RedisNativeProviderV2(_KeySpace, _TenantId))
                {
                    return redis.Exists(key);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(string.Format("RedisIsExist error:key={0}", key), ex);
            }
            return false;
        }

        /// <summary>
        /// 删除Redis
        /// </summary>
        /// <param name="key"></param>
        public void DelRedis(string key)
        {
            try
            {
                using (var redis = new RedisNativeProviderV2(_KeySpace, _TenantId))
                {
                    redis.Del(key);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(string.Format("GetRedis error:key={0}", key), ex);
            }
        }
    }
}

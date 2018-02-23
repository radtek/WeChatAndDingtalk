using System;
using System.Text;
using Beisen.RedisV2.Provider;

namespace Beisen.AppConnect.Infrastructure.Helper
{
    public class RedisHelper
    {
        /// <summary>
        /// KeySpace
        /// </summary>
        private const string _KeySpace = "AppConnect";

        private const int _TenantId = 999999;

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetRedis(string key)
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
                AppConnectLogHelper.Error(string.Format("GetRedis error:key={0}", key), ex);
            }
            return null;
        }

        /// <summary>
        /// 写入Redis
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="exSeconds">缓存时间</param>
        public static void SetRedis(string key, string value, int exSeconds)
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
                AppConnectLogHelper.Error(string.Format("SetRedis error:key={0},value={1},exSeconds={2}", key,value,exSeconds), ex);
            }
        }

        /// <summary>
        /// 写入Redis
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetRedis(string key, string value)
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
                AppConnectLogHelper.Error(string.Format("SetRedis error:key={0},value={1}", key,value), ex);
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsExist(string key)
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
                AppConnectLogHelper.Error(string.Format("RedisIsExist error:key={0}", key), ex);
            }
            return false;
        }

        /// <summary>
        /// 删除Redis
        /// </summary>
        /// <param name="key"></param>
        public static void DelRedis(string key)
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
                AppConnectLogHelper.Error(string.Format("GetRedis error:key={0}", key), ex);
            }
        }
    }

    public class RedisConstName
    {
        public const string LoginErrorCount = "LoginErrorCount";

        public const string Captcha = "Captcha";

        public const string SendCheck = "SendCheck";

        //public const string MobileCheck = "MobileCheck";
    }
}

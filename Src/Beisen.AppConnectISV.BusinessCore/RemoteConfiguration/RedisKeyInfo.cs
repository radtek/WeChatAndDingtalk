using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore
{
    /// <summary>
    /// 获取远程配置的RedisKey
    /// </summary>
    public class RedisKeyInfo : RedisKeyBase
    {
        public static string SuiteTicket
        {
            get
            {
                var configValue = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.RedisKey, AppconnectConfigConst.RedisKey_SuiteTicket);
                if (string.IsNullOrWhiteSpace(configValue))
                {
                    throw new Exception(string.Format("未在远程配置中获取RedisKey_ Suite_Ticket!"));
                }
                var redisKey = string.Format(configValue, _SuiteKey);
                return redisKey;
            }
        }
        public static string SuiteAccessToken
        {
            get
            {
                var configValue = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.RedisKey, AppconnectConfigConst.RedisKey_SuiteAccessToken);
                if (string.IsNullOrWhiteSpace(configValue))
                {
                    throw new Exception(string.Format("未在远程配置中获取RedisKe_SuiteAccessToken"));
                }
                var redisKey = string.Format(configValue, _SuiteKey);
                return redisKey;
            }
        }

        public static string SuiteTmpAuthCode
        {
            get
            {
                var configValue = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.RedisKey, AppconnectConfigConst.RedisKey_SuiteTmpAuthCode);
                if (string.IsNullOrWhiteSpace(configValue))
                {
                    throw new Exception(string.Format("未在远程配置中获取RedisKe_SuiteTmpAuthCode"));
                }
                var redisKey = string.Format(configValue, _SuiteKey);
                return redisKey;
            }
        }
        public static string CorpPermanentCode(string corpId)
        {
            var configValue = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.RedisKey, AppconnectConfigConst.RedisKey_CorpPermanentCode);
            if (string.IsNullOrWhiteSpace(configValue))
            {
                throw new Exception(string.Format("未在远程配置中获取RedisKe_SuiteTmpAuthCode"));
            }
            var redisKey = string.Format(configValue, ISVInfo.SuiteKey, corpId);
            return redisKey;
        }
        public static string CorpToken(string cropId)
        {
            var configValue = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.RedisKey, AppconnectConfigConst.RedisKey_CorpToken);
            if (string.IsNullOrWhiteSpace(configValue))
            {
                throw new Exception(string.Format("未在远程配置中获取RedisKey_CorpToken"));
            }
            var redisKey = string.Format(configValue, ISVInfo.SuiteKey, cropId);
            return redisKey;
        }
        public static string CorpJsApiTicket(string cropId)
        {
            var configValue = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.RedisKey, AppconnectConfigConst.RedisKey_CorpJsApiTicket);
            if (string.IsNullOrWhiteSpace(configValue))
            {
                throw new Exception(string.Format("未在远程配置中获取RedisKey_CorpToken"));
            }
            var redisKey = string.Format(configValue, ISVInfo.SuiteKey, cropId);
            return redisKey;
        }
        public static string AuthCorpInfo(string cropId)
        {
            var configValue = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.RedisKey, AppconnectConfigConst.RedisKey_AuthCorpInfo);
            if (string.IsNullOrWhiteSpace(configValue))
            {
                throw new Exception(string.Format("未在远程配置中获取RedisKey_AuthCorpInfo"));
            }
            var redisKey = string.Format(configValue, ISVInfo.SuiteKey, cropId);
            return redisKey;
        }
    }
}

using System;
using System.Collections.Generic;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.EntityFramework;

namespace Beisen.AppConnect.Infrastructure.Cache
{
    /// <summary>
    /// Relay缓存
    /// </summary>
    public static class AppConnectEntityProcedure
    {
        /// <summary>
        /// 保存缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void SaveEntityInstanceToCache<T>(T instance) where T : IAppConnectExtendedSerializableEntity, new()
        {
            try
            {
                if (instance.IsEnable)
                {
                    EntityProcedure.SaveEntityInstanceToCache(instance);
                }
            }
            catch (Exception exception)
            {
                AppConnectLogHelper.Error(exception);
            }
        }

        /// <summary>
        /// 保存缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instances"></param>
        public static void SaveEntityInstanceListToCache<T>(IList<T> instances) where T : IAppConnectExtendedSerializableEntity, new()
        {
            try
            {
                if (instances != null && instances.Count > 0 && instances[0].IsEnable)
                {
                    EntityProcedure.SaveEntityInstanceListToCache(instances);
                }
            }
            catch (Exception exception)
            {
                AppConnectLogHelper.Error(exception);
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void DeleteEntityFromCacheByExtendedId<T>(T instance) where T : IAppConnectExtendedSerializableEntity, new()
        {
            try
            {
                if (instance.IsEnable)
                {
                    EntityProcedure.DeleteEntityFromCache<T>(instance.ExtendedId);
                }
            }
            catch (Exception exception)
            {
                AppConnectLogHelper.Error(exception);
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void DeleteEntityFromCacheByPrimaryId<T>(T instance) where T : IAppConnectExtendedSerializableEntity, new()
        {
            try
            {
                if (instance.IsEnable)
                {
                    EntityProcedure.DeleteEntityFromCache<T>(instance.PrimaryId);
                }
            }
            catch (Exception exception)
            {
                AppConnectLogHelper.Error(exception);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static T GetEntityInstanceFromCacheByExtendedId<T>(T instance) where T : IAppConnectExtendedSerializableEntity, new()
        {
            try
            {
                if (instance.IsEnable)
                {
                    var item = EntityProcedure.GetEntityInstanceFromCache<T>(instance.ExtendedId);
                    if (item == null || item.IsEmpty)
                    {
                        return default(T);
                    }
                    return item;
                }
            }
            catch (Exception exception)
            {
                AppConnectLogHelper.Error(exception);
            }
            return default(T);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static T GetEntityInstanceFromCachePrimaryId<T>(T instance) where T : IAppConnectExtendedSerializableEntity, new()
        {
            try
            {
                if (instance.IsEnable)
                {
                    var item = EntityProcedure.GetEntityInstanceFromCache<T>(instance.PrimaryId);
                    if (item == null || item.IsEmpty)
                    {
                        return default(T);
                    }
                    return item;
                }
            }
            catch (Exception exception)
            {
                AppConnectLogHelper.Error(exception);
            }
            return default(T);
        }
    }
}
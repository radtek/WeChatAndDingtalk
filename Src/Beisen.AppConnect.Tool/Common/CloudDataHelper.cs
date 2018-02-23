using Beisen.MultiTenant.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beisen.ESB.ClientV2;
using Beisen.MultiTenant.Model;
using Beisen.SearchV3.DSL;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Constants;

namespace Beisen.AppConnect.Tool.Common
{
   public static class CloudDataHelper
    {
        #region 获取元数据实例
        private static IMetaObjectProvider MetaObjectProvider
        {
            get
            {
                var instance = PlatformServiceFactoryV2<IMetaObjectProvider>.Instance("ESB_MultiTenant");
                if (instance == null)
                {
                    throw new Exception("获取IMetaObjectProvider的ESB实例失败！");
                }
                return instance;
            }
        }

        #endregion

        #region 获取多租赁数据实例
        private static IDataAccessProvider DataAccessProvider
        {
            get
            {
                var instance = PlatformServiceFactoryV2<IDataAccessProvider>.Instance("ESB_MultiTenant");
                if (instance == null)
                {
                    throw new Exception("获取IDataAccessProvider的ESB实例失败！");
                }
                return instance;
            }
        }
        #endregion

        #region 获取元数据
        /// <summary>
        /// 根据租户和对象名获取一个指定的元数据
        /// </summary>
        /// <param name="tenantId">租户ID</param>
        /// <param name="metaObjectName">元数据对象名称</param>
        /// <returns>元数据MetaObject</returns>
        public static MetaObject GetMetaObject(int tenantId, string metaObjectName)
        {
            return MetaObjectProvider.GetObjectMeta(metaObjectName, tenantId);
        }
        /// <summary>
        /// 根据Guid获取一个指定的元数据对象
        /// </summary>
        /// <param name="metaObjectId">元数据对象的GUID</param>
        /// <returns></returns>
        public static MetaObject GetMetaObject(Guid metaObjectId)
        {
            return MetaObjectProvider.GetFullMetaObjectById(metaObjectId);
        }
        /// <summary>
        /// 获取一个应用下所有的元数据
        /// </summary>
        /// <param name="tenantId">租户ID</param>
        /// <param name="metaApplicationName">应用对象名称</param>
        /// <returns>元数据MetaObject的一个列表List</returns>
        public static List<MetaObject> GetApplicationAllMetaObjects(int tenantId, string metaApplicationName)
        {
            return MetaObjectProvider.GetAllObjectsByTenant(tenantId, metaApplicationName);
        }
        #endregion
        public static IEnumerable<ObjectData> GetEntityAllList(string metaObjectName, int tenantId, IFilter filter = null, DeletedStatus deletedStatus = DeletedStatus.False, string queryJson = null, Dictionary<string, SortDirection> sortFields = null, string[] columnNames = null)
        {
            return GetEntityAllList(metaObjectName, tenantId, null, queryJson, sortFields, columnNames);
        }

        private static IEnumerable<ObjectData> GetEntityAllList(string metaObjectName, int tenantId, string filterJson = null, string queryJson = null, Dictionary<string, SortDirection> sortFields = null, string[] columnNames = null)
        {
            sortFields = DefaultSort(metaObjectName, sortFields);

            var result = new List<ObjectData>();
            int totalCount = DataAccessProvider.GetCount(metaObjectName, tenantId, filterJson);

            int pageSize = CloudConstants.MaxQueryRowLimit;
            int pageCount = (int)Math.Ceiling((float)totalCount / pageSize);

            for (int from = 0; from < pageCount; from++)
            {
                result.AddRange(DataAccessProvider.GetEntityList(metaObjectName, tenantId, filterJson, from: pageSize * from, size: pageSize, columnNames: columnNames, sortFields: sortFields));
            }
            return result;
        }
        private static Dictionary<string, SortDirection> DefaultSort(string metaObjectName, Dictionary<string, SortDirection> sortFields)
        {
            if (sortFields == null)
            {
                sortFields = new Dictionary<string, SortDirection>();
                sortFields.Add("_uid", SortDirection.Asc);
            }
            else
            {
                if (!sortFields.ContainsKey("_uid"))
                {
                    sortFields.Add("_uid", SortDirection.Asc);
                }
            }

            return sortFields;
        }
        #region 多租赁更新 


        public static void Update(MetaObject metaObject, List<ObjectData> objectDatas)
        {
            LoopWrite(metaObject, objectDatas, DataAccessProvider.Update);
        }
        public static void Update(ObjectData objectData)
        {
            DataAccessProvider.Update(objectData);
        }
        #endregion

        #region 多租赁增加
        /// <summary>      
        /// </summary>
        /// <param name="metaObject">注：objectDatas的MetaObject必须赋值否则报错Value cannot be null</param>
        /// <param name="objectDatas"></param>
        public static void Add(MetaObject metaObject, List<ObjectData> objectDatas)
        {
            LoopWrite(metaObject, objectDatas, DataAccessProvider.Add);
        }

        private static void LoopWrite(MetaObject metaObject, List<ObjectData> objectDatas, Action<MetaObject, List<ObjectData>, bool, bool> action)
        {
            int totalCount = objectDatas.Count();

            int pageSize = CloudConstants.MaxQueryRowLimit;
            int pageCount = (int)Math.Ceiling((float)totalCount / pageSize);
            var take = 0;
            for (int from = 0; from < pageCount; from++)
            {
                take = totalCount - pageSize * from - pageSize > 0 ? pageSize : totalCount - pageSize * from;

                action(metaObject, objectDatas.GetRange(pageSize * from, take), false, false);
                if (pageSize * from / 1000 != pageSize * (from + 1) / 1000)
                    System.Threading.Thread.Sleep(2000);
            }
        }

        public static void Add(ObjectData objectData)
        {
            DataAccessProvider.Add(objectData);
        }

        #endregion

        #region 多租赁删除（真删）
        public static void Delete(string metaObjectName, int tenantId, params string[] objectDataIds)
        {
            int totalCount = objectDataIds.Count();

            int pageSize = CloudConstants.MaxQueryRowLimit;
            int pageCount = (int)Math.Ceiling((float)totalCount / pageSize);
            var take = 0;
            for (int from = 0; from < pageCount; from++)
            {
                take = totalCount - pageSize * from - pageSize > 0 ? pageSize : totalCount - pageSize * from;

                DataAccessProvider.Delete(metaObjectName, tenantId, objectDataIds.ToList().GetRange(pageSize * from, take).ToArray());
                System.Threading.Thread.Sleep(1000);
            }
        }
        #endregion
    }
}

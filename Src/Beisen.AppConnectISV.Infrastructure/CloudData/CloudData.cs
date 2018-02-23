using Beisen.DynamicScript.SDK;
using Beisen.MultiTenant.Model;
using Beisen.MultiTenant.ServiceInterface;
using Beisen.SearchV3;
using Beisen.SearchV3.DSL;
using Beisen.SearchV3.DSL.Filters;
using Beisen.SearchV3.DSL.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Beisen.AppConnectISV.Infrastructure
{
    public static class CloudData
    {
        #region 获取元数据接口封装
        private static IMetaObjectProvider MetaObjectProvider
        {
            get
            {
                var instance = ESBProxyV2.GetInstance<IMetaObjectProvider>("ESB_MultiTenant");
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
                // var instance = ServiceLocator<IDataAccessProvider>.Instance;

                var instance = ESBProxyV2.GetInstance<IDataAccessProvider>("ESB_MultiTenant");
                if (instance == null)
                {
                    throw new Exception("获取IDataAccessProvider的ESB实例失败！");
                }
                return instance;
            }
        }
        #endregion

        #region 多租赁逻辑删除
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="metaObject">元数据对象</param>
        /// <param name="objectDatas">待删除数据列表</param>
        private static void LogicDeleteDataObjects(MetaObject metaObject, IEnumerable<ObjectData> objectDatas)
        {
            var objectDataList = objectDatas.ToList();
            if (objectDataList.Count == 0) { return; }
            objectDataList.ForEach(n =>
            {
                n.MetaFields.AddOrUpdate(CloudDataConst.LogicalDelete, new Beisen.MultiTenant.Model.Data(CloudDataConst.LogicalDelete, DataType.Boolean, true));
            });
            Update(metaObject, objectDataList);
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="metaObjectName">元数据名称</param>
        /// <param name="tenantId">租户ID</param>
        /// <param name="objectDatas">待删除数据列表</param>
        public static void LogicDeleteDataObjects(string metaObjectName, int tenantId, IEnumerable<ObjectData> objectDatas)
        {
            var metaObject = GetMetaObject(tenantId, metaObjectName);
            LogicDeleteDataObjects(metaObject, objectDatas);
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="metaObjectName">元数据名称</param>
        /// <param name="tenantId">租户ID</param>
        /// <param name="objectIds">待删除数据ID的列表</param>
        public static void LogicDeleteDataObjects(string metaObjectName, string fieldName, int tenantId, string[] objectIds)
        {
            var booleanFilter = new BooleanFilter().Must(new QueryFilter(new MatchQuery(metaObjectName + "." + fieldName, objectIds)));
            var objectDataList = GetEntityAllList(metaObjectName, tenantId, ElasticSearchSerialization.Serialize(booleanFilter));
            LogicDeleteDataObjects(metaObjectName, tenantId, objectDataList);
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="metaObjectName">元数据名称</param>
        /// <param name="tenantId">租户ID</param>
        /// <param name="objectIds">待删除数据ID的列表</param>
        public static void LogicDeleteDataObjects(string metaObjectName, int tenantId, string[] objectIds)
        {
            var booleanFilter = new BooleanFilter()
             // .Must(new QueryFilter(new MatchQuery("_id", objectIds.ToArray())));
             .Must(new TermsFilter(metaObjectName + "_id", objectIds.ToArray()));
            var objectDataList = GetEntityAllList(metaObjectName, tenantId, ElasticSearchSerialization.Serialize(booleanFilter));
            LogicDeleteDataObjects(metaObjectName, tenantId, objectDataList);
        }
        /// <summary>
        /// 逻辑删除-单个
        /// </summary>
        /// <param name="metaObjectName">元数据名称</param>
        /// <param name="tenantId">租户ID</param>
        /// <param name="objectData">待删除数据对象</param>
        public static void LogicDeleteDataObject(string metaObjectName, int tenantId, ObjectData objectData)
        {
            LogicDeleteDataObjects(metaObjectName, tenantId, new ObjectData[] { objectData });
        }
        #endregion 逻辑删除

        #region 多租赁真删除
        public static void Delete(string metaObjectName, int tenantId, params string[] objectDataIds)
        {
            int totalCount = objectDataIds.Count();

            int pageSize = CloudDataConst.MaxQueryRowLimit;
            int pageCount = (int)Math.Ceiling((float)totalCount / pageSize);
            var take = 0;
            for (int from = 0; from < pageCount; from++)
            {
                take = totalCount - pageSize * from - pageSize > 0 ? pageSize : totalCount - pageSize * from;

                DataAccessProvider.Delete(metaObjectName, tenantId, objectDataIds.ToList().GetRange(pageSize * from, take).ToArray());
            }
        }
        #endregion

        #region 获取元数据
        public static MetaObject GetMetaObject(int tenantId, string metaObjectName)
        {
            return MetaObjectProvider.GetObjectMeta(metaObjectName, tenantId);
        }

        public static List<MetaObject> GetAllMetaObjects(int tenantId, string metaApplicationName)
        {
            return MetaObjectProvider.GetAllObjectsByTenant(tenantId, metaApplicationName);
        }

        public static Dictionary<string, MetaObject> GetMetaObjects(int tenantId, params string[] metaObjectNames)
        {
            Dictionary<string, MetaObject> dic = new Dictionary<string, MetaObject>();
            foreach (var metaObjectName in metaObjectNames)
            {
                dic.Add(metaObjectName, MetaObjectProvider.GetObjectMeta(metaObjectName, tenantId));
            }
            return dic;
        }

        public static MetaObject GetMetaObject(Guid metaObjectId)
        {
            return MetaObjectProvider.GetFullMetaObjectById(metaObjectId);
        }
        #endregion

        #region 多租赁查询
        public static SearchResultData GetEntityListWithCount(string metaObjectName, int tenantId, string[] columnNames = null, string filterJson = null, string queryJson = null, Dictionary<string, SortDirection> sortFields = null, int from = 0, int size = 10)
        {
            try
            {
                return DataAccessProvider.GetEntityListWithCount(metaObjectName, tenantId, columnNames, filterJson, queryJson, sortFields, from, size);
            }
            catch (Exception ex)
            {
                throw new Exception("获取多租赁数据异常", ex);
            }
        }
        private static Dictionary<string, SortDirection> DefaultSort(string metaObjectName, Dictionary<string, SortDirection> sortFields)
        {
            //默认排序
            var sortString = string.Format("{0}.{1}", metaObjectName, "CreatedTime");
            if (sortFields == null)
            {
                sortFields = new Dictionary<string, SortDirection>();
                sortFields.Add(sortString, SortDirection.Asc);
            }
            else
            {
                if (!sortFields.ContainsKey(sortString))
                {
                    sortFields.Add(sortString, SortDirection.Asc);
                }
            }

            return sortFields;
        }

        public static IEnumerable<ObjectData> GetEntityListWithSize(string metaObjectName, int tenantId, int size, IFilter filter = null, string queryJson = null, Dictionary<string, SortDirection> sortFields = null, string[] columnNames = null)
        {
            var filterJson = filter == null ? null : ElasticSearchSerialization.Serialize(filter);
            return GetEntityListWithSize(metaObjectName, tenantId, filterJson: filterJson, columnNames: columnNames, size: size, sortFields: sortFields);
        }

        public static IEnumerable<ObjectData> GetEntityListWithSize(string metaObjectName, int tenantId, int size, string filterJson = null, string queryJson = null, Dictionary<string, SortDirection> sortFields = null, string[] columnNames = null)
        {
            //  GuardHelper.Require(size, "size", Guards.Maximum(100));
            sortFields = DefaultSort(metaObjectName, sortFields);

            return DataAccessProvider.GetEntityList(metaObjectName, tenantId, filterJson, columnNames: columnNames, size: size, sortFields: sortFields);
        }


        public static IEnumerable<ObjectData> GetEntityAllList(string metaObjectName, int tenantId, IFilter filter = null, DeletedStatus deletedStatus = DeletedStatus.False, string queryJson = null, Dictionary<string, SortDirection> sortFields = null, string[] columnNames = null)
        {
            var deleteFilter = new BooleanFilter();
            if (deletedStatus == DeletedStatus.True)
                deleteFilter.Must(new TermFilter(metaObjectName + "." + "StdIsDeleted", true));
            else if (deletedStatus == DeletedStatus.False)
                deleteFilter.Must(new TermFilter(metaObjectName + "." + "StdIsDeleted", false));
            else
                deleteFilter = null;
            if (filter == null)
            {
                filter = deleteFilter;
            }
            else
            {
                filter = deleteFilter == null ? filter : (filter as BooleanFilter).Must(deleteFilter);
            }
            var filterJson = filter == null ? null : ElasticSearchSerialization.Serialize(filter);
            return GetEntityAllList(metaObjectName, tenantId, filterJson, queryJson, sortFields, columnNames);
        }
        private static IEnumerable<ObjectData> GetEntityAllList(string metaObjectName, int tenantId, string filterJson = null, string queryJson = null, Dictionary<string, SortDirection> sortFields = null, string[] columnNames = null)
        {
            sortFields = DefaultSort(metaObjectName, sortFields);
            var result = new List<ObjectData>();
            int totalCount = DataAccessProvider.GetCount(metaObjectName, tenantId, filterJson);

            int pageSize = CloudDataConst.MaxQueryRowLimit;
            int pageCount = (int)Math.Ceiling((float)totalCount / pageSize);

            for (int from = 0; from < pageCount; from++)
            {
                result.AddRange(DataAccessProvider.GetEntityList(metaObjectName, tenantId, filterJson, from: pageSize * from, size: pageSize, columnNames: columnNames, sortFields: sortFields));
            }
            return result;
        }

        public static IEnumerable<ObjectData> GetEntityListByFilter(string metaObjectName, int tenantId, IFilter filter = null, string queryJson = null, Dictionary<string, SortDirection> sortFields = null, string[] columnNames = null)
        {
            var filterJson = filter == null ? null : ElasticSearchSerialization.Serialize(filter);
            return GetEntityAllList(metaObjectName, tenantId, filterJson, queryJson, sortFields, columnNames);
        }

        /// <summary>
        /// 调用自封装的方法查询多租赁数据
        /// </summary>
        /// <param name="metaObjectName">元数据名称</param>
        /// <param name="tenantId">租户ID</param>
        /// <param name="filterJson">过滤条件</param>
        /// <param name="queryJson">查询条件（废弃，传随意）</param>
        /// <param name="sortFields">排序条件</param>
        /// <param name="columnNames">要查询的列名</param>
        /// <param name="from">从第几个开始</param>
        /// <param name="size">分页大小</param>
        /// <param name="skip">跳页</param>
        /// <param name="take">要查询的页总数，0为全部查询</param>
        /// <returns></returns>
        public static IEnumerable<ObjectData> GetEntityList(string metaObjectName, int tenantId, string filterJson = null, string queryJson = null, Dictionary<string, SortDirection> sortFields = null, string[] columnNames = null, int from = 0, int size = CloudDataConst.MaxQueryRowLimit, int skip = 0, int take = 0)
        {
            //导向统一的方法
            return GetEntityAllList(metaObjectName, tenantId, filterJson, queryJson, sortFields, columnNames);
        }
        /// <summary>
        /// 简单封装多租赁根据objectDataIds查询的方法，主要是为了防止多租赁查询有限制
        /// </summary>
        /// <param name="metaObjectName">元数据名称</param>
        /// <param name="tenantId">租户ID</param>
        /// <param name="objectDataIDs">数据ID列表</param>
        /// <param name="columnNames">列名</param>
        /// <returns></returns>
        public static IEnumerable<ObjectData> GetEntityListForIds(string metaObjectName, int tenantId, string[] objectDataIDs, string[] columnNames = null)
        {
            //这个地方的确有限制，超过100的就是不行，所以还是要限制查询
            var result = new List<ObjectData>();
            var Idslist = objectDataIDs.ToList();
            int totalCount = Idslist.Count;

            int pageSize = CloudDataConst.MaxQueryRowLimit;
            int pageCount = (int)Math.Ceiling((float)totalCount / pageSize);
            int take = 0;
            for (int from = 0; from < pageCount; from++)
            {
                take = totalCount - pageSize * from - pageSize > 0 ? pageSize : totalCount - pageSize * from;
                result.AddRange(DataAccessProvider.GetEntityList(metaObjectName, tenantId, Idslist.GetRange(pageSize * from, take).ToArray(), columnNames));
            }
            return result;
        }

        #endregion

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
        public static void Add(MetaObject metaObject, List<ObjectData> objectDatas)
        {
            LoopWrite(metaObject, objectDatas, DataAccessProvider.Add);
        }

        private static void LoopWrite(MetaObject metaObject, List<ObjectData> objectDatas, Action<MetaObject, List<ObjectData>, bool, bool> action)
        {
            int totalCount = objectDatas.Count();

            int pageSize = CloudDataConst.MaxQueryRowLimit;
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


        public static ObjectData ConvertJsonToObjectData(ObjectDataFromJson dataObject, int tenantId, MetaObject metaData)
        {
            var datalist = dataObject.metaFields;

            ObjectData objectData = new ObjectData(metaData);

            foreach (var data in datalist)
            {
                objectData[data.name] = data.value;
            }
            return objectData;
        }

        public static ObjectData ConvertJsonToObjectData(ObjectDataFromJson dataObject, int tenantId, string metaObjectName)
        {
            MetaObject metaData = MetaObjectProvider.GetObjectMeta(metaObjectName, tenantId);
            return ConvertJsonToObjectData(dataObject, tenantId, metaData);
        }

        public static OrFilter ConstituteOrFilter<T>(List<T> items, string fieldName)
        {
            var termsFilterList = new List<TermsFilter>();
            var itemsArray = items.ToArray();
            int totalCount = itemsArray.Count();

            int pageSize = CloudDataConst.TermsMaxLimit;
            int pageCount = (int)Math.Ceiling((float)totalCount / pageSize);
            var take = 0;
            for (int from = 0; from < pageCount; from++)
            {
                take = totalCount - pageSize * from - pageSize > 0 ? pageSize : totalCount - pageSize * from;
                termsFilterList.Add(new TermsFilter(fieldName, items.GetRange(pageSize * from, take).ConvertAll(n => n.ToString()).ToArray()));
            }

            return new OrFilter(termsFilterList.ToArray());
        }
    }
}

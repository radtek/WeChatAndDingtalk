using Beisen.MultiTenant.Model;
using Beisen.MultiTenant.ServiceInterface;
using Beisen.SearchV3;
using Beisen.SearchV3.DSL.Filters;
using Beisen.SearchV3.DSL.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using Beisen.SearchV3.DSL;
using Beisen.ESB.ClientV2;
using Beisen.AppConnect.Infrastructure.Constants;
using Beisen.AppConnect.Infrastructure.Helper;

namespace Beisen.AppConnect.Infrastructure
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

        #region 多租赁逻辑删除
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="metaObject">元数据对象</param>
        /// <param name="objectDatas">待删除数据列表</param>
        public static void LogicDeleteDataObjects(MetaObject metaObject, IEnumerable<ObjectData> objectDatas)
        {
            var objectDataList = objectDatas.ToList();
            if (objectDataList.Count == 0) { return; }
            objectDataList.ForEach(n =>
            {
                n.MetaFields.AddOrUpdate(CloudConstants.LogicalDelete, new Beisen.MultiTenant.Model.Data(CloudConstants.LogicalDelete, DataType.Boolean, true));
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
        public static void LogicDeleteDataObjects(string metaObjectName, int tenantId, string[] objectIds)
        {
            var objectDataList = GetEntityListByIds(metaObjectName, tenantId, objectIds);
            LogicDeleteDataObjects(metaObjectName, tenantId, objectDataList);
        }

        /// <summary>
        /// 逻辑删除-单个
        /// </summary>
        /// <param name="metaObjectName">元数据名称</param>
        /// <param name="tenantId">租户ID</param>
        /// <param name="objectId">待删除的数据ID</param>
        public static void LogicDeleteDataObject(string metaObjectName, int tenantId, string objectId)
        {
            LogicDeleteDataObjects(metaObjectName, tenantId, new string[] { objectId });
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

            int pageSize = CloudConstants.MaxQueryRowLimit;
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
        /// <summary>
        /// 批量获取元数据对象
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="metaObjectNames"></param>
        /// <returns></returns>
        public static Dictionary<string, MetaObject> GetMetaObjects(int tenantId, params string[] metaObjectNames)
        {
            Dictionary<string, MetaObject> dic = new Dictionary<string, MetaObject>();

            foreach (var metaObjectName in metaObjectNames)
            {
                try
                {
                    dic.Add(metaObjectName, MetaObjectProvider.GetObjectMeta(metaObjectName, tenantId));
                }
                catch (Exception ex)
                {
                    AppConnectLogHelper.ErrorFormat("批量获取元数据对象出现了问题，出错对象名为}");
                }
            }

            return dic;
        }
        #endregion

        #region 多租赁查询, DeletedStatus deletedStatus = DeletedStatus.Both
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
            sortFields = DefaultSort(metaObjectName, sortFields);

            return DataAccessProvider.GetEntityList(metaObjectName, tenantId, filterJson, columnNames: columnNames, size: size, sortFields: sortFields);
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
                result.AddRange(DataAccessProvider.GetEntityList(metaObjectName, tenantId, filterJson,queryJson,from: pageSize * from, size: pageSize, columnNames: columnNames, sortFields: sortFields));
            }
            return result;
        }

        public static IEnumerable<ObjectData> GetEntityAllList(string metaObjectName, int tenantId, IFilter filter = null, string queryJson = null, Dictionary<string, SortDirection> sortFields = null, string[] columnNames = null)
        {
            //var deleteFilter = new BooleanFilter();
            //if (filter == null)
            //{
            //    filter = deleteFilter;
            //}
            //else
            //{
            //    filter = deleteFilter == null ? filter : (filter as BooleanFilter).Must(deleteFilter);
            //}
            var filterJson = filter == null ? null : ElasticSearchSerialization.Serialize(filter);
            return GetEntityAllList(metaObjectName, tenantId, filterJson, queryJson, sortFields, columnNames);
        }
        //public static List<T> GetEntityAllList<T>(string metaObjectName, int tenantId, IFilter filter = null, DeletedStatus deletedStatus = DeletedStatus.False, string queryJson = null, Dictionary<string, SortDirection> sortFields = null, string[] columnNames = null)
        //{
        //    var objectDatas = GetEntityAllList(metaObjectName, tenantId, filter).ToList();
        //    var bussnissModels = objectDatas.ToExpandoModel<T>(tenantId);
        //    return bussnissModels;
        //}
        public static IEnumerable<ObjectData> GetEntityListByIds(string metaObjectName, int tenantId, string[] objectDataIDs, string[] columnNames = null)
        {
            //这个地方的确有限制，超过100的就是不行，所以还是要限制查询
            var result = new List<ObjectData>();
            var Idslist = objectDataIDs.ToList();
            int totalCount = Idslist.Count;

            int pageSize = CloudConstants.MaxQueryRowLimit;
            int pageCount = (int)Math.Ceiling((float)totalCount / pageSize);
            int take = 0;
            for (int from = 0; from < pageCount; from++)
            {
                take = totalCount - pageSize * from - pageSize > 0 ? pageSize : totalCount - pageSize * from;
                result.AddRange(DataAccessProvider.GetEntityList(metaObjectName, tenantId, Idslist.GetRange(pageSize * from, take).ToArray(), columnNames));
            }
            return result;
        }

        public static IEnumerable<string> GetEntityIds(string metaObjectName, int tenantId, string filterJson = null, Dictionary<string, SortDirection> sortFields = null)
        {
            sortFields = DefaultSort(metaObjectName, sortFields);

            var result = new List<string>();
            int totalCount = DataAccessProvider.GetCount(metaObjectName, tenantId, filterJson);

            int pageSize = CloudConstants.MaxQueryRowLimit;
            int pageCount = (int)Math.Ceiling((float)totalCount / pageSize);
            for (int from = 0; from < pageCount; from++)
            {
                result.AddRange(DataAccessProvider.GetEntityIds(metaObjectName, tenantId, filterJson, from: pageSize * from, size: pageSize, sortFields: sortFields));
            }
            return result;
        }

        public static ObjectData GetSingleEntity(string metaObjectName, int tenantId, string objectDataId, string[] columnNames = null)
        {
            return DataAccessProvider.GetEntity(metaObjectName, tenantId, objectDataId, columnNames);
        }

        /// <summary>
        /// 获取符合条件的多租赁数据的数量
        /// </summary>
        /// <param name="metaObjectName"></param>
        /// <param name="tenantId"></param>
        /// <param name="objectDataId"></param>
        /// <param name="filterJson"></param>
        /// <returns></returns>
        public static int GetCount(string metaObjectName, int tenantId, string objectDataId, string filterJson)
        {
            return DataAccessProvider.GetCount(metaObjectName, tenantId, filterJson);
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

        #region 其他工具
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

            int pageSize = CloudConstants.MaxQueryRowLimit;
            int pageCount = (int)Math.Ceiling((float)totalCount / pageSize);
            var take = 0;
            for (int from = 0; from < pageCount; from++)
            {
                take = totalCount - pageSize * from - pageSize > 0 ? pageSize : totalCount - pageSize * from;
                termsFilterList.Add(new TermsFilter(fieldName, items.GetRange(pageSize * from, take).ConvertAll(n => n.ToString()).ToArray()));
            }

            return new OrFilter(termsFilterList.ToArray());
        }
        #endregion
    }
}
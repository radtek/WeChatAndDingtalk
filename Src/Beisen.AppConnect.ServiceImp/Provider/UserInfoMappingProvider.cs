using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Constants;
using Beisen.AppConnect.Infrastructure.Enums;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.Common.Context;
using Beisen.MultiTenant.Model;
using Beisen.SearchV3.DSL.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp
{
    public class UserInfoMappingProvider : IUserInfoMappingProvider
    {
        #region Singleton
        static readonly IUserInfoMappingProvider _Instance = new UserInfoMappingProvider();
        public static IUserInfoMappingProvider Instance
        {
            get
            {
                return _Instance;
            }
        }
        #endregion

        public OperationResult UserUnbind(string objectIds)
        {
            OperationResult result = new OperationResult();
            try
            {
                int tenantId = ApplicationContext.Current.TenantId;
                int userId = ApplicationContext.Current.UserId;
                string[] idArray = objectIds.Split(new char[] { ',' });

                updateStatus(tenantId, idArray); //解绑用户所属租户以及公共租户

                result.Code = (int)ExceptionCode.Success;
                result.Message = "解绑成功！";
            }
            catch (Exception ex)
            {
                result.Code = (int)ExceptionCode.KnowError;
                result.Message = "用户解绑失败！" + ex.Message;
                AppConnectLogHelper.Error("用户解绑MRest接口异常:" + ex, ex);
            }

            return result;
        }

        public void updateStatus(int tenantId, string[] userMappingInfoId)
        {
            var metaobj = CloudDataHelper.GetMetaObject(tenantId, UserInfoMappingConstants.MetaName);
            var userMappingInfoList = CloudDataHelper.GetEntityListByIds(UserInfoMappingConstants.MetaName, tenantId, userMappingInfoId).ToList();
            if (userMappingInfoList != null && userMappingInfoList.Any())
            {
                foreach (var item in userMappingInfoList)
                {                   
                    item["Status"] = (int)BindStatusEnum.unbind;
                    CloudDataHelper.Update(metaobj,new List<ObjectData> { item });

                    //解绑公共租户数据
                    var filter = new BooleanFilter()
                         .Must(new TermFilter(UserInfoMappingConstants.UserInfo_StaffId, Convert.ToInt32(item["StaffId"])))
                         .Must(new TermFilter(UserInfoMappingConstants.UserInfo_SuiteKey, SystemInfo.SuiteKey))
                         .Must(new TermFilter(UserInfoMappingConstants.UserInfo_CorpId,Convert.ToString(item["CorpId"])))
                         .Must(new TermFilter(UserInfoMappingConstants.UserInfo_AppId,Convert.ToString(item["AppId"])))
                         .Must(new TermFilter(UserInfoMappingConstants.UserInfo_Status, (int)BindStatusEnum.bind));

                    var sysMetaobj = CloudDataHelper.GetMetaObject(SystemInfo.ISVSystemTenantId, UserInfoMappingConstants.MetaName);
                    var sysUserInfoList = CloudDataHelper.GetEntityAllList(UserInfoMappingConstants.MetaName, SystemInfo.ISVSystemTenantId, filter).ToList().FirstOrDefault();
                    sysUserInfoList["Status"] = (int)BindStatusEnum.unbind;
                    CloudDataHelper.Update(sysMetaobj, new List<ObjectData> { sysUserInfoList });
                }
            }
        }

        /// <summary>
        /// account 人员离职，删除后删除相应人员的绑定信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        public void UnBindInfo(int tenantId, int userId)
        {
            var filter = new BooleanFilter()
                .Must(new TermFilter(UserInfoMappingConstants.UserInfo_ISVTenantId, tenantId))
                .Must(new TermFilter(UserInfoMappingConstants.UserInfo_StaffId, userId))
                .Must(new TermFilter(UserInfoMappingConstants.UserInfo_SuiteKey, SystemInfo.SuiteKey));

            var metaobj = CloudDataHelper.GetMetaObject(tenantId, UserInfoMappingConstants.MetaName);
            var userInfoList = CloudDataHelper.GetEntityAllList(UserInfoMappingConstants.MetaName, tenantId, filter).ToList();
            if (userInfoList != null && userInfoList.Any())
            {
                userInfoList.ForEach(n =>
                {
                    n["Status"] = (int)BindStatusEnum.unbind;
                });
                CloudDataHelper.Update(metaobj, userInfoList);
            }

            var sysMetaObj = CloudDataHelper.GetMetaObject(SystemInfo.ISVSystemTenantId, UserInfoMappingConstants.MetaName);
            var sysUserInfoList = CloudDataHelper.GetEntityAllList(UserInfoMappingConstants.MetaName, SystemInfo.ISVSystemTenantId, filter).ToList();
            if (sysUserInfoList != null && sysUserInfoList.Any())
            {
                sysUserInfoList.ForEach(n =>
                {
                    n["Status"] = (int)BindStatusEnum.unbind;
                });
                CloudDataHelper.Update(sysMetaObj, sysUserInfoList);
            }

        }

    }
}

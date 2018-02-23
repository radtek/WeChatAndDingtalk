using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Infrastructure.Enums
{
    public enum AccountNotifyType
    {
        UploadDepartement = 10,
        AddDepartement = 11,
        UpdateDepartement = 12,
        AddStaff = 20,
        UpdateStaff = 21,
        UpdateStaffNoTita = 22,
        AddTenantInitFunction = 23,
        DelTenantInitFunction = 24,
        DisableUserStatus = 25,
        DisableStaffStatus = 26,
        DeleteStaff = 27
    }

    /// <summary>
    /// Account消息EmpBusinessType
    /// </summary>
    public enum EmpBusinessType
    {
        /// <summary>
        /// 入职
        /// </summary>
        Entry = 1,
        /// <summary>
        /// 试用期到期
        /// </summary>
        ProbationEnd = 2,
        /// <summary>
        /// 调动
        /// </summary>
        Transfer = 3,
        /// <summary>
        /// 转移
        /// </summary>
        Mobility = 4,

        /// <summary>
        /// 离职
        /// </summary>
        Dimission = 5,
        /// <summary>
        /// 退休
        /// </summary>
        Retire = 6,
        /// <summary>
        /// 返聘
        /// </summary>
        Reemploy = 7,
        /// <summary>
        /// 结束返聘
        /// </summary>
        ReemployEnd = 8,
        /// <summary>
        /// 亡故
        /// </summary>
        Dead = 9,
        /// <summary>
        /// 组织变更
        /// </summary>
        OrgChange = 10,
        /// <summary>
        /// 编辑员工
        /// </summary>
        Edit = 20,
        /// <summary>
        /// 删除员工
        /// </summary>
        Delete = 21,

        /// <summary>
        /// 未知类型
        /// </summary>
        UnKnowType = 99,
    }
}

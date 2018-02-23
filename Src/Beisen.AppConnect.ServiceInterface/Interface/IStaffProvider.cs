using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.WebModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beisen.UserFramework.Models;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface IStaffProvider
    {
        /// <summary>
        /// 校验用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool CheckUserNameExist(string userName);

        /// <summary>
        /// 创建员工
        /// </summary>
        /// <param name="info"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        int CreateStaff(RegisterUserInfo info, int operatorId = 0);

        /// <summary>
        /// 根据userName获取员工Id
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Dictionary<int, int> GetByUserName(string userName);
        Authentication Login(string userName, string password, out ResultModel res);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceInterface
{
    [ServiceContract]
    public interface IVerifyAppAccountProvider
    {
        /// <summary>
        ///  新增数据前，根据agentId以及AppSecret去重
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="agentId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        [OperationContract(Name = "VerifyAppAccount")]
        bool VerifyAppAccount(int tenantId, string agentId, string appSecret);

        /// <summary>
        /// 编辑（更新）数据前，根据agentId以及AppSecret去重
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="id"></param>
        /// <param name="agentId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        [OperationContract(Name = "EditVerifyAppAccount")]
        bool EditVerifyAppAccount(int tenantId, Guid id, string agentId, string appSecret);
    }
}

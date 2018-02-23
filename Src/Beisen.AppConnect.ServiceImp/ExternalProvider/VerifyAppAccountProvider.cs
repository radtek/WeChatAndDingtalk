using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Constants;
using Beisen.AppConnect.ServiceInterface;
using Beisen.SearchV3;
using Beisen.SearchV3.DSL.Filters;
using Beisen.SearchV3.DSL.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp
{
    public class VerifyAppAccountProvider:IVerifyAppAccountProvider
    {
        #region 单例

        private static readonly IVerifyAppAccountProvider _instance = new VerifyAppAccountProvider();
        public static IVerifyAppAccountProvider Instance
        {
            get { return _instance; }
        }

        private VerifyAppAccountProvider()
        {
        }
        #endregion      
        public bool VerifyAppAccount(int tenantId, string agentId, string appSecret)
        {
            var result = false;
            //添加去重条件：AgentId、AppSecret
            //思路：根据AgentId过滤多租赁，数据为空，继续根据AppSecret过滤多租赁，数据为空，则没有重复，继续之后的保存、更新操作
            var agentIdFilter = new BooleanFilter()
                .Must(new TermFilter(AppAccount.AppAccount_AgentId, agentId));
            var appAccountList = CloudDataHelper.GetEntityAllList(AppAccount.MetaName, tenantId, agentIdFilter).ToList();
            if (appAccountList.Count <= 0)
            {
                var appSecretFilter = new BooleanQuery()
                    .Must(new MatchQuery(AppAccount.AppAccount_AppSecret, appSecret));
                var appAccount = CloudDataHelper.GetEntityAllList(AppAccount.MetaName, tenantId,queryJson: ElasticSearchSerialization.Serialize(appSecretFilter)).ToList();
                if (appAccount.Count <= 0)
                {
                    result = true;
                }
            }

            return result;
        }

        public bool EditVerifyAppAccount(int tenantId,Guid id, string agentId, string appSecret)
        {
            var result = false;
            //添加去重条件：AgentId、AppSecret
            //思路：根据AgentId过滤多租赁，数据为空，继续根据AppSecret过滤多租赁，数据为空，则没有重复，继续之后的保存、更新操作
            var agentIdFilter = new BooleanFilter()
                .Must(new TermFilter(AppAccount.AppAccount_AgentId, agentId));
            var appAccountList = CloudDataHelper.GetEntityAllList(AppAccount.MetaName, tenantId, agentIdFilter).ToList();
            var appAccountId = appAccountList.Select(n => n.ID).ToList();
            if (appAccountList.Count <= 1&&appAccountId.Contains(id))
            {
                var appSecretFilter = new BooleanQuery()
                    .Must(new MatchQuery(AppAccount.AppAccount_AppSecret, appSecret));
                var appAccount = CloudDataHelper.GetEntityAllList(AppAccount.MetaName, tenantId, queryJson: ElasticSearchSerialization.Serialize(appSecretFilter)).ToList();
                var ids = appAccount.Select(n => n.ID).ToList();
                if (appAccount.Count <= 1&&ids.Contains(id))
                {
                    result = true;
                }
            }

            return result;
        }
    }
}

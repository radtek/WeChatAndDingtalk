using Beisen.MultiTenant.Model;
using Beisen.MultiTenant.Model.UIMetaDataV2;
using Beisen.MultiTenant.Model.UIMetaDataV2.TableList;
using Beisen.AppConnect.CloudClient.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Beisen.AppConnect.CloudClient.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var result = Beisen.AppConnect.CloudClient.AppAccount.GetAppAccountType();
        }

        [TestMethod]
        public void TestGetCorpId()
        {
            DataSourceContext dsContext = new DataSourceContext();
            string ss = "{'CorpList':[{'CorpName':'','CorpId':''}]}";
            var ss111 = JsonConvert.DeserializeObject<CorpsResult>(ss);
            var result = Corp.GetCorpId();
        }

        public TableListComponent Post_After(SearchCondition conditon, TableListComponent result)
        {
            var str = JsonConvert.SerializeObject(result);
            //logger.Debug("JSON888:" + str);
            //logger.Error("JSON888" + str);
            for (int i = 0; i < result.BizData.Count; i++)
            {
                FieldBizData newData = new FieldBizData();
                newData.Text = result.GetColumnBizData("_id", i).Text;
                newData.Value = result.GetColumnBizData("_id", i).Value;
                result.SetColumnBizData("AppConnect.AppAccount.AppAccountId", newData, i);
            }
            return result;
        }
    }
}

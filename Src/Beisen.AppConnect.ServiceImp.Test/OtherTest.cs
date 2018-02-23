using System;
using System.IO;
using Beisen.AppConnect.Infrastructure.RequestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using Beisen.Data;
using System.Linq;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.Common.Context;
using Beisen.AppConnect.Infrastructure;
using Beisen.MultiTenant.Model;
using Beisen.DynamicScript.SDK;
using Beisen.AppConnect.ServiceInterface;

namespace Beisen.AppConnect.ServiceImp.Test
{
    [TestClass]
    public class OtherTest
    {
        [TestMethod]
        public void TestLog()
        {
            var request = new RestRequest(Method.GET);
            var client = new RestClient();
            client.BaseUrl = new Uri("http://api.appconnect-dev.beisencorp.com/AppAccount/Type");
            var response = client.Execute(request);
            var jsonSerializer = new JsonSerializer();
            JsonReader reader = new JsonTextReader(new StringReader(response.Content));
            var json = jsonSerializer.Deserialize<JObject>(reader);

            foreach (var a in json["type_list"])
            {

            }
        }

        [TestMethod]
        public void TestMessage()
        {
            //var nonceStr = "nnnnn";
            //var timeStamp = ConvertToUnixTimeStamp(DateTime.Now);
            //var ticket = "EjC6Vo2oGAFF2cALckrPYZEYuuvmULHzuRsWqeXaEwIV43r0TBgR322Q0SacXDVrspcyA4igby7VK8M9QgiFqr";
            //var url = "http://appconnect-dev.beisencorp.com/menu/custom/id=cd8f22c0-695d-4064-a488-dd68b9e3dbe7";
            //var signature = SHA1Helper.ConverToSHA1Str(string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", ticket, nonceStr, timeStamp, url));



            try
            {
                var text = Beisen.Common.Encrypt.Cryption.EncryptStrMD5("~~~");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //var messageJson = "{\"touser\":\"oeJYSwN7X6pBUinvtRnF809xhEuo\",\"template_id\":\"N4jqfSPx61DhDWiexyACZPgKg-mznsXDD6_4Qti7df8\",\"url\":\"https://www.baidu.com\",\"data\":{\"first\": {\"value\":\"恭喜你购买成功！\",\"color\":\"#173177\"},\"keyword1\":{\"value\":\"南京市玄武区玄武大道699-32号途牛大厦一楼面试大厅（从东门石狮子门进）\",\"color\":\"#173177\"},\"keyword2\": {\"value\":\"39.8元\",\"color\":\"#173177\"},\"remark\":{\"value\":\"欢迎再次购买！\",\"color\":\"#173177\"}}}";


            //var model = SerializeHelper.Deserialize<JObject>(messageJson);

            //var to = "touser";
            //var touser = model["data"]["first"].ToString();

            //var noExit = model["aaa"];


            //dynamic model = Newtonsoft.Json.JsonConvert.DeserializeObject(messageJson);

            //var json0 = SerializeHelper.Serialize(model);

            //string data = model.Data.ToString();

            //var newModel = new TemplateModel();

            //newModel.ToUser = "oeJYSwN7X6pBUinvtRnF809xhEuo";
            //newModel.TemplateId = "N4jqfSPx61DhDWiexyACZPgKg-mznsXDD6_4Qti7df8";

            //var item = new List<Tuple<string, TemplateMessageFieldDataItem>>();
            //item.Add(new Tuple<string, TemplateMessageFieldDataItem>("first", new TemplateMessageFieldDataItem("恭喜你购买成功！")));
            //item.Add(new Tuple<string, TemplateMessageFieldDataItem>("keynote1", new TemplateMessageFieldDataItem("2014年9月22日")));
            //item.Add(new Tuple<string, TemplateMessageFieldDataItem>("remark", new TemplateMessageFieldDataItem("欢迎再次购买！")));

            //newModel.Data = item;

            //var json = SerializeHelper.Serialize(newModel);

        }

        public static long ConvertToUnixTimeStamp(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;            //除10000调整为13位
            return t;
        }
        [TestMethod]
        public void ESBTest()
        {
            var instance = ESBProxyV2.GetInstance<ITestProvider>("AppConnect");
            var a = instance.GetString();
        }
        [TestMethod]
        public void TestApi()
        {
            var url = "http://api.appconnect-test.beisencorp.com/QrCode/GenerateQrCode";
            var method = Method.POST;

            var result = Request.SendRequest(url, method);
        }

        [TestMethod]
        public void TestTime()
        {
            Random rd = new Random();
            int i = rd.Next();

        }


        /// <summary>
        /// 创建当前时间戳
        /// </summary>
        /// <returns></returns>
        private string CreateTimeStamp()
        {
            return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
        }

        /// <summary>    
        /// 将DateTime时间格式转换为Unix时间戳格式    
        /// </summary>    
        /// <param name="time">时间</param>    
        /// <returns>double</returns>    
        public long DateTimeToUnixTimeStamp(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000000;            //除10000调整为13位
            return t;
        }

        //[TestMethod]
        //public void DataSyncTool()
        //{
        //    var result = GetAllTenantIds();
        //}
        [TestMethod]
        public void GetAllTenantIds()
        {
            List<AppUserAccountInfo> appUserAccountInfos = new List<AppUserAccountInfo>();
            Database database = Database.GetDatabase("BeisenUserSecurity");
            SafeProcedure.ExecuteAndGetInstanceList(database, "[dbo].[AppUserAccount_GetAll]",
           delegate (IRecord record, int entity)
           {
               AppUserAccountInfo appUserAccountInfo = new AppUserAccountInfo();
               appUserAccountInfo.Id = record.GetOrDefault<int>("Id", 0);
               appUserAccountInfo.AppId = record.GetOrDefault<string>("AppId", "");
               appUserAccountInfo.OpenId = record.GetOrDefault<string>("OpenId", "");
               appUserAccountInfo.TenantId = record.GetOrDefault<int>("TenantId", 0);
               appUserAccountInfo.UserId = record.GetOrDefault<int>("UserId", 0);
               appUserAccountInfo.BeisenAccount = record.GetOrDefault<string>("BeisenAccount", "");
               appUserAccountInfo.TypeNotEnum = record.GetOrDefault<Int16>("Type", 0);
               appUserAccountInfo.StateNotEnum = record.GetOrDefault<Int16>("State", 0);
               appUserAccountInfo.ActivateTime = record.GetOrDefault<DateTime>("ActivateTime", DateTime.Now);
               appUserAccountInfo.CreateTime = record.GetOrDefault<DateTime>("CreateTime", DateTime.Now);
               appUserAccountInfo.UnbindTime = record.GetOrDefault<DateTime>("UnbindTime", DateTime.Now);
               appUserAccountInfo.MasterAccountId = record.GetOrDefault<int>("MasterAccountId", 0);
               appUserAccountInfos.Add(appUserAccountInfo);
           });
            var tenantIds = GetTenantId(appUserAccountInfos);
            tenantIds.ForEach(tenantId =>
            {
                AddCLoud(tenantId, appUserAccountInfos);

            });
        }
        private void AddCLoud(int tenantId, List<AppUserAccountInfo> appUserAccountInfos)
        {

            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = tenantId;
            ApplicationContext.Current.UserId = 100000;
            var metaObject = CloudDataHelper.GetMetaObject(tenantId, "AppConnect.AppUser");
            List<ObjectData> objectDatas = new List<ObjectData>();
            var appUserAccountInfos_tenantId = appUserAccountInfos.Where(w => w.TenantId == tenantId).ToList();
            if (appUserAccountInfos_tenantId != null && appUserAccountInfos_tenantId.Count > 0)
            {
                appUserAccountInfos_tenantId.ForEach(appUserAccountInfo =>
                {
                    ObjectData objectData = new ObjectData(metaObject);
                    objectData.ID = Guid.NewGuid();
                    objectData.CreatedBy = appUserAccountInfo.UserId;
                    objectData.CreatedTime = DateTime.Now;
                    objectData.ModifiedBy = appUserAccountInfo.UserId;
                    objectData.ModifiedTime = DateTime.Now;
                    objectData["StdIsDeleted"] = false;
                    objectData["TenantId"] = appUserAccountInfo.TenantId;
                    objectData["UserId"] = appUserAccountInfo.UserId;
                    objectData["UserEmail"] = appUserAccountInfo.BeisenAccount;
                    objectData["State"] = appUserAccountInfo.StateNotEnum;
                    objectData["ActivateTime"] = DateTime.Now;
                    objectData["OpenId"] = appUserAccountInfo.OpenId;
                    objectData["AppId"] = appUserAccountInfo.AppId;
                    objectData["SqlId"] = appUserAccountInfo.Id;
                    objectData["LoginType"] = appUserAccountInfo.TypeNotEnum;
                    objectData["SqlId"] = appUserAccountInfo.Id;
                    objectData["MasterAccountId"] = appUserAccountInfo.MasterAccountId;
                    objectDatas.Add(objectData);
                });
            }
            CloudDataHelper.Add(metaObject, objectDatas);
        }

        private List<int> GetTenantId(List<AppUserAccountInfo> appUserAccountInfos)
        {
            if (appUserAccountInfos != null && appUserAccountInfos.Count > 0)
            {
                var tenantIds = appUserAccountInfos.Select(s => s.TenantId).Distinct().ToList();
                return tenantIds;
            }
            else
            {
                return null;
            }
        }
    }
}

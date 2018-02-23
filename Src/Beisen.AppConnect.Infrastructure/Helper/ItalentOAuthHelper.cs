using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using Beisen.AppConnect.Infrastructure.Configuration;

namespace Beisen.AppConnect.Infrastructure.Helper
{
    public static class ItalentOAuthHelper
    {
        public static string TitaApiUrl = Beisen.Configuration.ConfigManager.AppSettings["TitaApiUrl"];
        public static string GetSginQuery(int tenantId, int userId, int titaAppId, int signType = 1)
        {
            var timeStamp = DateTimeHelper.ConvertToUnixTimeStamp(DateTime.Now);
            var signValue = GetSginV2(tenantId, userId, titaAppId, timeStamp, signType);

            return string.Format("tenant_id={0}&user_id={1}&app_id={2}&time_stamp={3}&sign={4}&sign_type={5}", tenantId, userId, titaAppId, timeStamp, signValue, signType);
        }

        public static string GetSgin(int tenantId, int userId, int titaAppId, long timeStamp, int signType = 1)
        {
            var appKey = AppConnectTitaAppHostMappingConfig.AppKeyCache[titaAppId];

            string signValue;
            switch (signType)
            {
                case 1:
                    signValue = string.Format("{0}{1}{2}{3}{4}{5}", tenantId, userId, titaAppId, appKey, timeStamp, signType);
                    break;
                case 2:
                    signValue = string.Format("{0}{1}{2}{3}{4}", tenantId, userId, appKey, timeStamp, signType);
                    break;
                default:
                    throw new ArgumentException();
            }

            var encrypt = Encoding.ASCII.GetBytes(signValue);
            var md5Csp = new MD5CryptoServiceProvider();
            var resEncrypt = md5Csp.ComputeHash(encrypt);
            var sBuilder = new StringBuilder();
            foreach (var t in resEncrypt)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static string GetSginV2(int tenantId, int userId, int titaAppId, long timeStamp, int signType = 1)
        {
            try
            {
                //调用tita获取sign信息
                string requestUrl = string.Format("{0}/api/v1/{1}/{2}/sign?app_id={3}&time_stamp={4}&sign_type={5}", TitaApiUrl, tenantId, userId, titaAppId, timeStamp, signType);
                AppConnectLogHelper.DebugFormat("GetSginV2-Url:{0}", requestUrl);
                string resultStr = HttpClientHelper.HttpGet(requestUrl);
                var resultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultModel>(resultStr);
                if (resultModel.Code != 0)
                {
                    AppConnectLogHelper.Error("GetSginV2-获取数据api失败");
                    return string.Empty;
                }
                return resultModel.Data.ObjData;
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.ErrorFormat("GetSginV2-获取数据api异常:{0}", ex.Message);
                return string.Empty;
            }
        }
    }

    [DataContract]
    public class ApiResultModel
    {
        [DataMember(Name = "code")]
        public int Code { get; set; }
        [DataMember(Name = "data")]
        public ApiEmptyResultModel Data { get; set; }
    }
    [DataContract]
    public class ApiEmptyResultModel
    {
        [DataMember(Name = "objData")]
        public string ObjData { get; set; }
    }
}

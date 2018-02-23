using Beisen.AppConnectISV.BusinessCore.Tita.Model;
using Beisen.AppConnectISV.Infrastructure;
using Beisen.AppConnectISV.Infrastructure.RequestUtility;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Beisen.AppConnectISV.BusinessCore
{
    /// <summary>
    /// Tita Sign
    /// </summary>
    public class TitaSign
    {
        public static string TitaApiUrl = Beisen.Configuration.ConfigManager.AppSettings["TitaApiUrl"];

        #region Singleton 
        static readonly TitaSign _Instance = new TitaSign();
        public static TitaSign Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion

        public string GetSgin(int tenantId, int userId, int appId, string appkey, int signType = 1)
        {
            var timeStamp = DateTimeCryptography.ConvertToUnixTimeStamp(DateTime.Now);
            var signValue = GetSgin(tenantId, userId, appId, appkey, timeStamp, signType);
            return string.Format("tenant_id={0}&user_id={1}&app_id={2}&time_stamp={3}&sign={4}&sign_type={5}", tenantId, userId, appId, timeStamp, signValue, signType);
        }

        private static string GetSgin(int tenantId, int userId, int appId, string appKey, long timeStamp, int signType = 1)
        {

            string signValue;
            switch (signType)
            {
                case 1:
                    signValue = string.Format("{0}{1}{2}{3}{4}{5}", tenantId, userId, appId, appKey, timeStamp, signType);
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


        //public string GetSginV2(int tenantId, int userId, int titaAppId, int signType = 1)
        //{
        //    var timeStamp = DateTimeCryptography.ConvertToUnixTimeStamp(DateTime.Now);
        //    var signValue = GetSginV2(tenantId, userId, titaAppId, timeStamp, signType);
        //    return string.Format("tenant_id={0}&user_id={1}&app_id={2}&time_stamp={3}&sign={4}&sign_type={5}", tenantId, userId, titaAppId, timeStamp, signValue, signType);
        //}

        public string GetSginV2(int tenantId, int userId, int titaAppId, long timeStamp, int signType)
        {
            //调用tita获取sign信息
            string sginV2Url = string.Format("{0}/api/v1/{1}/{2}/sign?app_id={3}&time_stamp={4}&sign_type={5}", TitaApiUrl, tenantId, userId, titaAppId, timeStamp, signType);
            if (string.IsNullOrWhiteSpace(sginV2Url))
            {
                throw new Exception(string.Format("未获取到SginV2Url;"));
            }
            var resultModel = HttpClientTool.HttpGet<GetSign_Result>(sginV2Url);
            LogHelper.Instance.Dump("GetSginV2:" + JsonConvert.SerializeObject(resultModel));
            if (resultModel.Code != 0 || string.IsNullOrWhiteSpace(resultModel.Data.ObjData))
            {
                LogHelper.Instance.Dump(string.Format("TanantId:{0},UserId:{1},TitaAppId:{2},SiginType:{3}", tenantId, userId, titaAppId, signType), LogType.Debug);
                LogHelper.Instance.Dump(string.Format("TanantId:{0},UserId:{1},TitaAppId:{2},SiginType:{3}", tenantId, userId, titaAppId, signType), LogType.Error);
                throw new Exception(string.Format("未获取到Tita Sign Info;"));
            }
            return resultModel.Data.ObjData;
        }
    }
}

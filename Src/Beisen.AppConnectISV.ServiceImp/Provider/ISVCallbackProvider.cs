using Beisen.AppConnectISV.BusinessCore;
using Beisen.AppConnectISV.BusinessCore.DingDing;
using Beisen.AppConnectISV.Infrastructure;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Beisen.AppConnectISV.ServiceImp
{
    public class ISVCallbackProvider
    {
        #region Singleton 
        static readonly ISVCallbackProvider _Instance = new ISVCallbackProvider();
        public static ISVCallbackProvider Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion

        public HttpResponseMessage ISVCallback(string httpBodyContent, string currentSignature, string currentTimestamp, string currentNonce)
        {
            try
            {
                #region 获取回调URL里面的参数
                //url中的签名
                string msgSignature = currentSignature;
                //url中的时间戳
                string timeStamp = currentTimestamp;
                //url中的随机字符串
                string nonce = currentNonce;
                //post数据包数据中的加密数据
                string encryptStr = httpBodyContent;
                #endregion


                //构造DingTalkCrypt
                DingTalkCrypt dingTalk = new DingTalkCrypt(ISVInfo.Token, ISVInfo.EncodingAesKey, ISVInfo.SuiteKey);
                string plainText = "";
                //Post里面的数据进行解密，即plainText的值
                dingTalk.DecryptMsg(msgSignature, timeStamp, nonce, encryptStr, ref plainText);
                Hashtable tb = (Hashtable)JsonConvert.DeserializeObject(plainText, typeof(Hashtable));
                if (tb == null)
                {
                    LogHelper.Instance.Dump("Post数据{0}解密后值为空!" + plainText, LogType.Error);
                    throw new Exception("解密失败!");
                }
                string eventType = tb["EventType"].ToString();
                string res = "failure";
                LogHelper.Instance.Dump("Message--:" + plainText, LogType.Debug, null, eventType);
                switch (eventType)
                {
                    case "suite_ticket"://定时推送Ticket
                        string suiteKey = tb["SuiteKey"].ToString();
                        var suiteTicket = tb["SuiteTicket"].ToString();
                        ISVCallbackCoreProvider.Instance.SaveTicket(suiteTicket);
                        res = "success";
                        break;
                    case "tmp_auth_code"://企业授权：钉钉推送过来的临时授权码
                        string suiteKey_TmpAuthCode = tb["SuiteKey"].ToString();
                        var redisTmpAuthCode_Value = tb["AuthCode"].ToString();
                        ISVCallbackCoreProvider.Instance.Activate_Suite(redisTmpAuthCode_Value);
                        res = "success";
                        break;
                    case "suite_relieve":
                        string authCorpId = tb["AuthCorpId"].ToString();
                        ISVCallbackCoreProvider.Instance.Suite_Relieve(authCorpId);
                        res = "success";
                        break;
                    case "change_auth":// do something;
                        break;
                    case "check_create_suite_url":
                        //Create_回调函数校验
                        res = tb["Random"].ToString();
                        break;
                    case "check_update_suite_url":
                        //Update_回调函数校验
                        res = tb["Random"].ToString();
                        break;
                }
                timeStamp = Helper.GetTimeStamp().ToString();
                string encrypt = "";
                string signature = "";
                dingTalk = new DingTalkCrypt(ISVInfo.Token, ISVInfo.EncodingAesKey, ISVInfo.SuiteKey);
                dingTalk.EncryptMsg(res, timeStamp, nonce, ref encrypt, ref signature);
                Hashtable jsonMap = new Hashtable
                {
                    {"msg_signature", signature},
                    {"encrypt", encrypt},
                    {"timeStamp", timeStamp},
                    {"nonce", nonce}
                };
                string result = JsonConvert.SerializeObject(jsonMap);
                return Json.Instance.toJson(result);
            }
            catch (Exception ex)
            {
                HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent("请求失败!", Encoding.GetEncoding("UTF-8"), "application/json") };
                LogHelper.Instance.Error(DateTime.Now + ex.Message, ex);
                return result;
            }
        }

    }
}

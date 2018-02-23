using Beisen.AppConnectISV.Infrastructure;
using Beisen.AppConnectISV.Model.BusinessModel;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore.DingDing
{
    public class DingDingMethod
    {
        #region Singleton 
        static readonly DingDingMethod _Instance = new DingDingMethod();
        public static DingDingMethod Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion

        static object _lock = new object();

        #region Activate_Suite
        public void Activate_Suite(string corpId, string permanent_code, string suiteAccessToken)
        {
            var activate_SuiteUrl = string.Format(DingTalkApiUrl.Activate_Suite, suiteAccessToken);
            Activate_Suite_Post activate_Suite_Post = new Activate_Suite_Post();
            activate_Suite_Post.Auth_Corpid = corpId;
            activate_Suite_Post.Permanent_Code = permanent_code;
            activate_Suite_Post.Suite_Key = ISVInfo.SuiteKey;
            var dingTalkBase_Result = RestClientTool.SendRequest<DingTalkBase>(activate_SuiteUrl, Method.POST, activate_Suite_Post);
            if (dingTalkBase_Result.ErrCode != 0)
            {
                throw new Exception(string.Format("Activate_Suite失败!ErrCode:{0},ErrMsg{1}", dingTalkBase_Result.ErrCode, dingTalkBase_Result.ErrMsg));
            }
            LogHelper.Instance.Dump(string.Format("Activate_Suite Method:Activate_Suite:{0}", JsonConvert.SerializeObject(dingTalkBase_Result)), LogType.Debug);
        }
        #endregion

        #region GetSign
        public string GetSign(string ticket, string nonce, double timeStamp, string url)
        {
            var sign = ConverToSHA1(string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", ticket, nonce, timeStamp, url)).ToLower();
            return sign;
        }
        private string ConverToSHA1(string str)
        {
            var sha = new SHA1CryptoServiceProvider();
            var enc = new ASCIIEncoding();
            var dataToHash = enc.GetBytes(str);
            var dataHashed = sha.ComputeHash(dataToHash);
            var hash = BitConverter.ToString(dataHashed).Replace("-", "");

            return hash;
        }
        public double DingTalkTimeStamp()
        {
            //UnixTimeStamp
            DateTime time = DateTime.Now;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            double t = (time.Ticks - startTime.Ticks) / 10000000;
            return t;
        }
        public string Nonce()
        {
            return Guid.NewGuid().ToString();
        }
        #endregion

        #region Cookie Method
        public void GetDingTaklUserInfo(string corpId, string code)
        {
            var corpAccessToken = DingDingMethod.Instance.GetCorpAccessToken(corpId);
            var userInfoUrl = DingTalkApiUrl.Instance.GetUserInfo(corpAccessToken, code);
            var userInfo = RestClientTool.SendRequest<UserInfo_Result>(userInfoUrl, Method.GET);
            if (userInfo.ErrCode != 0)
            {
                throw new Exception(string.Format("GetUserInfo失败!ErrCode:{0},ErrMsg{1}", userInfo.ErrCode, userInfo.ErrMsg));
            }
            DingTalkUserInfo dingTalkUserInfo = new DingTalkUserInfo();
            dingTalkUserInfo.DingTalkUserId = userInfo.Userid;
            dingTalkUserInfo.CorpId = corpId;
            dingTalkUserInfo.DeviceId = userInfo.DeviceId;
            dingTalkUserInfo.Is_sys = userInfo.Is_sys;
            dingTalkUserInfo.Sys_level = userInfo.Sys_level;
            Cookie.SetDingTalkUserInfo(dingTalkUserInfo);
        }

        #endregion

        #region Redis Method

        public string GetTicket()
        {
            var suite_Ticket = Redis.Instance.GetRedis(RedisKeyInfo.SuiteTicket);
            if (string.IsNullOrWhiteSpace(suite_Ticket))
            {
                throw new Exception(string.Format("未获取到当前Ticket,RedisKey{0}", RedisKeyInfo.SuiteTicket));
            }
            return suite_Ticket;
        }

        #region  PermanentCode
        public string GetPermanentCode(string corpId)
        {
            var redisKey = RedisKeyInfo.CorpPermanentCode(corpId);
            var permanentCode_Result = Redis.Instance.GetRedis<PermanentCode_Result>(redisKey);
            if (permanentCode_Result == null)
            {
                throw new Exception(string.Format("企业未授权RedisKey{0}!", redisKey));
            }
            return permanentCode_Result.Permanent_Code;
        }
        public PermanentCode_Result GetPermanent_Code(string tmp_auth_code, string suiteAccessToken)
        {
            lock (_lock)
            {
                //var ticket = Redis.Instance.GetRedis(redisKey);
                //if (string.IsNullOrWhiteSpace(ticket))
                //{
                //}
                var permanent_CodeUrl = string.Format(DingTalkApiUrl.Permanent_Code, suiteAccessToken);
                PermanentCode_Post permanentCode_Post = new PermanentCode_Post();
                permanentCode_Post.Tmp_auth_code = tmp_auth_code;
                var permanentCode_Result = RestClientTool.SendRequest<PermanentCode_Result>(permanent_CodeUrl, Method.POST, permanentCode_Post);
                LogHelper.Instance.Dump("PermanentCode_Result返回值:" + JsonConvert.SerializeObject(permanentCode_Result));
                if (permanentCode_Result.ErrCode != 0)
                {
                    throw new Exception(string.Format("GetSuiteAccessToken失败!ErrCode:{0},ErrMsg{1}", permanentCode_Result.ErrCode, permanentCode_Result.ErrMsg));
                }
                var redisKey = RedisKeyInfo.CorpPermanentCode(permanentCode_Result.AuthCorpInfo.Corpid);
                Redis.Instance.SetRedis(redisKey, JsonConvert.SerializeObject(permanentCode_Result));
                return permanentCode_Result;
            }
        }
        public PermanentCode_Result GetPermanentCodeInfo(string corpId)
        {
            var redisKey = RedisKeyInfo.CorpPermanentCode(corpId);
            var permanentCode_Result = Redis.Instance.GetRedis<PermanentCode_Result>(redisKey);
            if (permanentCode_Result == null)
            {
                throw new Exception(string.Format("企业未授权RedisKey{0}!", redisKey));
            }
            return permanentCode_Result;
        }
        #endregion



        public AuthCorpInfo_Result GetAuthCorpInfo(string corpId)
        {
            var redisKey = RedisKeyInfo.AuthCorpInfo(corpId);
            var authCorpInfo = Redis.Instance.GetRedis<AuthCorpInfo_Result>(redisKey);
            if (authCorpInfo == null)
            {
                var suiteAccessToken = GetSuiteAccessToken();
                var authCorpInfoUrl = DingTalkApiUrl.Instance.GetAuthCorpInfo(suiteAccessToken);
                AuthCorpInfo_Post authCorpInfo_Post = new AuthCorpInfo_Post();
                authCorpInfo_Post.Suite_Key = ISVInfo.SuiteKey;
                authCorpInfo_Post.Auth_Corpid = corpId;
                authCorpInfo = RestClientTool.SendRequest<AuthCorpInfo_Result>(authCorpInfoUrl, Method.POST, authCorpInfo_Post);
                if (authCorpInfo.ErrCode != 0)
                {
                    throw new Exception(string.Format("GetAuthCorpInfo失败!ErrCode:{0},ErrMsg{1}", authCorpInfo.ErrCode, authCorpInfo.ErrMsg));
                }
                Redis.Instance.SetRedis(redisKey, JsonConvert.SerializeObject(authCorpInfo));
            }
            LogHelper.Instance.Dump(string.Format("GetAuthCorpInfo，corpId:{0},appId{1}", corpId, JsonConvert.SerializeObject(authCorpInfo)), LogType.Debug);
            return authCorpInfo;
        }

        public string GetAgentId(string corpId, string appId)
        {
            string agentId = string.Empty;
            var authCorpInfo_Result = GetAuthCorpInfo(corpId);
            var agentInfo = authCorpInfo_Result.auth_info.agent.Where(w => w.appid == appId).FirstOrDefault();
            if (agentInfo == null)
            {
                throw new Exception("未获取企业授权信息!");
            }
            return agentInfo.agentid;
        }

        public string GetCorpAccessToken(string corpId)
        {
            var suiteAccessToken = GetSuiteAccessToken();
            var corpAccessToken = Redis.Instance.GetRedis(RedisKeyInfo.CorpToken(corpId));
            if (string.IsNullOrWhiteSpace(corpAccessToken))
            {
                lock (_lock)
                {
                    var corpTokenUrl = DingTalkApiUrl.Instance.GetCorpToken(suiteAccessToken);
                    Corp_Token_Post corp_Token_Post = new Corp_Token_Post();
                    corp_Token_Post.Auth_Corpid = corpId;
                    corp_Token_Post.Permanent_Code = GetPermanentCode(corpId);
                    var corpTokenResult = RestClientTool.SendRequest<Corp_Token_Result>(corpTokenUrl, Method.POST, corp_Token_Post);
                    if (corpTokenResult.ErrCode != 0)
                    {
                        throw new Exception(string.Format("GetSuiteAccessToken失败!ErrCode:{0},ErrMsg{1}", corpTokenResult.ErrCode, corpTokenResult.ErrMsg));
                    }
                    corpAccessToken = corpTokenResult.Access_Token;
                    Redis.Instance.SetRedis(RedisKeyInfo.CorpToken(corpId), corpTokenResult.Access_Token, RedisInfo.Expires);
                }
            }
            return corpAccessToken;
        }
        public string GetJsApiTicket(string corpId, string corpAccessToken)
        {
            var redisKey = RedisKeyInfo.CorpJsApiTicket(corpId);
            var ticket = Redis.Instance.GetRedis(redisKey);
            if (string.IsNullOrWhiteSpace(ticket))
            {
                lock (_lock)
                {
                    var jsApiTicketUrl = DingTalkApiUrl.Instance.GetJsApiTicket(corpAccessToken);
                    var jsApiTicket = RestClientTool.SendRequest<JsApiTicket_Result>(jsApiTicketUrl, Method.GET);
                    if (jsApiTicket.ErrCode != 0)
                    {
                        throw new Exception(string.Format("GetJsApiTicket失败!ErrCode:{0},ErrMsg{1}", jsApiTicket.ErrCode, jsApiTicket.ErrMsg));
                    }
                    ticket = jsApiTicket.Ticket;
                    Redis.Instance.SetRedis(RedisKeyInfo.CorpJsApiTicket(corpId), jsApiTicket.Ticket, RedisInfo.Expires);
                }
            }
            LogHelper.Instance.Dump(string.Format("GetJsApiTicket:CorpID{0},corpAccessToken{1}", ticket, corpAccessToken));
            return ticket;
        }
        #endregion

        #region DingTalk Common Method
        public string GetSuiteAccessToken()
        {
            var suite_Ticket = GetTicket();
            var suiteAccessToken = Redis.Instance.GetRedis(RedisKeyInfo.SuiteAccessToken);
            if (string.IsNullOrWhiteSpace(suiteAccessToken))
            {
                lock (_lock)
                {
                    SuiteAccessToken_Post suiteAccessToken_Post = new SuiteAccessToken_Post();
                    suiteAccessToken_Post.Suite_Key = ISVInfo.SuiteKey;
                    suiteAccessToken_Post.Suite_Secret = ISVInfo.SuitSecret;
                    suiteAccessToken_Post.Suite_Ticket = suite_Ticket;
                    var suiteAccessToken_Result = RestClientTool.SendRequest<SuiteAccessToken_Result>(DingTalkApiUrl.SuiteAccessToken, Method.POST, suiteAccessToken_Post);
                    if (suiteAccessToken_Result.ErrCode != 0)
                    {
                        throw new Exception(string.Format("GetSuiteAccessToken失败!ErrCode:{0},ErrMsg{1}", suiteAccessToken_Result.ErrCode, suiteAccessToken_Result.ErrMsg));
                    }
                    suiteAccessToken = suiteAccessToken_Result.Suite_Access_Token;
                    Redis.Instance.SetRedis(RedisKeyInfo.SuiteAccessToken, suiteAccessToken_Result.Suite_Access_Token, RedisInfo.Expires);
                }
            }
            return suiteAccessToken;
        }
        #endregion

        #region SendMessage
        public SendMessage_Result SendOAMessage(string corpId, string agentid, string fromUser, string toUser, string redirectUrlMobil, string redirectUrlPC, string title, string description, string imageUrl = null)
        {
            var corpAccessToken = GetCorpAccessToken(corpId);
            var messageInfoUrl = DingTalkApiUrl.Instance.GetSendMessage(corpAccessToken);
            var data = new
            {
                touser = toUser,
                toparty = "",
                agentid = agentid,
                msgtype = "oa",
                oa = new
                {
                    message_url = redirectUrlMobil,//客户端点击消息时跳转到的H5地址
                    pc_message_url = redirectUrlPC,
                    body = new  //消息体
                    {
                        title = title, //消息体的标题
                        content = description,
                        image = imageUrl,
                        //  author = fromUser //发送人UserId
                    }
                }
            };
            var messageInfo = RestClientTool.SendRequest<SendMessage_Result>(messageInfoUrl, Method.POST, data);
            return messageInfo;
        }
        #endregion
    }
}

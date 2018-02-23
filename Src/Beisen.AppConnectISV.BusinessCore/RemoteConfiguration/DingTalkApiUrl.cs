using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.BusinessCore
{
    public class DingTalkApiUrl
    {
        #region Singleton 
        static readonly DingTalkApiUrl _Instance = new DingTalkApiUrl();
        public static DingTalkApiUrl Instance
        {
            get
            {
                return _Instance;
            }
        }
        #endregion

        public static string SuiteAccessToken
        {
            get
            {
                var suiteAccessTokenUrl = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.DingDingApi, AppconnectConfigConst.DingDingApi_SuiteAccessToken);
                if (string.IsNullOrWhiteSpace(suiteAccessTokenUrl))
                {
                    throw new Exception(string.Format("未在远程配置中获取SuiteAccessTokenUrl{0}", suiteAccessTokenUrl));
                }
                return suiteAccessTokenUrl;
            }
        }

        public static string Permanent_Code
        {
            get
            {
                var permanentCodeUrl = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.DingDingApi, AppconnectConfigConst.DingDingApi_PermanentCode);
                if (string.IsNullOrWhiteSpace(permanentCodeUrl))
                {
                    throw new Exception(string.Format("未在远程配置中获取PermanentCodeUrl{0}", permanentCodeUrl));
                }
                return permanentCodeUrl;
            }
        }

        public static string Activate_Suite
        {
            get
            {
                var activateSuiteUrl = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.DingDingApi, AppconnectConfigConst.DingDingApi_ActivateSuite);
                if (string.IsNullOrWhiteSpace(activateSuiteUrl))
                {
                    throw new Exception(string.Format("未在远程配置中获取ActivateSuiteUrl{0}", activateSuiteUrl));
                }
                return activateSuiteUrl;
            }
        }
        private static string Corp_Token
        {
            get
            {
                var corpTokenUrl = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.DingDingApi, AppconnectConfigConst.DingDingApi_CorpToken);
                if (string.IsNullOrWhiteSpace(corpTokenUrl))
                {
                    throw new Exception(string.Format("未在远程配置中获取CorpTokenUrl{0}", corpTokenUrl));
                }
                return corpTokenUrl;
            }
        }
        private static string JsApiTicket
        {
            get
            {
                var corpTokenUrl = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.DingDingApi, AppconnectConfigConst.DingDingApi_JsApiTicket);
                if (string.IsNullOrWhiteSpace(corpTokenUrl))
                {
                    throw new Exception(string.Format("未在远程配置中获取JsApiTicketUrl{0}", corpTokenUrl));
                }
                return corpTokenUrl;
            }
        }
        private static string AuthCorpInfo
        {
            get
            {
                var authCorpInfoUrl = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.DingDingApi, AppconnectConfigConst.DingDingApi_AuthCorpInfo);
                if (string.IsNullOrWhiteSpace(authCorpInfoUrl))
                {
                    throw new Exception(string.Format("未在远程配置中获取AuthCorpInfo{0}", authCorpInfoUrl));
                }
                return authCorpInfoUrl;
            }
        }
        private static string UserInfo
        {
            get
            {
                var userInfoUrl = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.DingDingApi, AppconnectConfigConst.DingDingApi_UserInfo);
                if (string.IsNullOrWhiteSpace(userInfoUrl))
                {
                    throw new Exception(string.Format("未在远程配置中获取UserInfoUrl{0}", userInfoUrl));
                }
                return userInfoUrl;
            }
        }

        private static string Message
        {
            get
            {
                var messageUrl = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.DingDingApi, AppconnectConfigConst.DingDingApi_Message);
                if (string.IsNullOrWhiteSpace(messageUrl))
                {
                    throw new Exception(string.Format("未在远程配置中获取MessageUrl{0}", messageUrl));
                }
                return messageUrl;
            }
        }

        private static string AsyncMessage
        {
            get
            {
                var url = AppconnectConfig.GetAppconnectConfigInfo<string>(AppconnectConfigConst.DingDingApi, AppconnectConfigConst.DingDingApi_AsyncMessage);
                if (string.IsNullOrWhiteSpace(url))
                {
                    throw new Exception(string.Format("未在远程配置中获取Url{0}", url));
                }
                return url;
            }
        }

        public string GetCorpToken(string suiteAccessToken)
        {
            var corpTokenUrl = string.Format(Corp_Token, suiteAccessToken);
            return corpTokenUrl;
        }
        public string GetJsApiTicket(string corpAccessToken)
        {
            var jsApiTicket = string.Format(JsApiTicket, corpAccessToken);
            return jsApiTicket;
        }
        public string GetAuthCorpInfo(string suiteAccessToken)
        {
            var authCorpInfo = string.Format(AuthCorpInfo, suiteAccessToken);
            return authCorpInfo;
        }
        public string GetUserInfo(string suiteAccessToken, string code)
        {
            var userInfo = string.Format(UserInfo, suiteAccessToken, code);
            return userInfo;
        }

        public string GetSendMessage(string corpAccessToken)
        {
            var message = string.Format(Message, corpAccessToken);
            return message;
        }

        public string GetAsyncMessage(string suiteAccessToken, string timestamp)
        {
            var asyncMessage = string.Format(AsyncMessage, suiteAccessToken, timestamp);
            return asyncMessage;
        }
    }
}

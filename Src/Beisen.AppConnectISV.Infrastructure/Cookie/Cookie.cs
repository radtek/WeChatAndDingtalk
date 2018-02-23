using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;

namespace Beisen.AppConnectISV.Infrastructure
{
    public class Cookie
    {
        private const string _isvLoginInfo = "ISVLoginInfo";
        private const string _dingTalkUserInfo = "DingTalkUserInfo";
        private const string _isvAuthInfo = "ISVAuthInfo";
        #region  登录授权信息
        public static ISVAuthInfo GetISVAuthInfo()
        {
            var loginInfo = new ISVAuthInfo();
            var LoginInfoKey = HttpContext.Current.Request.Cookies.Get(_isvAuthInfo);
            if (LoginInfoKey != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[_isvAuthInfo];
                if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                {
                    var value = Cryptography.AESDecrypt(cookie.Value);
                    loginInfo = JsonConvert.DeserializeObject<ISVAuthInfo>(value);
                }
            }

            return loginInfo;
        }
        public static void SetISVAuthInfo(ISVAuthInfo loginInfo)
        {
            if (loginInfo != null)
            {
                var cookieValue = Cryptography.AESEncrypt(JsonConvert.SerializeObject(loginInfo));
                var cookie = new HttpCookie(_isvAuthInfo, cookieValue)
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Request.Cookies.Remove(_isvAuthInfo);
                HttpContext.Current.Request.Cookies.Add(cookie);
            }
        }
        public static void ClearISVAuthInfo()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[_isvAuthInfo];
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                cookie.Expires = DateTime.Today.AddDays(-10);
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Request.Cookies.Remove(_isvAuthInfo);
            }
        }

        #endregion
        #region 登录人员信息
        public static ISVLoginInfo GetISVLoginInfo()
        {
            var userCookie = new ISVLoginInfo();
            var isvLoginInfoKey = HttpContext.Current.Request.Cookies.Get(_isvLoginInfo);
            if (isvLoginInfoKey != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[_isvLoginInfo];
                if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                {
                    var value = Cryptography.AESDecrypt(cookie.Value);
                    userCookie = JsonConvert.DeserializeObject<ISVLoginInfo>(value);
                }
            }
            return userCookie;
        }
        public static void SetISVLoginInfo(ISVLoginInfo userCookie)
        {
            if (userCookie != null)
            {
                var cookieValue = Cryptography.AESEncrypt(JsonConvert.SerializeObject(userCookie));
                var cookie = new HttpCookie(_isvLoginInfo, cookieValue)
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Request.Cookies.Remove(_isvLoginInfo);
                HttpContext.Current.Request.Cookies.Add(cookie);
            }
        }
        public static void ClearISVLoginInfo()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[_isvLoginInfo];
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                cookie.Expires = DateTime.Today.AddDays(-10);
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Request.Cookies.Remove(_isvLoginInfo);
            }
        }
        #endregion
        #region DingTalkUserInfo
        public static DingTalkUserInfo GetDingTalkUserInfo()
        {
            var userCookie = new DingTalkUserInfo();
            var dingTalkUserInfoKey = HttpContext.Current.Request.Cookies.Get(_dingTalkUserInfo);
            if (dingTalkUserInfoKey != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[_dingTalkUserInfo];
                if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                {
                    var value = Cryptography.AESDecrypt(cookie.Value);
                    userCookie = JsonConvert.DeserializeObject<DingTalkUserInfo>(value);
                }
            }
            return userCookie;
        }
        public static void SetDingTalkUserInfo(DingTalkUserInfo dingTalkUserInfo)
        {
            if (dingTalkUserInfo != null)
            {
                var cookieValue = Cryptography.AESEncrypt(JsonConvert.SerializeObject(dingTalkUserInfo));
                var cookie = new HttpCookie(_dingTalkUserInfo, cookieValue)
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Request.Cookies.Remove(_dingTalkUserInfo);
                HttpContext.Current.Request.Cookies.Add(cookie);
            }
        }
        public static void ClearDingTalkUserInfo()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[_dingTalkUserInfo];
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                cookie.Expires = DateTime.Today.AddDays(-10);
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Request.Cookies.Remove(_dingTalkUserInfo);
            }
        }
        #endregion

    }

    public class ISVLoginInfo
    {
        /// <summary>
        /// TenantId
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// TitaUserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 账户类型：1代表测评项目创建的受测者 2代表外部用户 3代表员工
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// ObjectId
        /// </summary>
        public string SystemObjectId { get; set; }
    }
    public class DingTalkUserInfo
    {
        /// <summary>
        /// DingTalkUserId
        /// </summary>
        public string DingTalkUserId { get; set; }
        /// <summary>
        ///手机设备号,由钉钉在安装时随机产生
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// 级别，0：非管理员 1：超级管理员（主管理员） 2：普通管理员（子管理员） 100：老板
        /// </summary>
        public string Sys_level { get; set; }
        /// <summary>
        /// 是否是管理员
        /// </summary>
        public string Is_sys { get; set; }
        /// <summary>
        /// CorpId
        /// </summary>
        public string CorpId { get; set; }
    }
    public class ISVAuthInfo
    {
        /// <summary>
        /// 判断是否登录，因为多租赁查询有延迟，所以使用了这个标识
        /// </summary>
        public bool ISLogin { get; set; }

        /// <summary>
        /// CorpId
        /// </summary>
        public string CorpId { get; set; }

        /// <summary>
        /// AppId
        /// </summary>
        public string AppId
        {
            get; set;
        }
        /// <summary>
        /// BusinessRedirectUrl
        /// </summary>
        public string BusinessRedirectUrl { get; set; }
        /// <summary>
        /// 授权URL
        /// </summary>
        public string AuthRedirectUrl { get; set; }
        /// <summary>
        /// TitaAppId
        /// </summary>
        public int TitaAppId { get; set; }
        /// <summary>
        /// 登录类型
        /// </summary>
        public int LoginType { get; set; }
    }
}

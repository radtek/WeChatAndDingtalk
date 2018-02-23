using System;
using System.Collections.Generic;
using System.Web;

namespace Beisen.AppConnect.Infrastructure.Helper
{
    public class CookieHelper
    {
        private const string _cookieName = "BsAppConnect";

        private const string _cookieState = "State";

        public static UserCookie GetCookie()
        {
            var userCookie = new UserCookie();
            HttpCookie cookie = HttpContext.Current.Request.Cookies[_cookieName];
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                var value = CryptographyHelper.AESDecrypt(cookie.Value);
                userCookie = SerializeHelper.Deserialize<UserCookie>(value);
            }
            return userCookie;
        }

        public static void SetCookie(UserCookie userCookie)
        {
            if (userCookie != null)
            {
                var cookieValue = CryptographyHelper.AESEncrypt(SerializeHelper.Serialize(userCookie));
                var cookie = new HttpCookie(_cookieName, cookieValue)
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Request.Cookies.Remove(_cookieName);
                HttpContext.Current.Request.Cookies.Add(cookie);
            }
        }

        public static string GetOpenId(UserCookie userCookie, string apppId)
        {
            if (userCookie == null || userCookie.OpenIds == null || !userCookie.OpenIds.ContainsKey(apppId))
            {
                return null;
            }
            return userCookie.OpenIds[apppId];
        }

        public static void ClearCookie()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[_cookieName];
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                cookie.Expires = DateTime.Today.AddDays(-10);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void SetState(string state)
        {
            if (state != null)
            {
                var cookieValue = CryptographyHelper.AESEncrypt(state);
                var cookie = new HttpCookie(_cookieState, cookieValue)
                {
                    Expires = DateTime.Now.AddMinutes(10),
                    HttpOnly = true
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static string GetState()
        {
            string stateCookie = null;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[_cookieState];
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                stateCookie = CryptographyHelper.AESDecrypt(cookie.Value);
            }
            return stateCookie;
        }
    }

    public class UserCookie
    {
        public string AccountAppId { get; set; }

        public string AccountOpenId { get; set; }

        public int AccountUserId { get; set; }

        public Dictionary<string, string> OpenIds { get; set; }
    }
}

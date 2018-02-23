using Beisen.AppConnectISV.Model;
using System;
using System.Web;
using System.Web.Mvc;
using Beisen.AppConnectISV.Infrastructure;
using Beisen.AppConnectISV.ServiceImp;
using Newtonsoft.Json;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnectISV.Model.BusinessEnum;

namespace Beisen.AppConnect.Web.WebSite.Controllers
{
    public class ISVController : Controller
    {
        // GET: ISV
        public ActionResult Index()
        {
            ISVAuthInfo loginInfo = new ISVAuthInfo();
            loginInfo.CorpId = "eee";
            loginInfo.AppId = "ddd";
            loginInfo.LoginType = 1;
            loginInfo.BusinessRedirectUrl = "ddd";
            Cookie.SetISVAuthInfo(loginInfo);
            var login11Info = Cookie.GetISVAuthInfo();
            return View();
        }
        public ActionResult ClearCookie()
        {
            Cookie.ClearDingTalkUserInfo();
            Cookie.ClearISVAuthInfo();
            Cookie.ClearISVLoginInfo();
            var errorInfoUrl = MvcRouting.Instance.GetErrorInfoUrl("清除Cookie成功了!");
            return Redirect(errorInfoUrl);

            return View();
        }
        public ActionResult ISVLoginAuthorize(string corpId = "", string appId = "", int loginType = (int)DingTalkLoginType.Mobil, string businessRedirectUrl = null, int titaAppId = 1)
        {
            try
            {
                LogHelper.Instance.Dump(string.Format("ISVLoginAuthorizeStart:CorpID:{0},AppId:{1},LogType{2},BusinessRedirectUrl:{3},TitaAppID:{4}", corpId, appId, loginType, businessRedirectUrl, titaAppId), LogType.Debug);
                if (string.IsNullOrWhiteSpace(corpId) || string.IsNullOrWhiteSpace(appId))
                {
                    var errorInfoUrl = MvcRouting.Instance.GetErrorInfoUrl(string.Format("CorpId:{0}和AppId{1}都不能为空!", corpId, appId));
                    return Redirect(errorInfoUrl);
                }

                //写入Cookie
                var isvAuthInfo = Cookie.GetISVAuthInfo();
                LogHelper.Instance.Dump(string.Format("ISVLoginAuthorize,GetISVAuthInfo,Before:{0}", JsonConvert.SerializeObject(isvAuthInfo)));
                isvAuthInfo.LoginType = loginType;
                isvAuthInfo.CorpId = corpId;
                isvAuthInfo.AppId = appId;
                isvAuthInfo.BusinessRedirectUrl = businessRedirectUrl;
                isvAuthInfo.AuthRedirectUrl = HttpContext.Request.RawUrl;
                isvAuthInfo.TitaAppId = titaAppId;
                LogHelper.Instance.Dump(string.Format("ISVLoginAuthorize,GetISVAuthInfo,After:{0}", JsonConvert.SerializeObject(isvAuthInfo)));
                Cookie.SetISVAuthInfo(isvAuthInfo);

                #region 钉钉
                var dingTalkUserInfo = Cookie.GetDingTalkUserInfo();
                LogHelper.Instance.Dump(string.Format("ISV_CurrentDingTalkUserInfo：{0}", JsonConvert.SerializeObject(dingTalkUserInfo)), LogType.Debug);

                //先获取钉钉的UserId信息
                if (dingTalkUserInfo == null || dingTalkUserInfo.CorpId != isvAuthInfo.CorpId)// 解绑的时候，可以通过这里做清楚钉钉cookie操作
                {
                    if (loginType == (int)DingTalkLoginType.Mobil)
                    {
                        LogHelper.Instance.Dump(string.Format("跳转Mobile登录"), LogType.Debug);
                        return Redirect(MvcRouting.DingTalkAuthorize);
                    }
                    else if (loginType == (int)DingTalkLoginType.PC)
                    {
                        LogHelper.Instance.Dump(string.Format("跳转PC登录"), LogType.Debug);
                        return Redirect(MvcRouting.DingTalkAuthorizePC);
                    }
                    else
                    {
                        var errorInfoUrl = MvcRouting.Instance.GetErrorInfoUrl("LogType错误!");
                        return Redirect(errorInfoUrl);
                    }
                }
                else
                {
                    Cookie.SetDingTalkUserInfo(dingTalkUserInfo);
                }
                #endregion

                //验证是否已绑定
                var flag = ISVLoginProvider.Instance.VerifyIsActivate();
                if (!flag)
                {
                    LogHelper.Instance.Dump(string.Format("VerifyIsActivate为False,跳转到Login"), LogType.Debug);
                    return Redirect(MvcRouting.LoginPage);
                };

                var userInfoCookie = Cookie.GetISVLoginInfo();
                LogHelper.Instance.Dump(string.Format("ISV_UserInfoCookie：{0}", JsonConvert.SerializeObject(userInfoCookie)), LogType.Debug);
                LogHelper.Instance.Dump(string.Format("GetISVAuthInfo：{0}", JsonConvert.SerializeObject(isvAuthInfo)), LogType.Debug);

                #region 拼接跳转Url
                var redirectUrl = string.Empty;
                if (loginType == (int)DingTalkLoginType.Mobil)
                {
                    redirectUrl = ISVLoginProvider.Instance.GetTitaAuthUrl();
                }
                else if (loginType == (int)DingTalkLoginType.PC)
                {
                    redirectUrl = ISVLoginProvider.Instance.GetTitaAuthPCUrl();
                }
                if (string.IsNullOrWhiteSpace(redirectUrl))
                {
                    var errorInfoUrl = MvcRouting.Instance.GetErrorInfoUrl("RedirectUrl域名获取失败,请检查远程配置或LoginType值!");
                    return Redirect(errorInfoUrl);
                }
                var businessUrl = isvAuthInfo.BusinessRedirectUrl;
                if (!string.IsNullOrWhiteSpace(businessUrl))
                {
                    redirectUrl = string.Format(redirectUrl, HttpUtility.UrlEncode(businessUrl));
                }
                else
                {
                    var cloudDomain = ISVLoginProvider.Instance.GetCloudDomain();
                    redirectUrl = string.Format(redirectUrl, HttpUtility.UrlEncode(cloudDomain));
                    //if (loginType == (int)DingTalkLoginType.Mobil)
                    //{
                    //}
                    //else
                    //{
                    //    var cloudDomain = "http://www.italent.link";
                    //    redirectUrl = string.Format(redirectUrl, HttpUtility.UrlEncode(cloudDomain));
                    //}
                }
                LogHelper.Instance.Dump(string.Format("获取RedirectURL Domain：{0}", redirectUrl));
                #endregion

                #region 签名
                //获取Tita签名
                var signResult = ISVLoginProvider.Instance.GetTitaSsoSignV2(userInfoCookie.TenantId, userInfoCookie.UserId, isvAuthInfo.TitaAppId);
                if (string.IsNullOrWhiteSpace(signResult.SsoSign))
                {
                    var errorInfoUrl = MvcRouting.Instance.GetErrorInfoUrl("未获取到SsoSign!");
                    return Redirect(errorInfoUrl);
                }
                redirectUrl = UrlTool.AddQuery(redirectUrl, signResult.SsoSign);
                #endregion

                LogHelper.Instance.Dump(string.Format("最终URL：{0}", redirectUrl), LogType.Debug);
                // Url跳转的时候无法清除之前Url钉钉的Cookie
                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                var errorInfoUrl = MvcRouting.Instance.GetErrorInfoUrl("ISVLoginAuthorize:" + ex.Message);
                LogHelper.Instance.Dump(string.Format("ISVLoginAuthorize Error{0}", JsonConvert.SerializeObject(ex)));
                return Redirect(errorInfoUrl);
            }
        }


        #region  Login
        public ActionResult LoginPage()
        {
            return View();
        }
        public ActionResult LoginAuthorize(string userName, string passWord)
        {
            try
            {
                LogHelper.Instance.Dump(string.Format("进入移动端登录"), LogType.Debug);
                var isvAuthInfo = Cookie.GetISVAuthInfo();
                LogHelper.Instance.Dump(string.Format("LoginAuthorize:Before:{0}", JsonConvert.SerializeObject(isvAuthInfo)));
                //判断账号是否激活,未激活清楚Cookie
                var loginInfo = ISVLoginProvider.Instance.Login(userName, passWord);
                loginInfo.AuthRedirectUrl = isvAuthInfo.AuthRedirectUrl;
                LogHelper.Instance.Dump(string.Format("LoginAuthorize, After:{0}", JsonConvert.SerializeObject(isvAuthInfo)));
                return Json(loginInfo);
            }
            catch (Exception ex)
            {
                var errorInfoUrl = MvcRouting.Instance.GetErrorInfoUrl("LoginAuthorize:" + ex.Message);
                return Redirect(errorInfoUrl);
            }
        }
        #endregion

        #region DingTalk
        public ActionResult DingTalkAuthorize()
        {
            LogHelper.Instance.Dump(string.Format("进入移动端登录"), LogType.Debug);
            JsApiInfo jsApiInfo = new JsApiInfo();
            try
            {
                var isvAuthInfo = Cookie.GetISVAuthInfo();
                LogHelper.Instance.Dump(string.Format("DingTalkAuthorize:{0}", JsonConvert.SerializeObject(isvAuthInfo)));
                var corpId = isvAuthInfo.CorpId;
                var appId = isvAuthInfo.AppId;
                var currentUrl = HttpUtility.UrlDecode(Request.Url.AbsoluteUri);
                jsApiInfo = ISVLoginProvider.Instance.GetJsApiInfo(corpId, appId, currentUrl);
            }
            catch (Exception ex)
            {
                var errorInfoUrl = MvcRouting.Instance.GetErrorInfoUrl("DingTalkAuthorize:" + ex.Message);
                LogHelper.Instance.Dump(string.Format("DingTalkAuthorize,Redirect:{0}", JsonConvert.SerializeObject(ex)), LogType.Debug);
                LogHelper.Instance.Dump(string.Format("DingTalkAuthorize,Redirect:{0}", JsonConvert.SerializeObject(ex)), LogType.Error);
                return Redirect(errorInfoUrl);
            }
            return View(jsApiInfo);
        }
        public ActionResult DingTalkAuthorizePC()
        {
            LogHelper.Instance.Dump(string.Format("进入PC登录"), LogType.Debug);
            JsApiInfo jsApiInfo = new JsApiInfo();
            try
            {
                var isvAuthInfo = Cookie.GetISVAuthInfo();
                LogHelper.Instance.Dump(string.Format("DingTalkAuthorizePC:{0}", JsonConvert.SerializeObject(isvAuthInfo)));
                var corpId = isvAuthInfo.CorpId;
                var appId = isvAuthInfo.AppId;
                var currentUrl = HttpUtility.UrlDecode(Request.Url.AbsoluteUri);
                jsApiInfo = ISVLoginProvider.Instance.GetJsApiInfo(corpId, appId, currentUrl);
            }
            catch (Exception ex)
            {
                var errorInfoUrl = MvcRouting.Instance.GetErrorInfoUrl("DingTalkAuthorizePCError:" + ex.Message);
                return Redirect(errorInfoUrl);
            }
            LogHelper.Instance.Dump(string.Format("进入PC登录返回值JsApiInfo:{0}", JsonConvert.SerializeObject(jsApiInfo)), LogType.Debug);
            return View(jsApiInfo);
        }
        public ActionResult DingTalkAuthorize_CallBack(string code)
        {
            try
            {
                LogHelper.Instance.Dump(string.Format("进入DingTalkAuthorize_CallBack"), LogType.Debug);
                var isvAuthInfo = Cookie.GetISVAuthInfo();
                LogHelper.Instance.Dump(string.Format("DingTalkAuthorize_CallBack:{0}", JsonConvert.SerializeObject(isvAuthInfo)));
                var currentCorpId = isvAuthInfo.CorpId;
                ISVLoginProvider.Instance.GetDingTaklUserInfo(currentCorpId, code);
                var isvLoginAuthorizeUrl = isvAuthInfo.AuthRedirectUrl;
                LogHelper.Instance.Dump(string.Format("DingTalkAuthorize_CallBack最终跳转Url:{0}", isvLoginAuthorizeUrl), LogType.Debug);
                return Redirect(isvLoginAuthorizeUrl);
            }
            catch (Exception ex)
            {
                var errorInfoUrl = MvcRouting.Instance.GetErrorInfoUrl("DingTalkAuthorize_CallBack:" + ex.Message);
                return Redirect(errorInfoUrl);
            }
        }
        #endregion

    }
}
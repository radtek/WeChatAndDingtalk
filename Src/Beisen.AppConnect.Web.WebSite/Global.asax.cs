using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Beisen.AppConnect.Infrastructure.Helper;

namespace Beisen.AppConnect.Web.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Context != null && Context.AllErrors != null)
            {
                var url = Context.Request.Url.ToString();

                //var text = new StringBuilder();
                //text.Append("Request.Browser.MajorVersion:"+Request.Browser.MajorVersion);
                //text.AppendLine();
                //text.Append("Request.Browser.Platform:" + Request.Browser.Platform);
                //text.AppendLine();
                //text.Append("Request.UserHostAddress:" + Request.UserHostAddress);
                //text.AppendLine();
                //text.Append("Request.Url:" + Request.Url);
                //text.AppendLine();
                //text.Append("Request.UrlReferrer:" + Request.UrlReferrer);
                //text.AppendLine();
                //text.Append("Request.UserAgent:" + Request.UserAgent);
                //text.AppendLine();
                //text.Append("Request.UserHostAddress:" + Request.UserHostAddress);
                //text.AppendLine();
                //text.Append("Request.UserHostName :" + Request.UserHostName);
                //text.AppendLine();
                //text.Append("Request.UserLanguages :" + Request.UserLanguages);
                //AppConnectLogHelper.Error(string.Format("[TestLog][{0}]", text.ToString()));

                foreach (var error in Context.AllErrors)
                {
                    AppConnectLogHelper.Error(string.Format("[Application_Error][{0}]", url), error);
                }
            }
        }
    }
}

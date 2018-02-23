using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Beisen.AppConnect.Infrastructure.Helper;

namespace Beisen.AppConnect.Api.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configuration.ParameterBindingRules.Insert(0,SimplePostVariableParameterBinding.HookupParameterBinding);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            if (Context != null && Context.AllErrors != null)
            {
                var url = Context.Request.Url.ToString();
                foreach (var error in Context.AllErrors)
                {
                    AppConnectLogHelper.Error(string.Format("[Application_Error][{0}]", url), error);
                }
            }
        }
    }
}

using System.Web.Http;

namespace Beisen.AppConnect.Api.Controller.Controllers
{
    public class TestController:ApiControllerBase
    {
        [HttpGet]
        public string Test()
        {
            return "OK";
        }
    }
}

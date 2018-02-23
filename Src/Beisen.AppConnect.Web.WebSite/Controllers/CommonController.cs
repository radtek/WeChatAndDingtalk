using Beisen.AppConnect.Infrastructure.Helper;
using System.Text;
using System.Web.Mvc;

namespace Beisen.AppConnect.Web.WebSite.Controllers
{
    public class CommonController : BaseController
    {
        // GET: Common
        public ActionResult WeChatVerify(string key)
        {
            byte[] data = Encoding.UTF8.GetBytes(key.Split('_')[2]);
            return File(data, "text/plain", key + ".txt");
        }

        public ActionResult Error(string title = "出错了", string message = "")
        {
            ViewData["Title"] = title;
            ViewData["Message"] = message;
            AppConnectLogHelper.DebugFormat("页面信息：{0}", message);
            return View();
        }
    }
}
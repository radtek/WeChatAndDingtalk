using System.Web.Mvc;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface.Model;
using System.Collections.Generic;
using System;
using Beisen.AppConnect.ServiceInterface;

namespace Beisen.AppConnect.Web.WebSite.Controllers
{
    public class TestController : BaseController
    {
        // GET: Test
        //public ActionResult GetCookie2()
        //{
        //    var cookie = CookieHelper.GetCookie();

        //    //CookieHelper.SetOpenId(Guid.NewGuid().ToString(), DateTime.Now.ToString());

        //    return Content(SerializeHelper.Serialize(cookie));
        //}

        //[AccountIdentity]
        //public ActionResult GetCookie()
        //{
        //    var cookie = CookieHelper.GetCookie();
        //    return Content(SerializeHelper.Serialize(cookie));
        //}

        public ActionResult ClearCookie()
        {
            CookieHelper.ClearCookie();

            return Content("ok111");
        }

        public ActionResult Test()
        {
            var messageInfo = new MessageInfo();
            var resultMsg = string.Empty;
            try
            {
                var content = new MessageContent();
                content.Url = "http://www.baidu.com";
                content.Title = "测试消息";
                content.Detail = new List<MessageContentDetail>();
                content.Detail.Add(new MessageContentDetail { Key = "first", Text = "", Value = "您好，邀请您参加面试dfdfdfd" });
                content.Detail.Add(new MessageContentDetail { Key = "keyword1", Text = "应聘职位d", Value = "开发工程师dfdfdfd" });
                content.Detail.Add(new MessageContentDetail { Key = "keyword2", Text = "面试时间d", Value = DateTime.Now.ToString("yyyy-MM-dd HH:MM") });
                content.Detail.Add(new MessageContentDetail { Key = "keyword3", Text = "面试地点d", Value = "北京市海淀区上地东路颐泉汇大厦7层dfdfd" });
                content.Detail.Add(new MessageContentDetail { Key = "keyword4", Text = "面试说明", Value = "请携带相关证件" });
                content.Detail.Add(new MessageContentDetail { Key = "keyword5", Text = "通知人", Value = "北森hr" });
                content.Detail.Add(new MessageContentDetail { Key = "remark", Text = "", Value = "点击详情，答复是否参加" });


                messageInfo.TenantId = 107106;
                messageInfo.FromUser = 114687683;
                messageInfo.ToUser = "114687683";
                messageInfo.ProductId = "909";
                messageInfo.MessageTypeList = new List<int>() { 21 };
                messageInfo.Content = new MessageContent();
                messageInfo.Content.Url = "http://cloud.italent.cn";
                messageInfo.Content.Title = "Szy消息接口线上测试!";
                messageInfo.Content.Detail = new List<MessageContentDetail>();
                foreach (var detail in content.Detail)
                {
                    messageInfo.Content.Detail.Add(new MessageContentDetail { Key = detail.Key, Text = detail.Text, Value = detail.Value });
                }
                messageInfo.ContentJson = SerializeHelper.Serialize(messageInfo.Content);
                AppConnectLogHelper.Debug("开始发送消息,消息内容Test:" + Newtonsoft.Json.JsonConvert.SerializeObject(messageInfo));
                var id = ProviderGateway.MessageProvider.Send(messageInfo);
                resultMsg = "发送成功!BatchId" + id;
            }
            catch (System.Exception ex)
            {
                resultMsg = "发送失败! Ex Message" + ex.Message;
            }
            return Content(resultMsg + "ok");
        }
    }
}
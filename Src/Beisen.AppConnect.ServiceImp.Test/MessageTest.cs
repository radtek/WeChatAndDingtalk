using System;
using System.Collections.Generic;
using Beisen.AppConnect.Infrastructure.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceImp.Provider;

namespace Beisen.AppConnect.ServiceImp.Test
{
    [TestClass]
    public class MessageTest
    {
        [TestMethod]
        public void TestSend()
        {
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

                var message = new MessageInfo();
                message.TenantId = 107106;
                message.FromUser = 114687683;
                message.ToUser = "114687683";
                //   message.ToOpenId = "0600532336792907";
                message.TemplateIdShort = "OPENTM206176827";
                message.Content = content;
                message.ContentJson = SerializeHelper.Serialize(content);
                message.AppAccountId = "2056d796-260e-4007-87f6-1a8e0ef9efa1";
                message.ProductId = "907";
                message.MessageTypeList = new List<int> { 21, 14 };
                var batch = ProviderGateway.MessageProvider.Send(message);

                Assert.IsFalse(string.IsNullOrWhiteSpace(batch));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void TestSendALL()
        {
            try
            {
                var content = new MessageContent();
                content.Url = "http://cloud.italent.link/Attendance/#/indexPage?metaObjName=Attendance.AttendanceStatistics&app=Attendance";
                content.Title = "消息通知3333";
                content.Detail = new List<MessageContentDetail>();
                content.Detail.Add(new MessageContentDetail { Key = "first", Text = "", Value = "您好，42423424334353" });
                content.Detail.Add(new MessageContentDetail { Key = "remark", Text = "", Value = "点击详情，答复是否参加" });

                var message = new MessageInfo();
                message.TenantId = 100002;
                message.FromUser = 112664957;
                message.ToUser = "112664957";
                message.ProductId = "1";
                message.Content = content;
                message.MessageTypeList = new List<int> { 21,13, 14 };
                message.ContentJson = SerializeHelper.Serialize(content);

                var batch = ProviderGateway.MessageProvider.Send(message);

                Assert.IsFalse(string.IsNullOrWhiteSpace(batch));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void TestWX()
        {
            var appAccount = ProviderGateway.AppAccountProvider.Get("7AE0406B-3CE1-47E2-8D34-709CC94F4AA3");
            var token = ProviderGateway.TokenProvider.GetToken(appAccount);
            var sendResult = WeChatSDK.QY.Message.SendNews(token, "zhangxin", 37, "title", "dddddd", "http://www.baidu.com");
        }

        [TestMethod]
        public void TestWX2()
        {
            var appAccount = ProviderGateway.AppAccountProvider.Get("9100cd6c-bf4b-4097-ad02-989adc9c31fb");
            var token = ProviderGateway.TokenProvider.GetToken(appAccount);
            var picUrl = MediaDataHelper.GetPicUrl("1", "14");
            var sendResult = WeChatSDK.Work.Message.SendNews(token, "LiuZhiWei", 1000002, "title", "dddddd", "http://www.baidu.com", picUrl);
        }

        [TestMethod]
        public void TestDD()
        {
            var appAccount = ProviderGateway.AppAccountProvider.Get("56F84327-47E2-4133-8C45-B6CE4E1FC07E");
            var token = ProviderGateway.TokenProvider.GetToken(appAccount);
            var sendResult = DingTalkSDK.Message.SendOA(token, "0600532336792907", "53218321", "http://www.baidu.com", "tttttt", "ddd:ddddd\ndd:fdfd\nsdL:sdsd");
        }
        [TestMethod]
        public void TestCheckApp()
        {
            int titaAppId = 1;
            string url = "http://www.italent.link/v1/sso/italent/auth?return_url=http://www.italent.link/feedInfo?appId=908";
            var a = AuthorizeProvider.Instance.CheckApp(titaAppId, url);
        }

    }
}

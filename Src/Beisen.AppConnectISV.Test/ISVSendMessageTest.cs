using Beisen.AppConnectISV.Model.HttpModel;
using Beisen.AppConnectISV.ServiceImp;
using Beisen.Common.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Test
{
    [TestClass]
    public class ISVSendMessageTest
    {
        [TestMethod]
        public void sendMessage()
        {
            //ApplicationContext.Current.TenantId = 245494;
            //ApplicationContext.Current.UserId = 201038245;
            //ApplicationContext.Current.ApplicationName = "AppConnect";

            //var tenantId = 100002;
            //var toUserIds = new int[] { 112737565 };

            //List<MessageContentDetail> detailList = new List<MessageContentDetail>();
            //detailList.Add(new MessageContentDetail { Key = "first", Text = "", Value = "您好，邀请您参加面试dfdfdfd" });
            //detailList.Add(new MessageContentDetail { Key = "keyword1", Text = "应聘职位d", Value = "开发工程师dfdfdfd" });
            //detailList.Add(new MessageContentDetail { Key = "keyword2", Text = "面试时间d", Value = DateTime.Now.ToString("yyyy-MM-dd HH:MM") });
            //detailList.Add(new MessageContentDetail { Key = "keyword3", Text = "面试地点d", Value = "北京市海淀区上地东路颐泉汇大厦7层dfdfd" });
            //detailList.Add(new MessageContentDetail { Key = "keyword4", Text = "面试说明", Value = "请携带相关证件" });
            //detailList.Add(new MessageContentDetail { Key = "keyword5", Text = "通知人", Value = "北森hr" });
            //detailList.Add(new MessageContentDetail { Key = "remark", Text = "", Value = "点击详情，答复是否参加" });

            //List<MessageInfo> messageInfoList = new List<MessageInfo>();
            //MessageInfo message = new MessageInfo
            //{
            //    AppId = "4778",
            //    CorpId = "dingd8afaa8e8eb609cf35c2f4657eb6378f",
            //    TenantId = 100002,
            //    FromUser = 112737565,
            //    ToUser = "112841065",
            //    MediaUrl = "http://image.baidu.com/search/detail?z=0&word=%E7%8E%8B%E4%B9%89%E5%8D%9A%E4%BD%9C%E5%93%81&hs=0&pn=1&spn=0&di=0&pi=42860135199&tn=baiduimagedetail&is=0%2C0&ie=utf-8&oe=utf-8&cs=740205218%2C2685385096&os=&simid=&adpicid=0&lpn=0&fm=&sme=&cg=&bdtype=-1&oriquery=&objurl=http%3A%2F%2Fh.hiphotos.baidu.com%2Fimage%2Fpic%2Fitem%2Fd788d43f8794a4c262dd7be107f41bd5ac6e3979.jpg&fromurl=&gsm=0&catename=pcindexhot",
            //    Content = new MessageContent
            //    {
            //        Url = "https://www.baidu.com/",
            //        Title = "ISV发消息",
            //        Detail = detailList
            //    }
            //};
            //  messageInfoList.Add(message);

            //  var result = ISVSendMessageProvider.Instance.sendMessage(messageInfoList);
        }
        [TestMethod]
        public void TestSend()
        {
            try
            {
                int tenantId = 100002;
                int userId = 112737565;
                MessageInfoArgument message = new MessageInfoArgument();

                var content = new MessageContent();

                content.title = "测试消112221息";
                content.detail = new List<MessageContentDetail>();
                content.detail.Add(new MessageContentDetail { key = "first", text = "", value = "您好，邀请您参加11111111面试dfdfdfd" });
                content.detail.Add(new MessageContentDetail { key = "keyword1", text = "应聘职位d", value = "开发工1111程师dfdfdfd" });
                message.content = content;
                //测试通过
                //  message.content.url = "http://cloud.italent.link";  
                message.content.url = "http://cloud.italent.link/Attendance/#/indexPage?metaObjName=Attendance.AttendanceStatistics&app=Attendance";
                // message.receiverId = "112737565|111";
                message.receiverId = "112737651";
                message.productId = "1";
                message.messageType = "11|21";
                var batch = ISVSendMessageProvider.Instance.SendMessageDingTalk(100002, 112737651, message);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

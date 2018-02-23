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
    public class TestInitBase
    {
        public static int TenantId { get; set; }

        public static int CurrentUserId { get; set; }

        public void Init_Services()
        {

        }

        [TestInitialize]
        public void Init_Dev()
        {
            TenantId = 204233;
            CurrentUserId = 100373347;
            //TenantId = 245347;
            //CurrentUserId = 201034630;
            //TenantId = 245494;
            //CurrentUserId = 201038245;

            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = TenantId;
            ApplicationContext.Current.UserId = CurrentUserId;

            //在这个地方加载一下ServiceImp，要不然依赖注入进不来
            //var domain = new AnnouncementProvider();
        }
        //[TestInitialize]
        public void Init_Test()
        {
            TenantId = 100002;
            CurrentUserId = 112737565;

            ApplicationContext.Current.ApplicationName = "AppConnect";
            ApplicationContext.Current.TenantId = TenantId;
            ApplicationContext.Current.UserId = CurrentUserId;

            //在这个地方加载一下ServiceImp，要不然依赖注入进不来
            //var domain = new AnnouncementProvider();
        }
    }
}

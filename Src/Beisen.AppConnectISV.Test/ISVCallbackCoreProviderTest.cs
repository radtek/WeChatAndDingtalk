using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnectISV.Infrastructure;
using Beisen.AppConnectISV.ServiceImp;
using Beisen.AppConnectISV.BusinessCore;
using Beisen.AppConnectISV.BusinessCore.DingDing;
using Newtonsoft.Json;
namespace Beisen.AppConnectISV.Test
{
    [TestClass]
    public class ISVCallbackCoreProviderTest
    {
        /// <summary>
        /// 激活条件
        /// </summary>
        [TestMethod]
        public void Activate_Suite()
        {
            try
            {
                string mToken = ISVInfo.Token;
                string mSuiteKey = ISVInfo.SuiteKey;
                string mSuitSecret = ISVInfo.SuitSecret;
                string mEncodingAesKey = ISVInfo.EncodingAesKey;
                string tem_code = "09cf8570c27a34eaae56ed77e87e1186";
                string suiteTicket = "wri8H39Ck6UF9FyOJ0kXtmoENmvDCt1C82rpI77B3kcWVGzfqrYrKa7ksqw3rRwo5RAE087Bd34sNG0aRzZenX";
                ISVCallbackCoreProvider.Instance.SaveTicket(suiteTicket);
                ISVCallbackCoreProvider.Instance.Activate_Suite(tem_code);
            }
            catch (Exception ex)
            {

            }


        }

        /// <summary>
        /// 解除授权
        /// </summary>
        [TestMethod]
        public void Suite_Relieve()
        {
            string corpId = "dingb89b7c15fba4016235c2f4657eb6378f";
            ISVCallbackCoreProvider.Instance.Suite_Relieve(corpId);
        }

        /// <summary>
        /// 保存Cloud
        /// </summary>
        [TestMethod]
        public void Suite_SaveCloud()
        {
            PermanentCode_Result t1 = new PermanentCode_Result();
            t1.Permanent_Code = "111";
            Auth_Corp_Info t2 = new Auth_Corp_Info();
            t2.Corpid = "CorpId";
            t2.Corp_name = "CorpName";
            t1.AuthCorpInfo = t2;
            ISVCallbackCoreProvider.Instance.SavePermanentCode(t1);
        }
        /// <summary>
        /// 保存Cloud
        /// </summary>
        [TestMethod]
        public void Suite_DeletedCloud()
        {
            ISVCallbackCoreProvider.Instance.DeletedPermanentCode("CorpId");
        }
    }
}

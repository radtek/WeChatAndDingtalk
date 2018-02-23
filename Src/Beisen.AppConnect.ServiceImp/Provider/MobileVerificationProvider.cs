using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.ServiceImp.Persistance;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.Common.HelperObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    public class MobileVerificationProvider : IMobileVerificationProvider
    {
        #region 单例

        private static readonly IMobileVerificationProvider _instance = new MobileVerificationProvider();
        public static IMobileVerificationProvider Instance
        {
            get { return _instance; }
        }

        private MobileVerificationProvider()
        {
        }
        #endregion

        /// <summary>
        /// 生成ISV验证码，并发送到用户手机
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="isv"></param>
        /// <param name="mobile"></param>
        /// <param name="type">0:注册验证码</param>
        /// <returns></returns>
        public bool SendISVMessage(int tenantId, string isv, string mobile, out string msg, int type = 0)
        {
            ArgumentHelper.AssertPositive(tenantId, "tenantId is 0");
            ArgumentHelper.AssertNotNullOrEmpty(isv, "isv is null");
            ArgumentHelper.AssertNotNullOrEmpty(mobile, "mobile is null");

            msg = string.Empty;
            if (string.IsNullOrEmpty(isv) || string.IsNullOrWhiteSpace(mobile))
            {
                msg = string.Format("isv或手机号不能为空,isv:{0},mobile:{1}", isv, mobile);
                return false;
            }
            var check = MobileVerificationDao.Get(mobile, type, DateTime.Now.AddMinutes(-1));
            if (check == 1)
            {
                msg = string.Format("验证码发送过于频繁");
                return false;
            }
            var isvSMSTemplate = GetISVSMSTemplate(isv, mobile, type);
            if (isvSMSTemplate == null)
            {
                msg = string.Format("{0}未找到设置,mobile:{1},type:{2}", isv, mobile, type);
                return false;
            }

            var code = GetCode();

            //将验证码和手机号存入数据库
            MobileVerificationDao.Add(mobile, code, type);
            //发送短信
            string message = string.Format(isvSMSTemplate.Message, code);
            var result =
                ProviderGateway.SMSProvider.SendISVMobileValCode(tenantId, isv, isvSMSTemplate.SMSChannelId, message, mobile,
                    SMSType.MobileVerification, null);

            if (!result)
            {
                MobileVerificationDao.Abate(mobile, code, type);
                msg = string.Format("发送失败,tenantId:{0},isv:{1},mobile:{2}", tenantId, isv, mobile);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 校验验证码状态及验证时间
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <param name="type">0:注册验证码 1:登录动态验证码</param>
        /// <returns></returns>
        public bool CheckCode(string mobile, int code, int type)
        {
            return MobileVerificationDao.Check(mobile, code, type);
        }

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool VerifyCode(string mobile, int code, int type, out int codestate)
        {
            //to do sth.要写入配置文件里
            codestate = 0;
            return MobileVerificationDao.Verify(mobile, code,
               DateTime.Now.AddMinutes(-5), type, out codestate);
        }

        /// <summary>
        /// 根据isv获取短信模板
        /// </summary>
        /// <param name="isv"></param>
        /// <param name="mobile"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private ISVSMSTemplate GetISVSMSTemplate(string isv, string mobile, int type)
        {
            var isvSettings = Beisen.AppConnect.Infrastructure.Configuration.AppConnectISVSettingsConfig.Cache;
            if (isvSettings == null || !isvSettings.ContainsKey(isv))
            {
                return null;
            }
            var isvSetting = isvSettings[isv];
            ISVSMSTemplate template = null;
            if (isvSetting.SMSTemplates == null || !isvSetting.SMSTemplates.Contains(type.ToString()))
            {
                //template = isvSetting.SMSTemplates["default"];
                return null;
            }
            else
            {
                template = isvSetting.SMSTemplates[type.ToString()];
            }

            if (template == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(template.SMSChannelId))
            {
                template.SMSChannelId = isvSetting.SMSChannelId;
            }
            return template;
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private int GetCode(int length = 6)
        {
            string code = "";
            //每次生成随机数的时候都使用机密随机数生成器来生成种子，
            //这样即使在很短的时间内也可以保证生成的随机数不同
            Random rdm = new Random(Chaos_GetRandomSeed());
            for (int i = 0; i < length; i++)
            {
                int iRand = rdm.Next(1, 9);
                code += iRand.ToString();
            }
            return Convert.ToInt32(code);
        }

        /// <summary>
        /// 加密随机数生成器，生成随机种子
        /// </summary>
        /// <returns></returns>
        private static int Chaos_GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng =
                new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}

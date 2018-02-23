using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beisen.SMSMailPlatformServices.ServiceInterface;
using Beisen.SMSMailPlatformServices.SMS.Model;
using Beisen.ESB.Client;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.Infrastructure.Exceptions;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    public class SMSProvider : ISMSProvider
    {
        #region 单例

        private static readonly ISMSProvider _instance = new SMSProvider();
        public static ISMSProvider Instance
        {
            get { return _instance; }
        }

        private SMSProvider()
        {
        }
        #endregion

        /// <summary>
        /// 发送ISV手机验证码（只有发送验证码可用，发送其他短信不要用此接口）
        /// </summary>
        public bool SendISVMobileValCode(int tenantId, string isv, string channelId, string content, string toPhone, SMSType type, string templateMessageText)
        {
            if (string.IsNullOrEmpty(channelId))
            {
                AppConnectLogHelper.Error(new SMSSendException("短信通道未配置"));
                return false;
            }

            var smsInvokeProvider = PlatformServiceFactory<ISMSMailProvider>.Instance();
            if (smsInvokeProvider == null)
            {
                AppConnectLogHelper.Error(new ESBServiceNotFoundException("SMS ESB Service Is null"));
                return false;
            }

            var smsBatch = new SMSBatch();
            var shortMessages = new List<ShortMessage>()
            {
                new ShortMessage()
                {
                    PhoneNumber = toPhone,
                    MessageText = content,
                    TemplateMessageText = templateMessageText
                }
            };

            var messageCollection = new MessageCollection(shortMessages.ToArray());

            smsBatch.SystemId = channelId;
            if (tenantId == 0)
            {
                tenantId = 2;
            }
            smsBatch.TenantId = tenantId;
            smsBatch.TenantName = "ISV业务短信";
            smsBatch.ShortMessages = messageCollection;
            smsBatch.UserId = 0;
            ArgumentHelper.IsValid(smsBatch, true);

            var sendInfo = smsInvokeProvider.SMS_SendSMSByTime(smsBatch);
            bool isSuccess = CheckResult(sendInfo);

            if (isSuccess)
            {
                return true;
            }
            AppConnectLogHelper.Error("短信发送失败：" + sendInfo);
            return false;
        }

        private bool CheckResult(string result)
        {
            if (string.IsNullOrEmpty(result))
            {
                return true;
            }

            if (result.Equals("Unknow"))
            {
                return false;
            }

            else if (result.Equals("EmptyRequest"))
            {
                return false;
            }
            else if (result.Equals("ArgumentError"))
            {
                return false;

            }
            else if (result.Equals("Unauthorized"))
            {
                return false;
            }
            else if (result.Equals("UnallowedTime"))
            {
                return false;
            }
            else if (result.Equals("Failed"))
            {
                return false;

            }
            else
            {
                return true;
            }
        }
    }
}

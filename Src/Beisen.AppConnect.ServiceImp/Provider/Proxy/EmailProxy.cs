using System;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.ESB.Client;
using Beisen.SMSMailPlatformServices.ServiceInterface;
using Beisen.SMSMailPlatformServices.Email.Model;

namespace Beisen.AppConnect.ServiceImp.Provider.Proxy
{
    public static class EmailProxy
    {
        private static string _systemId = "RecruitingSHMail";

        private static string _virtualFromEmail = "italent@italent.cn";

        public static string SendEmail(int tenantId, string emailSubject, string fromUser, string toUsers, string body, bool isBodyHtml = true, EmailType emailType = EmailType.Normal)
        {
            Beisen.BeisenUser.BeisenTenant beisenTenant = null;
            if (tenantId != 0)
            {
                beisenTenant = BeisenUser.ServiceImp.BeisenUserGateway.BeisenTenantProvider.GetTenantById(tenantId);
            }

            var smsMailProvider = PlatformServiceFactory<ISMSMailProvider>.Instance();
            if (smsMailProvider == null)
            {
                AppConnectLogHelper.Error("SMSMailProvider实例化错误");
                return string.Empty;
            }
            var batch = new MailBatch
            {
                SystemId = _systemId,
                TenantId = beisenTenant != null ? beisenTenant.ID : 0,
                EMailType = emailType,
                TenantName = beisenTenant != null ? beisenTenant.Name : "北森"
            };

            var to = new MailAddressCollection();
            to.Add(new MailAddress(toUsers, toUsers));

            var msg = new MailMessage
            {
                From = new MailAddress(_virtualFromEmail, "iTalent"),
                To = to,
                //ReplyTo = new MailAddressCollection()
                //{
                //    new MailAddress()
                //    {
                //        DisplayName = "iTalent",
                //        Address = _virtualFromEmail
                //    }
                //},
                Subject = emailSubject,
                Body = body,
                IsBodyHtml = isBodyHtml,
                Priority = MailPriority.Normal
            };
            batch.MailMessages.Add(msg);

            var result = smsMailProvider.Mail_SendMailByBatch(batch);

            if (Enum.IsDefined(typeof (RequestStatus), result))
            {
                AppConnectLogHelper.Error(string.Format("发送邮件失败：result={0},toUsers={1}", result, string.Join("|", toUsers)));
                return string.Empty;
            }
            return result;
        }
    }
}

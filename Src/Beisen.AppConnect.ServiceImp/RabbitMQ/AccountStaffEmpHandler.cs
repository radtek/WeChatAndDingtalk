using Beisen.Amqp;
using Beisen.AppConnect.Infrastructure.Enums;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceImp.Provider;
using Beisen.AppConnect.ServiceImp.RabbitMQ.Handler;
using Beisen.AppConnect.ServiceImp.RabbitMQ.Message;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.RabbitMQ
{
    public class AccountStaffEmpHandler : StringBaseMessageHandler
    {
        private static readonly IRabbitMQHandlerQueue _asyncHandleQueue = new RabbitMQHandlerQueue();
        public override MessageResult OnMessage(MessageContext context)
        {
            if (context == null)
            {
                AppConnectLogHelper.Error("AppConnect context is null , process finished");
                return Ack();
            }

            try
            {
                #region 处理前的校验
                if (context.Data == null)//消息体为空
                {
                    AppConnectLogHelper.Error("AppConnect context.Data is null , process finished");
                    return Ack();
                }
                var messageFiledList = Convert.ToString(context.Data).Trim('\"').Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (messageFiledList == null || messageFiledList.Count() < 3)
                {
                    AppConnectLogHelper.Error("处理消息时 消息结构不完整 , process finished");
                    return Ack();
                }
                AccountStaffMessage empMessage = new AccountStaffMessage
                {
                    TenantId = Convert.ToInt32(messageFiledList[0]),
                    NotifyType = (AccountNotifyType)(Convert.ToInt32(messageFiledList[1])),
                    UserId = Convert.ToInt32(messageFiledList[2])
                };
                AppConnectLogHelper.Debug("Account Staff的消息" + JsonConvert.SerializeObject(empMessage));
                #endregion

                switch (empMessage.NotifyType)
                {
                    //account账号停用，离职，删除
                    case AccountNotifyType.DisableUserStatus:
                    case AccountNotifyType.DisableStaffStatus:
                    case AccountNotifyType.DeleteStaff:
                        //AppConnectLogHelper.Error("分发rbmq 未使用ActionBlock方法作为缓冲");
                        //var provider = new DimissionRecordExcuter(empMessage);
                        //provider.Excute();
                        AppConnectLogHelper.Error("分发rbmq 使用本地的ActionBlock方法作为缓冲");
                        if (!_asyncHandleQueue.Post(new DimissionRecordExcuter(empMessage))) return Reject();
                        break;
                    default:
                        AppConnectLogHelper.Debug("AppConnect MessageHandler 未订阅的消息类型  .  Json :" + JsonConvert.SerializeObject(empMessage));
                        break;
                }
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.Error("AppConnect MessageHandler 处理出现异常  .  Json :" + JsonConvert.SerializeObject(ex));
            }
            return Ack();
        }
    }
}

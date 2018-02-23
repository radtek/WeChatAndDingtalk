using Beisen.AppConnectISV.BusinessCore;
using Beisen.AppConnectISV.BusinessCore.DingDing;
using Beisen.AppConnectISV.Infrastructure;
using Beisen.AppConnectISV.Model;
using Beisen.AppConnectISV.Model.BusinessModel;
using Beisen.AppConnectISV.Model.HttpModel;
using Beisen.SearchV3.DSL.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Beisen.MultiTenant.Model;
using Beisen.AppConnectISV.Model.BusinessEnum;
using Beisen.AppConnectISV.ServiceImp.CoreProvider;

namespace Beisen.AppConnectISV.ServiceImp
{
    public class ISVSendMessageProvider
    {
        #region Singleton 
        static readonly ISVSendMessageProvider _Instance = new ISVSendMessageProvider();
        public static ISVSendMessageProvider Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion
        public ApiResult SendMessageDingTalk(int tenantId, int userId, MessageInfoArgument messageInfoArgument)
        {
            ApiResult apiResutl = new ApiResult();
            try
            {
                var dingTalkType = Convert.ToString((int)MappingType.DingTalk);
                if (messageInfoArgument.messageType.Contains(dingTalkType))
                {
                    var sendMessageResult = ISVSendMessageCoreProvider.Instance.SendMessageDingTalk(tenantId, userId, messageInfoArgument);
                    apiResutl.ErrCode = sendMessageResult.ErrCode;
                    apiResutl.ErrMsg = sendMessageResult.ErrMsg;
                }
            }
            catch (Exception ex)
            {
                apiResutl.ErrCode = 417;
                apiResutl.ErrMsg = ex.Message;
                LogHelper.Instance.Dump("SendMessageDingTalkError" + JsonConvert.SerializeObject(ex));
            }
            return apiResutl;

        }
    }
}

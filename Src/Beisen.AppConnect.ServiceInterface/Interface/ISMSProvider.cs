using Beisen.AppConnect.ServiceInterface.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface ISMSProvider
    {
        /// <summary>
        /// 发送isv手机验证码
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="isv"></param>
        /// <param name="channelId"></param>
        /// <param name="content"></param>
        /// <param name="toPhone"></param>
        /// <param name="type"></param>
        /// <param name="templateMessageText"></param>
        /// <returns></returns>
        bool SendISVMobileValCode(int tenantId, string isv, string channelId, string content, string toPhone, SMSType type, string templateMessageText);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface IMobileVerificationProvider
    {
        /// <summary>
        /// 发送isv手机验证码
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="isv"></param>
        /// <param name="mobile"></param>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool SendISVMessage(int tenantId, string isv, string mobile, out string msg, int type = 0);

        /// <summary>
        /// 校验验证码状态及验证时间
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CheckCode(string mobile, int code, int type);

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool VerifyCode(string mobile, int code, int type, out int codestate);
    }
}

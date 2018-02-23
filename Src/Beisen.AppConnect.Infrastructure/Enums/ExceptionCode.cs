using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Infrastructure.Enums
{
    public enum ExceptionCode
    {
        None = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,
        /// <summary>
        /// 失败
        /// </summary>
        KnowError = 417,
        /// <summary>
        /// 未发现
        /// </summary>
        NotFound = 404,
        /// <summary>
        /// 服务器错误
        /// </summary>
        ServiceError = 500
    }

}

using Beisen.Data;
using System;

namespace Beisen.AppConnectISV.Infrastructure
{
    /// <summary>
    /// 请求日志
    /// </summary>
    internal class RequestLog
    {
        /// <summary>
        /// 记录请求日志
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="body"></param>
        /// <param name="response"></param>
        /// <param name="message"></param>
        internal static void Add(string url, string method, string body, string response, string message = null)
        {
            try
            {
                HttpRequestDao.Insert(url, method, body, response, message);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error("记录请求日志错误", ex);
            }
        }
    }

    /// <summary>
    /// 请求日志数据访问层
    /// </summary>
    internal class RequestLogDao
    {
        /// <summary>
        /// 记录请求日志
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="body"></param>
        /// <param name="response"></param>
        /// <param name="message"></param>

    }
}

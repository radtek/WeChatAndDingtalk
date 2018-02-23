using System;
using Beisen.Data;
using Beisen.AppConnect.Infrastructure.Helper;

namespace Beisen.AppConnect.Infrastructure.RequestUtility
{
    /// <summary>
    /// 请求日志
    /// </summary>
    internal class RequestLogHelper
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
                RequestLogDao.Insert(url, method, body, response, message);
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.Error("记录请求日志错误", ex);
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
        internal static void Insert(string url, string method, string body, string response, string message)
        {
            var db = Database.GetDatabase(DatabaseName.AppConnect);
            SafeProcedure.ExecuteNonQuery(db, "dbo.RequestLog_Insert", parameterMapper =>
            {
                parameterMapper.AddWithValue("@Url", url);
                parameterMapper.AddWithValue("@Method", method);
                parameterMapper.AddWithValue("@Body", body);
                parameterMapper.AddWithValue("@Response", response);
                parameterMapper.AddWithValue("@Message", message);
            });
        }
    }
}

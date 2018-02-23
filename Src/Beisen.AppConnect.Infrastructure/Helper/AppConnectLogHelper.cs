using System;
using Beisen.Logging;

namespace Beisen.AppConnect.Infrastructure.Helper
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public static class AppConnectLogHelper
    {
        /// <summary>
        /// appName
        /// </summary>
        //private static string _appName= "www.wechat.com";
        static LogWrapper _log = new LogWrapper();

        /// <summary>
        /// debug是否可用
        /// </summary>
        public static bool IsDebugEnabled
        {
            get { return _log.IsDebugEnabled; }
        }

        /// <summary>
        /// warn是否可用
        /// </summary>
        public static bool IsWarnEnabled
        {
            get { return _log.IsWarnEnabled; }
        }

        /// <summary>
        /// info是否可用
        /// </summary>
        public static bool IsInfoEnabled
        {
            get { return _log.IsInfoEnabled; }
        }

        /// <summary>
        /// error
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(int tenantId, string message, Exception exception)
        {
            _log.Error(message, exception, new LogModel { TenantID = tenantId });
        }

        /// <summary>
        /// error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(string message, Exception exception)
        {
            _log.Error(message, exception);
        }

        /// <summary>
        /// error
        /// </summary>
        /// <param name="exception"></param>
        public static void Error(Exception exception)
        {
            _log.Error(exception);
        }
        /// <summary>
        /// error
        /// </summary>
        /// <param name="exception"></param>
        public static void ErrorFormat(string info, params object[] args)
        {
            _log.ErrorFormat(info, args);
        }

        /// <summary>
        /// error
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            _log.Error(message);

        }

        /// <summary>
        /// debug
        /// </summary>
        /// <param name="info"></param>
        public static void Debug(string info)
        {
            _log.Debug(info);
        }
        /// <summary>
        /// debug
        /// </summary>
        /// <param name="info"></param>
        public static void DebugFormat(string info, params object[] args)
        {
            _log.DebugFormat(info, args);
        }

        /// <summary>
        /// info
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            _log.Info(message);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            _log.Warn(message);

        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public static void Warn(string message, Exception e)
        {
            _log.Warn(message, e);

        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Warn(string format, object[] args)
        {
            _log.WarnFormat(format, args);
        }

        /// <summary>
        /// 安全警告
        /// </summary>
        /// <param name="message"></param>
        public static void WarnSecurity(string message)
        {
            _log.WarnSecurity(message);
        }
    }
}

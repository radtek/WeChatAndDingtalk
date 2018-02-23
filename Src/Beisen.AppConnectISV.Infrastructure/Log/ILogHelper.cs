using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Infrastructure
{
    public interface ILogHelper
    {
        void Dump(string Msg, LogType logtype = LogType.Debug, Exception ex = null, string Tittle = "", int TenantId = 0, int UserId = 0, Object Context = null);
        void Error(string msg);
        void Debug(string msg);
    }
    public class LogModel
    {
        private readonly Guid _id;
        private readonly DateTime _createTime;
        public LogModel()
        {
            _id = Guid.NewGuid();
            _createTime = DateTime.Now;
        }
        public Guid id { get { return _id; } }
        public string FileName { get; set; }
        public string MethodName { get; set; }
        public string CodeLine { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int TenantId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateTime { get { return _createTime; } }
        public Exception Ex { get; set; }
        /// <summary>
        /// 要记录的上下文
        /// </summary>
        public Object Context { get; set; }
    }
    public enum LogType
    {
        Debug = 0,
        Info,
        Warn,
        Error,
        Fatal
    }
}

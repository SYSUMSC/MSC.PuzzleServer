using MSC.Server.Models;
using NLog;
using LogLevel = NLog.LogLevel;

namespace MSC.Server.Utils
{
    public static class LogHelper
    {
        /// <summary>
        /// 登记一条 Log 记录
        /// </summary>
        /// <param name="_logger">传入的 Nlog.Logger</param>
        /// <param name="msg">Log 消息</param>
        /// <param name="user">用户对象</param>
        /// <param name="status">操作执行结果</param>
        /// <param name="level">Log 级别</param>
        public static void Log(Logger _logger, string msg, UserInfo user, TaskStatus status, LogLevel? level = null)
        {
            LogEventInfo logEventInfo = new(level ?? LogLevel.Info, _logger.Name, msg);
            logEventInfo.Properties["uname"] = user.UserName;
            logEventInfo.Properties["ip"] = user.IP;
            logEventInfo.Properties["status"] = status.ToString();
            _logger.Log(logEventInfo);
        }

        /// <summary>
        /// 登记一条 Log 记录
        /// </summary>
        /// <param name="_logger">传入的 Nlog.Logger</param>
        /// <param name="msg">Log 消息</param>
        /// <param name="ip">连接IP</param>
        /// <param name="status">操作执行结果</param>
        /// <param name="level">Log 级别</param>
        public static void Log(Logger _logger, string msg, string ip, TaskStatus status, LogLevel? level = null)
        {
            LogEventInfo logEventInfo = new(level ?? LogLevel.Info, _logger.Name, msg);
            logEventInfo.Properties["uname"] = "Anonymous";
            logEventInfo.Properties["ip"] = ip;
            logEventInfo.Properties["status"] = status.ToString();
            _logger.Log(logEventInfo);
        }

        /// <summary>
        /// 登记一条 Log 记录
        /// </summary>
        /// <param name="_logger">传入的 Nlog.Logger</param>
        /// <param name="msg">Log 消息</param>
        /// <param name="username">用户名</param>
        /// <param name="ip">当前IP</param>
        /// <param name="status">操作执行结果</param>
        /// <param name="level">Log 级别</param>
        public static void Log(Logger _logger, string msg, string username, string ip, TaskStatus status, LogLevel? level = null)
        {
            LogEventInfo logEventInfo = new(level ?? LogLevel.Info, _logger.Name, msg);
            logEventInfo.Properties["uname"] = username;
            logEventInfo.Properties["ip"] = ip;
            logEventInfo.Properties["status"] = status.ToString();
            _logger.Log(logEventInfo);
        }
    }
}
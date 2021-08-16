using MSC.Server.Models;
using System.Threading.Tasks;

namespace MSC.Server.Hubs.Interface
{
    public interface ILoggingClient
    {
        /// <summary>
        /// 接收到广播日志信息
        /// </summary>
        public Task RecivedLog(LogMessageModel log);
    }
}

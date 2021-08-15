using Microsoft.AspNetCore.Http;
using NLog;
using MSC.Server.Models;
using System.Threading.Tasks;

namespace MSC.Server.Utils
{
    public class HubHelper
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 当前请求是否具有权限
        /// </summary>
        /// <param name="context">当前请求</param>
        /// <param name="privilege">权限</param>
        /// <returns></returns>
        public static Task<bool> HasPrivilege(HttpContext context, Privilege privilege)
        {
            return Task.FromResult(true);
        }
    }
}

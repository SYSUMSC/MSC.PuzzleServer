using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSC.Server.Middlewares;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;
using NLog;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class AdminController : ControllerBase
    {
        private static readonly Logger logger = LogManager.GetLogger("AdminController");
        private readonly ILogRepository logRepository;

        public AdminController(ILogRepository _logRepository)
        {
            logRepository = _logRepository;
        }

        [HttpPost]
        [RequireAdmin]
        [SwaggerResponse(400, "校验失败")]
        [SwaggerResponse(200, "成功获取日志", typeof(IList<LogMessageModel>))]
        public async Task<ActionResult<List<LogMessageModel>>> Logs(LogRequestModel model)
        {
            return await logRepository.GetLogs(model.Skip, model.Count, model.Level);
        }
    }
}

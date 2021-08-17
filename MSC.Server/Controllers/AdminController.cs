using Microsoft.AspNetCore.Mvc;
using MSC.Server.Middlewares;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;
using NLog;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MSC.Server.Controllers
{
    [RequireAdmin]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]/[action]")]
    [SwaggerTag("管理员数据交互接口")]
    public class AdminController : ControllerBase
    {
        private static readonly Logger logger = LogManager.GetLogger("AdminController");
        private readonly ILogRepository logRepository;

        public AdminController(ILogRepository _logRepository)
        {
            logRepository = _logRepository;
        }

        [HttpGet]
        [SwaggerResponse(400, "校验失败")]
        [SwaggerResponse(200, "成功获取日志", typeof(IList<LogMessageModel>))]
        [SwaggerOperation(
            Summary = "系统日志获取接口",
            Description = "使用此接口获取系统日志，需要Admin权限"
        )]
        public async Task<ActionResult<List<LogMessageModel>>> Logs([FromQuery] LogRequestModel model)
        {
            return await logRepository.GetLogs(model.Skip, model.Count, model.Level);
        }
    }
}
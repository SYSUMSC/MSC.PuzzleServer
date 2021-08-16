using Microsoft.AspNetCore.Mvc;
using MSC.Server.Middlewares;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private static readonly Logger logger = LogManager.GetLogger("AdminController");
        private readonly ILogRepository logRepository;

        public AdminController(ILogRepository _logRepository)
        {
            logRepository = _logRepository;
        }

        [HttpGet]
        [RequireAdmin]
        public async Task<ActionResult<List<LogMessageModel>>> Logs(LogRequestModel model)
        {
            if (!TryValidateModel(model))
                return BadRequest();

            return await logRepository.GetLogs(model.Skip, model.Count, model.Level);
        }
    }
}

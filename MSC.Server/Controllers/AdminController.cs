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
    public class AdminController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly AppDbContext context;
        private readonly ILogRepository logRepository;

        public AdminController(
            AppDbContext _context,
            ILogRepository _logRepository)
        {
            context = _context;
            logRepository = _logRepository;
        }

        [HttpPost("/api/admin/log")]
        [RequireAdmin]
        public async Task<IActionResult> GetLogs(LogRequestModel model)
        {
            if (!TryValidateModel(model))
                return new JsonResult(new { status = "Fail", msg = "请求无效!" });

            var res = await logRepository.GetLogs(model.Skip, model.Count, model.Level);
            var logs = from log in res select new LogMessageModel
                        {
                            Time = log.TimeUTC.ToLocalTime().ToString("M/d HH:mm:ss"),
                            IP = log.RemoteIP,
                            Msg = log.Message,
                            Status = log.Status,
                            UserName = log.UserName
                        };
            return new JsonResult(new { status = "OK", data = logs });
        }
    }
}

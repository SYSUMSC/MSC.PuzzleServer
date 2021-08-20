using Microsoft.AspNetCore.Mvc;
using MSC.Server.Middlewares;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;
using MSC.Server.Utils;
using NLog;
using System.Net.Mime;

namespace MSC.Server.Controllers;

/// <summary>
/// 管理员数据交互接口
/// </summary>
[RequireAdmin]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(typeof(RequestResponse), StatusCodes.Status401Unauthorized)]
[Route("api/[controller]/[action]")]
public class AdminController : ControllerBase
{
    private static readonly Logger logger = LogManager.GetLogger("AdminController");
    private readonly ILogRepository logRepository;

    public AdminController(ILogRepository _logRepository)
    {
        logRepository = _logRepository;
    }

    /// <summary>
    /// 系统日志获取接口
    /// </summary>
    /// <remarks>
    /// 使用此接口获取系统日志，需要Admin权限
    /// </remarks>
    /// <param name="model"></param>
    /// <param name="token">操作取消token</param>
    /// <response code="200">成功获取日志</response>
    /// <response code="400">校验失败</response>
    /// <response code="401">无权访问</response>
    [HttpGet]
    [ProducesResponseType(typeof(IList<LogMessageModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<LogMessageModel>>> Logs([FromQuery] LogRequestModel model, CancellationToken token)
    {
        return await logRepository.GetLogs(model.Skip, model.Count, model.Level, token);
    }
}
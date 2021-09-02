﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
[Route("api/[controller]/[action]")]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(typeof(RequestResponse), StatusCodes.Status401Unauthorized)]
public class AdminController : ControllerBase
{
    private static readonly Logger logger = LogManager.GetLogger("AdminController");
    private readonly ILogRepository logRepository;
    private readonly IRankRepository rankRepository;
    private readonly IMemoryCache cache;

    public AdminController(ILogRepository _logRepository,
        IMemoryCache memoryCache,
        IRankRepository _rankRepository)
    {
        cache = memoryCache;
        logRepository = _logRepository;
        rankRepository = _rankRepository;
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

    /// <summary>
    /// 检查用户提交并核对排名分数
    /// </summary>
    /// <param name="token">操作取消token</param>
    /// <response code="200">成功完成操作</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckRankScore(CancellationToken token)
    {
        await rankRepository.CheckRankScore(token);
        cache.Remove(CacheKey.ScoreBoard);
        return Ok();
    }
}
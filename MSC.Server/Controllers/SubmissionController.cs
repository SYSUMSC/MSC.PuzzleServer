using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSC.Server.Middlewares;
using MSC.Server.Models;
using MSC.Server.Repositories.Interface;
using MSC.Server.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace MSC.Server.Controllers
{
    /// <summary>
    /// 提交数据交互接口
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status401Unauthorized)]
    [Route("api/[controller]/[action]")]
    public class SubmissionController : ControllerBase
    {
        private static readonly Logger logger = LogManager.GetLogger("SubmissionController");
        private readonly UserManager<UserInfo> userManager;
        private readonly IRankRepository rankRepository;
        private readonly ISubmissionRepository submissionRepository;
        private readonly IPuzzleRepository puzzleRepository;

        public SubmissionController(
            UserManager<UserInfo> _userManager,
            IPuzzleRepository _puzzleRepository,
            ISubmissionRepository _submissionRepository,
            IRankRepository _rankRepository)
        {
            userManager = _userManager;
            rankRepository = _rankRepository;
            puzzleRepository = _puzzleRepository;
            submissionRepository = _submissionRepository;
        }

        /// <summary>
        /// 获取当前用户最新提交接口
        /// </summary>
        /// <remarks>
        /// 使用此接口获取当前用户最新提交，限制为10个，需要SignedIn权限
        /// </remarks>
        /// <param name="id">题目Id</param>
        /// <param name="token">操作取消token</param>
        /// <response code="200">成功获取提交</response>        
        /// <response code="401">无权访问</response>
        [HttpGet("/api/[controller]/{id}")]
        [RequireSignedIn]
        [ProducesResponseType(typeof(List<Submission>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SelfHistory(int id, CancellationToken token)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var submissions = await submissionRepository.GetSubmissions(token, 0, 10, id, userid);

            return Ok(submissions);
        }

        /// <summary>
        /// 获取全部用户最新提交接口
        /// </summary>
        /// <remarks>
        /// 使用此接口获取当前用户最新提交，限制为50个，需要Monitor权限
        /// </remarks>
        /// <param name="id">题目Id</param>
        /// <param name="token">操作取消token</param>
        /// <response code="200">成功获取提交</response>
        /// <response code="401">无权访问</response>
        [HttpGet("{id}")]
        [RequireMonitor]
        [ProducesResponseType(typeof(List<Submission>), StatusCodes.Status200OK)]
        public async Task<IActionResult> History(int id, CancellationToken token)
            => Ok(await submissionRepository.GetSubmissions(token, 0, 10, id));
    }
}

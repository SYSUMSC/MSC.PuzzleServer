using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSC.Server.Middlewares;
using MSC.Server.Models;
using MSC.Server.Repositories.Interface;
using NLog;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MSC.Server.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]/[action]")]
    [SwaggerTag("提交数据交互接口")]
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
        /// 获取当前用户最新提交API
        /// </summary>
        [HttpGet("{id}")]
        [RequireSignedIn]
        [SwaggerResponse(403, "无权访问")]
        [SwaggerResponse(200, "成功获取提交")]
        [SwaggerOperation(
            Summary = "获取当前用户最新提交接口",
            Description = "使用此接口获取当前用户最新提交，限制为10个"
        )]
        public async Task<IActionResult> My(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var submissions = await submissionRepository.GetSubmissions(0, 10, id, user.Id);

            return Ok(submissions);
        }

        /// <summary>
        /// 获取题目最新提交API
        /// </summary>
        [HttpGet("{id}")]
        [RequireMonitor]
        [SwaggerResponse(403, "无权访问")]
        [SwaggerResponse(200, "成功获取提交")]
        [SwaggerOperation(
            Summary = "获取全部用户最新提交接口",
            Description = "使用此接口获取当前用户最新提交，限制为50个"
        )]
        public async Task<IActionResult> Log(int id)
            => Ok(await submissionRepository.GetSubmissions(0, 10, id));
    }
}

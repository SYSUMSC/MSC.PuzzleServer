using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSC.Server.Middlewares;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;
using MSC.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace MSC.Server.Controllers
{
    /// <summary>
    /// 排名相关接口
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]/[action]")]
    public class InfoController : ControllerBase
    {
        private readonly UserManager<UserInfo> userManager;
        private readonly IRankRepository rankRepository;
        private readonly ISubmissionRepository submissionRepository;

        public InfoController(
            UserManager<UserInfo> _userManager,
            ISubmissionRepository _submissionRepository,
            IRankRepository _rankRepository)
        {
            userManager = _userManager;
            rankRepository = _rankRepository;
            submissionRepository = _submissionRepository;
        }

        /// <summary>
        /// 积分榜接口
        /// </summary>
        /// <remarks>
        /// 使用此接口获取积分榜，需要已登录权限
        /// </remarks>
        /// <param name="token">操作取消token</param>
        /// <response code="200">成功新建题目</response>
        [HttpGet]
        [RequireSignedIn]
        [ProducesResponseType(typeof(ScoreBoardMessageModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ScoreBoard(CancellationToken token)
        {
            ScoreBoardMessageModel result = new();
            result.Rank = await rankRepository.GetRank(token);

            var top10 = result.Rank.Take(10);
            foreach (var r in top10)
            {
                result.TopDetail.Add(new ScoreBoardTimeLine()
                {
                    UserName = r.UserName,
                    TimeLine = await submissionRepository.GetTimeLine(r.UserId, token)
                });
            }

            result.UpdateTime = DateTime.Now;

            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IRankRepository rankRepository;
        private readonly ISubmissionRepository submissionRepository;
        private readonly IMemoryCache cache;

        public InfoController(
            IMemoryCache memoryCache,
            ISubmissionRepository _submissionRepository,
            IRankRepository _rankRepository)
        {
            cache = memoryCache;
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
        /// <response code="200">成功获取积分榜</response>
        [HttpGet]
        [RequireSignedIn]
        [ProducesResponseType(typeof(ScoreBoardMessageModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ScoreBoard(CancellationToken token)
        {
            ScoreBoardMessageModel result;

            if (cache.TryGetValue(CacheKey.ScoreBoard, out result))
                return Ok(result);

            result = new();
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

            cache.Set(CacheKey.ScoreBoard, result, TimeSpan.FromHours(6));

            return Ok(result);
        }
    }
}

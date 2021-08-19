using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSC.Server.Middlewares;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;
using MSC.Server.Utils;
using NLog;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace MSC.Server.Controllers
{
    /// <summary>
    /// 题目数据交互接口
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]/[action]")]
    [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status401Unauthorized)]
    public class PuzzleController : ControllerBase
    {
        private static readonly Logger logger = LogManager.GetLogger("PuzzleController");
        private readonly UserManager<UserInfo> userManager;
        private readonly IRankRepository rankRepository;
        private readonly ISubmissionRepository submissionRepository;
        private readonly IPuzzleRepository puzzleRepository;

        public PuzzleController(
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
        /// 新建题目接口
        /// </summary>
        /// <remarks>
        /// 使用此接口添加新题目，需要Admin权限
        /// </remarks>
        /// <param name="model"></param>
        /// <param name="token">操作取消token</param>
        /// <response code="200">成功新建题目</response>
        /// <response code="400">校验失败</response>
        [HttpPost]
        [RequireAdmin]
        [ProducesResponseType(typeof(PuzzleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] PuzzleBase model, CancellationToken token)
        {
            var puzzle = await puzzleRepository.AddPuzzle(model, token);

            if (puzzle is null)
                return BadRequest(new RequestResponse("无效的题目"));

            return Ok(new PuzzleResponse(puzzle.Id));
        }

        /// <summary>
        /// 更新题目接口
        /// </summary>
        /// <remarks>
        /// 使用此接口更新题目，需要Admin权限
        /// </remarks>
        /// <param name="id">题目Id</param>
        /// <param name="model"></param>
        /// <param name="token">操作取消token</param>
        /// <response code="200">成功更新题目</response>
        /// <response code="400">校验失败</response>
        [HttpPut("{id}")]
        [RequireAdmin]
        [ProducesResponseType(typeof(PuzzleResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] PuzzleBase model, CancellationToken token)
        {
            var puzzle = await puzzleRepository.UpdatePuzzle(id, model, token);

            return Ok(new PuzzleResponse(puzzle.Id));
        }

        /// <summary>
        /// 获取题目接口
        /// </summary>
        /// <remarks>
        /// 使用此接口更新题目，需要SignedIn权限
        /// </remarks>
        /// <param name="id">题目Id</param>
        /// <param name="token">操作取消token</param>
        /// <response code="200">成功获取题目</response>
        /// <response code="401">无权访问或题目无效</response>
        [HttpGet("{id}")]
        [RequireSignedIn]
        [ProducesResponseType(typeof(UserPuzzleModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get(int id, CancellationToken token)
        {
            var user = await userManager.GetUserAsync(User);
            var puzzle = await puzzleRepository.GetUserPuzzle(id, user.AccessLevel, token);

            if (puzzle is null)
            {
                LogHelper.Log(logger, $"试图获取未授权题目#{id}", user, TaskStatus.Denied);
                return Unauthorized(new RequestResponse("无权访问或题目无效"));
            }

            return Ok(puzzle);
        }


        /// <summary>
        /// 删除题目接口
        /// </summary>
        /// <remarks>
        /// 使用此接删除题目，需要Admin权限
        /// </remarks>
        /// <param name="id">题目Id</param>
        /// <param name="token">操作取消token</param>
        /// <response code="200">成功删除题目</response>
        /// <response code="400">题目删除失败</response>
        [HttpDelete("{id}")]
        [RequireAdmin]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id, CancellationToken token)
        {
            var (res, title) = await puzzleRepository.DeletePuzzle(id, token);

            if(!res)
                return BadRequest(new RequestResponse("题目删除失败"));

            var user = await userManager.GetUserAsync(User);

            LogHelper.Log(logger, $"删除题目#{id} {title}", user, TaskStatus.Success);

            return Ok();
        }

        /// <summary>
        /// 提交题目答案接口
        /// </summary>
        /// <remarks>
        /// 使用此接口提交题目答案，此接口限制为3次每60秒，需要SignedIn权限
        /// </remarks>
        /// <param name="id">题目Id</param>
        /// <param name="answer">题目答案</param>
        /// <param name="token">操作取消token</param>
        /// <response code="200">成功获取题目</response>
        /// <response code="400">错误的答案</response>
        /// <response code="401">无权访问或题目无效</response>
        [HttpPost("{id}")]
        [RequireSignedIn]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Submit(int id, [FromBody] string answer, CancellationToken token)
        {
            var user = await userManager.Users.Include(u => u.Rank)
                .SingleAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await puzzleRepository.VerifyAnswer(id, answer, user.AccessLevel, token);

            await submissionRepository.AddSubmission(id, user.Id, answer, result, token);

            if (result.Result == AnswerResult.Unauthorized)
            {
                LogHelper.Log(logger, "提交未授权的题目。", user, TaskStatus.Denied);
                return Unauthorized(new RequestResponse("无权访问或题目无效", 401));
            }

            if (result.Result == AnswerResult.WrongAnswer)
            {
                LogHelper.Log(logger, "答案错误：[" + answer + "]", user, TaskStatus.Fail);
                return BadRequest(new RequestResponse("答案错误"));
            }

            if (user.Rank is null)
                user.Rank = new Rank() { UserId = user.Id };

            await rankRepository.UpdateRank(user.Rank, result.Score, token);

            LogHelper.Log(logger, "答案正确：[" + answer + "]", user, TaskStatus.Success);

            return Ok();
        }
    }
}
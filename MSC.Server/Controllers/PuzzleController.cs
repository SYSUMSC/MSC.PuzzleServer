using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSC.Server.Middlewares;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;
using MSC.Server.Utils;
using NLog;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MSC.Server.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]/[action]")]
    [SwaggerTag("题目数据交互接口")]
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
        /// 新建题目API
        /// </summary>
        [HttpPost]
        [RequireAdmin]
        [SwaggerResponse(400, "校验失败")]
        [SwaggerResponse(200, "成功新建题目", typeof(PuzzleResponse))]
        [SwaggerOperation(
            Summary = "新建题目接口",
            Description = "使用此接口添加新题目"
        )]
        public async Task<IActionResult> Add([FromBody] PuzzleBase model)
        {
            var puzzle = await puzzleRepository.AddPuzzle(model);

            if (puzzle is null)
                return BadRequest(new RequestResponse("无效的题目"));

            return Ok(new PuzzleResponse(puzzle.Id));
        }

        /// <summary>
        /// 更新题目API
        /// </summary>
        [HttpPut("{id}")]
        [RequireAdmin]
        [SwaggerResponse(400, "校验失败")]
        [SwaggerResponse(200, "成功更新题目", typeof(PuzzleResponse))]
        [SwaggerOperation(
            Summary = "更新题目接口",
            Description = "使用此接口更新题目"
        )]
        public async Task<IActionResult> Update(int id, [FromBody] PuzzleBase model)
        {
            var puzzle = await puzzleRepository.UpdatePuzzle(id, model);

            return Ok(new PuzzleResponse(puzzle.Id));
        }

        /// <summary>
        /// 获取题目API
        /// </summary>
        [HttpGet("{id}")]
        [RequireSignedIn]
        [SwaggerResponse(401, "无权访问或题目无效")]
        [SwaggerResponse(200, "成功获取题目", typeof(UserPuzzleModel))]
        [SwaggerOperation(
            Summary = "获取题目接口",
            Description = "使用此接口获取题目"
        )]
        public async Task<IActionResult> Get(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var puzzle = await puzzleRepository.GetUserPuzzle(id, user.AccessLevel);

            if (puzzle is null)
                return Unauthorized(new RequestResponse("无权访问或题目无效"));

            return Ok(puzzle);
        }


        /// <summary>
        /// 删除题目API
        /// </summary>
        [HttpDelete("{id}")]
        [RequireAdmin]
        [SwaggerResponse(400, "题目删除失败")]
        [SwaggerResponse(200, "成功删除题目")]
        [SwaggerOperation(
            Summary = "删除题目接口",
            Description = "使用此接删除题目"
        )]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await puzzleRepository.DeletePuzzle(id);

            if(res)
                return Ok();

            return BadRequest(new RequestResponse("题目删除失败"));
        }

        /// <summary>
        /// 提交题目API
        /// </summary>
        [HttpPost("{id}")]
        [RequireSignedIn]
        [SwaggerResponse(400, "错误的答案")]
        [SwaggerResponse(401, "无权访问或题目无效")]
        [SwaggerResponse(200, "提交题目")]
        [SwaggerOperation(
            Summary = "提交题目答案接口",
            Description = "使用此接口提交题目答案，此接口限制为3次每60秒"
        )]
        public async Task<IActionResult> Submit(int id, [FromBody]string answer)
        {
            var user = await userManager.Users.Include(u => u.Rank)
                .SingleAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await puzzleRepository.VerifyAnswer(id, answer, user.AccessLevel);

            await submissionRepository.AddSubmission(id, user.Id, answer, result);

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

            await rankRepository.UpdateRank(user.Rank, result.Score);

            LogHelper.Log(logger, "答案正确：[" + answer + "]", user, TaskStatus.Success);

            return Ok();
        }
    }
}
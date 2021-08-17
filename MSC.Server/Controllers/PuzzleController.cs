using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSC.Server.Middlewares;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;
using MSC.Server.Utils.Response;
using NLog;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
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
        private readonly SignInManager<UserInfo> signInManager;
        private readonly IPuzzleRepository puzzleRepository;

        public PuzzleController(
            IPuzzleRepository _puzzleRepository,
            UserManager<UserInfo> _userManager,
            SignInManager<UserInfo> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            puzzleRepository = _puzzleRepository;
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
                return BadRequest(new BadRequestResponse("无效的题目。"));

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
        [SwaggerResponse(400, "校验失败")]
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
                return Unauthorized(new BadRequestResponse("无权访问。"));

            return Ok(puzzle);
        }
    }
}
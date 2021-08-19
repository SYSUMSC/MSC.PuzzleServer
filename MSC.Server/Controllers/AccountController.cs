using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSC.Server.Extensions;
using MSC.Server.Middlewares;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Services.Interface;
using MSC.Server.Utils;
using NLog;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MSC.Server.Controllers
{
    /// <summary>
    /// 用户账户相关接口
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private static readonly Logger logger = LogManager.GetLogger("AccountController");
        private readonly AppDbContext context;
        private readonly IMailSender mailSender;
        private readonly IRecaptchaExtension recaptcha;
        private readonly UserManager<UserInfo> userManager;
        private readonly SignInManager<UserInfo> signInManager;

        public AccountController(
            AppDbContext _context,
            IMailSender _mailSender,
            IRecaptchaExtension _recaptcha,
            UserManager<UserInfo> _userManager,
            SignInManager<UserInfo> _signInManager)
        {
            context = _context;
            recaptcha = _recaptcha;
            mailSender = _mailSender;
            userManager = _userManager;
            signInManager = _signInManager;
        }

        /// <summary>
        /// 用户注册接口
        /// </summary>
        /// <remarks>
        /// 使用此接口注册新用户
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="200">注册成功</response>
        /// <response code="400">校验失败或用户已存在</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!await recaptcha.VerifyAsync(model.GToken, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()))
                return BadRequest(new RequestResponse("Recaptcha校验未通过!"));

            await signInManager.SignOutAsync();
            var user = new UserInfo
            {
                UserName = model.UserName,
                Email = model.Email,
                Privilege = Privilege.User,
                LastSignedInUTC = DateTime.UtcNow,
                LastVisitedUTC = DateTime.UtcNow,
                RegisterTimeUTC = DateTime.UtcNow,
                Rank = new()
                {
                    UpdateTimeUTC = DateTime.UtcNow,
                }
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var current = await userManager.FindByEmailAsync(model.Email);

                if (current is null)
                    return BadRequest(new RequestResponse(result.Errors.FirstOrDefault().Description));

                if (await userManager.IsEmailConfirmedAsync(current))
                    return BadRequest(new RequestResponse("此账户已存在。"));

                user = current;
            }

            LogHelper.Log(logger, "发送用户邮箱验证邮件。", user, TaskStatus.Pending);

            mailSender.SendConfirmEmailUrl(user.UserName, user.Email,
                "https://" + HttpContext.Request.Host.ToString()
                + "/Account/Verify?token=" + Codec.Base64.Encode(await userManager.GenerateEmailConfirmationTokenAsync(user))
                + "&email=" + Codec.Base64.Encode(model.Email));

            return Ok();
        }

        /// <summary>
        /// 用户找回密码请求接口
        /// </summary>
        /// <remarks>
        /// 使用此接口请求找回密码，向用户邮箱发送邮件
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="200">用户密码重置邮件发送成功</response>
        /// <response code="400">校验失败</response>
        /// <response code="404">用户不存在</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Recovery([FromBody] RecoveryModel model)
        {
            if (!await recaptcha.VerifyAsync(model.GToken, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()))
                return BadRequest(new RequestResponse("Recaptcha校验未通过!"));

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return NotFound();

            LogHelper.Log(logger, "发送用户密码重置邮件。", user, TaskStatus.Pending);

            mailSender.SendResetPasswordUrl(user.UserName, user.Email,
                HttpContext.Request.Scheme + "://"
                + HttpContext.Request.Host.ToString()
                + "/Account/PasswordReset?token=" + Codec.Base64.Encode(await userManager.GeneratePasswordResetTokenAsync(user))
                + "&email=" + Codec.Base64.Encode(model.Email));

            return Ok();
        }

        /// <summary>
        /// 用户重置密码接口
        /// </summary>
        /// <remarks>
        /// 使用此接口重置密码，需要邮箱验证码
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="200">用户成功重置密码</response>
        /// <response code="400">校验失败</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetModel model)
        {
            if (!await recaptcha.VerifyAsync(model.GToken, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()))
                return BadRequest(new RequestResponse("Recaptcha校验未通过!"));

            var user = await userManager.FindByEmailAsync(Codec.Base64.Decode(model.Email));
            var result = await userManager.ResetPasswordAsync(user, Codec.Base64.Decode(model.RToken), model.Password);

            if (!result.Succeeded)
                return BadRequest(new RequestResponse(result.Errors.FirstOrDefault().Description));

            LogHelper.Log(logger, "用户成功重置密码。", user, TaskStatus.Success);

            return Ok();
        }

        /// <summary>
        /// 用户邮箱确认接口
        /// </summary>
        /// <remarks>
        /// 使用此接口通过邮箱验证码确认邮箱
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="200">用户通过邮箱验证</response>
        /// <response code="400">校验失败</response>
        /// <response code="401">邮箱验证失败</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Verify([FromBody] AccountVerifyModel model)
        {
            var user = await userManager.FindByEmailAsync(Codec.Base64.Decode(model.Email));
            var result = await userManager.ConfirmEmailAsync(user, Codec.Base64.Decode(model.Token));

            if (result.Succeeded)
            {
                LogHelper.Log(logger, "通过邮箱验证。", user, TaskStatus.Success);
                await signInManager.SignInAsync(user, true);
                return Ok();
            }

            return Unauthorized();
        }

        /// <summary>
        /// 用户登录接口
        /// </summary>
        /// <remarks>
        /// 使用此接口登录账户
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="200">用户成功登陆</response>
        /// <response code="400">校验失败</response>
        /// <response code="401">密码错误</response>
        /// <response code="404">用户不存在</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LogIn([FromBody] LoginModel model)
        {
            if (!await recaptcha.VerifyAsync(model.GToken, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()))
                return BadRequest();

            await signInManager.SignOutAsync();

            var user = await userManager.FindByEmailAsync(model.UserName);
            if (user is null)
                user = await userManager.FindByNameAsync(model.UserName);

            if (user is null)
                return NotFound();

            user.LastSignedInUTC = DateTime.UtcNow;
            await context.SaveChangesAsync();

            var result = await signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (!result.Succeeded)
                return Unauthorized();

            LogHelper.Log(logger, "用户成功登录。", user, TaskStatus.Success);

            return Ok();
        }

        /// <summary>
        /// 用户登出接口
        /// </summary>
        /// <remarks>
        /// 使用此接口登出账户，需要SignedIn权限
        /// </remarks>
        /// <response code="200">用户已登出</response>
        /// <response code="401">无权访问</response>
        [HttpPost]
        [RequireSignedIn]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return Ok();
        }

        /// <summary>
        /// 用户数据更新接口
        /// </summary>
        /// <remarks>
        /// 使用此接口更新用户用户名和描述，需要SignedIn权限
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="200">用户数据成功更新</response>
        /// <response code="400">校验失败或用户数据更新失败</response>
        /// <response code="401">无权访问</response>
        [HttpPut]
        [RequireSignedIn]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] ProfileUpdateModel model)
        {
            var user = await userManager.GetUserAsync(User);
            var oname = user.UserName;
            user.UserName = model.UserName;
            user.Description = model.Des;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new RequestResponse(result.Errors.FirstOrDefault().Description));

            if (oname != model.UserName)
                LogHelper.Log(logger, "用户更新：" + oname + "=>" + model.UserName, user, TaskStatus.Success);

            return Ok();
        }

        /// <summary>
        /// 用户密码更改接口
        /// </summary>
        /// <remarks>
        /// 使用此接口更新用户密码，需要SignedIn权限
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="200">用户成功更新密码</response>
        /// <response code="400">校验失败或用户密码更新失败</response>
        /// <response code="401">无权访问</response>
        [HttpPut]
        [RequireSignedIn]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeModel model)
        {
            var user = await userManager.GetUserAsync(User);
            var result = await userManager.ChangePasswordAsync(user, model.Old, model.New);

            if (!result.Succeeded)
                return BadRequest(new RequestResponse(result.Errors.FirstOrDefault().Description));

            LogHelper.Log(logger, "用户更新密码。", user, TaskStatus.Success);

            return Ok();
        }

        /// <summary>
        /// 用户邮箱更改接口
        /// </summary>
        /// <remarks>
        /// 使用此接口更改用户邮箱，需要SignedIn权限
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="200">成功发送用户邮箱更改邮件</response>
        /// <response code="400">校验失败或邮箱已经被占用</response>
        /// <response code="401">无权访问</response>
        [HttpPut]
        [RequireSignedIn]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangeEmail([FromBody] MailChangeModel model)
        {
            if (await userManager.FindByEmailAsync(model.NewMail) is not null)
                return BadRequest(new RequestResponse("邮箱已经被占用。"));

            var user = await userManager.GetUserAsync(User);
            LogHelper.Log(logger, "发送用户邮箱更改邮件。", user, TaskStatus.Pending);

            mailSender.SendChangeEmailUrl(user.UserName, model.NewMail,
                HttpContext.Request.Scheme + "://"
                + HttpContext.Request.Host.ToString()
                + "/Account/ChangeEmail?token=" + Codec.Base64.Encode(await userManager.GenerateChangeEmailTokenAsync(user, model.NewMail))
                + "&email=" + Codec.Base64.Encode(model.NewMail));

            return Ok();
        }

        /// <summary>
        /// 用户邮箱更改确认接口
        /// </summary>
        /// <remarks>
        /// 使用此接口确认更改用户邮箱，需要邮箱验证码，需要SignedIn权限
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="200">用户成功更改邮箱</response>
        /// <response code="400">校验失败或无效邮箱</response>
        /// <response code="401">无权访问</response>
        [HttpPost]
        [RequireSignedIn]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> MailChangeConfirm([FromBody] AccountVerifyModel model)
        {
            var user = await userManager.GetUserAsync(User);
            var result = await userManager.ChangeEmailAsync(user, Codec.Base64.Decode(model.Email), Codec.Base64.Decode(model.Token));

            if (!result.Succeeded)
                return BadRequest(new RequestResponse("无效邮箱。"));

            LogHelper.Log(logger, "更改邮箱成功。", user, TaskStatus.Success);

            return Ok();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MSC.Server.Models;
using MSC.Server.Utils;
using NLog;
using System.Security.Claims;

namespace MSC.Server.Middlewares;

/// <summary>
/// 需要权限访问
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequirePrivilegeAttribute : Attribute, IAsyncAuthorizationFilter
{
    /// <summary>
    /// 所需权限
    /// </summary>
    private readonly Privilege RequiredPrivilege;

    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public RequirePrivilegeAttribute(Privilege privilege)
        => RequiredPrivilege = privilege;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        AppDbContext dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        UserInfo? currentUser = await dbContext.Users.FirstOrDefaultAsync(i => i.Id == userId);

        if (currentUser is null)
        {
            var result = new JsonResult(new RequestResponse("请先登录", 401));
            result.StatusCode = 401;
            context.Result = result;
            return;
        }

        //this method will be only called here
        currentUser.UpdateByHttpContext(context.HttpContext);
        await dbContext.SaveChangesAsync();

        if (currentUser.Privilege < RequiredPrivilege)
        {
            var result = new JsonResult(new RequestResponse("无权访问", 401));
            result.StatusCode = 401;
            context.Result = result;
        }
    }
}

/// <summary>
/// 需要已登录用户权限
/// </summary>
public class RequireSignedInAttribute : RequirePrivilegeAttribute
{
    public RequireSignedInAttribute() : base(Privilege.User)
    {
    }
}

/// <summary>
/// 需要监控者权限
/// </summary>
public class RequireMonitorAttribute : RequirePrivilegeAttribute
{
    public RequireMonitorAttribute() : base(Privilege.Monitor)
    {
    }
}

/// <summary>
/// 需要Admin权限
/// </summary>
public class RequireAdminAttribute : RequirePrivilegeAttribute
{
    public RequireAdminAttribute() : base(Privilege.Admin)
    {
    }
}

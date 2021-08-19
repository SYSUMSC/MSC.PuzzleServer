using MSC.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using NLog;
using MSC.Server.Utils;

namespace MSC.Server.Middlewares
{
    /// <summary>
    /// 需要权限访问
    /// </summary>
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
            AppDbContext dbContext = (AppDbContext)context.HttpContext.RequestServices.GetService(typeof(AppDbContext));
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserInfo currentUser = await dbContext.Users.FirstOrDefaultAsync(i => i.Id == userId);

            if (currentUser is null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            //this method will be only called here
            currentUser.UpdateByHttpContext(context.HttpContext);
            await dbContext.SaveChangesAsync();

            if (currentUser.Privilege < RequiredPrivilege)
                context.Result = new UnauthorizedResult();
        }
    }

    /// <summary>
    /// 需要已登录用户权限
    /// </summary>
    public class RequireSignedInAttribute : RequirePrivilegeAttribute
    {
        public RequireSignedInAttribute() : base(Privilege.User) { }
    }

    /// <summary>
    /// 需要监控者权限
    /// </summary>
    public class RequireMonitorAttribute : RequirePrivilegeAttribute
    {
        public RequireMonitorAttribute() : base(Privilege.Monitor) { }
    }

    /// <summary>
    /// 需要管理员权限
    /// </summary>
    public class RequireAdminAttribute : RequirePrivilegeAttribute
    {
        public RequireAdminAttribute() : base(Privilege.Admin) { }
    }
}

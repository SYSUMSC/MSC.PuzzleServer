using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Models
{
    public class UserInfo : IdentityUser
    {
        /// <summary>
        /// 用户权限
        /// </summary>
        public Privilege Privilege { get; set; } = Privilege.User;

        /// <summary>
        /// 用户最近访问IP
        /// </summary>
        public string IP { get; set; } = "0.0.0.0";

        /// <summary>
        /// 用户最近登录时间
        /// </summary>
        public DateTime LastSignedInUTC { get; set; } = DateTime.Parse("1970-01-01T00:00:00");

        /// <summary>
        /// 用户最近访问时间
        /// </summary>
        public DateTime LastVisitedUTC { get; set; } = DateTime.Parse("1970-01-01T00:00:00");

        /// <summary>
        /// 用户注册时间
        /// </summary>
        public DateTime RegisterTimeUTC { get; set; } = DateTime.Parse("1970-01-01T00:00:00");

        /// <summary>
        /// 个性签名
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 通过Http请求更新用户最新访问时间和IP
        /// </summary>
        /// <param name="context"></param>
        public void UpdateByHttpContext(HttpContext context)
        {
            var remoteAddress = context.Connection.RemoteIpAddress;
            if (remoteAddress.IsIPv4MappedToIPv6)
                IP = remoteAddress.MapToIPv4().ToString();
            else
                IP = remoteAddress.MapToIPv6().ToString().Replace("::ffff:", "");

            LastVisitedUTC = DateTime.UtcNow;
        }
    }
}

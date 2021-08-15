using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MSC.Server.Models.Data;

namespace MSC.Server.Models
{
    public class AppDbContext : IdentityDbContext<UserInfo>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<LogModel> Logs { get; set; }
    }
}

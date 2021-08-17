using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MSC.Server.Models
{
    public class AppDbContext : IdentityDbContext<UserInfo>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<LogModel> Logs { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Puzzle> Puzzles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserInfo>(entity =>
            {
                entity.Property(e => e.Privilege)
                    .HasConversion<int>();

                entity.HasOne(e => e.Rank)
                    .WithOne(e => e.User)
                    .HasForeignKey<Rank>(e => e.UserId);

                entity.HasMany(e => e.Processes)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId);

                entity.HasMany(e => e.Submissions)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId);
            });

            builder.Entity<Puzzle>(entity =>
            {
                entity.HasMany(e => e.Processes)
                    .WithOne(e => e.Puzzle)
                    .HasForeignKey(e => e.PuzzleId);

                entity.HasMany(e => e.Submissions)
                    .WithOne(e => e.Puzzle)
                    .HasForeignKey(e => e.PuzzleId);
            });

        }
    }
}

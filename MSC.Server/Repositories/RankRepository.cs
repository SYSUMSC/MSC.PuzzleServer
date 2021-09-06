using Microsoft.EntityFrameworkCore;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;

namespace MSC.Server.Repositories;

public class RankRepository : RepositoryBase, IRankRepository
{
    public RankRepository(AppDbContext context) : base(context)
    {
    }

    public Task CheckRankScore(CancellationToken token)
    {
        var users = context.Users.Include(u => u.Submissions).Include(u => u.Rank);

        foreach (UserInfo user in users)
        {
            int currentScore = 0;
            HashSet<int> puzzleIds = new();

            foreach (var sub in user.Submissions)
            {
                if (!puzzleIds.Contains(sub.PuzzleId))
                {
                    currentScore += sub.Score;
                    puzzleIds.Add(sub.PuzzleId);
                }
            }

            user.Rank!.Score = currentScore;
        }

        return context.SaveChangesAsync(token);
    }

    public Task<List<RankMessageModel>> GetRank(CancellationToken token, int skip = 0, int count = 100)
        => (from rank in context.Ranks.OrderByDescending(r => r.Score)
                .Skip(skip).Take(count).Include(r => r.User)
            select new RankMessageModel
            {
                Score = rank.Score,
                UpdateTime = rank.UpdateTimeUTC,
                UserName = rank.User!.UserName,
                Descr = rank.User.Description,
                UserId = rank.UserId
            }).ToListAsync(token);

    public async Task UpdateRank(Rank rank, int score, CancellationToken token)
    {
        rank.UpdateTimeUTC = DateTime.UtcNow;
        rank.Score += score;
        await context.SaveChangesAsync(token);
    }
}

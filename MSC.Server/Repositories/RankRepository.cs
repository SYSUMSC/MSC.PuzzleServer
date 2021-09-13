﻿using Microsoft.EntityFrameworkCore;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;
using MSC.Server.Utils;
using NLog;

namespace MSC.Server.Repositories;

public class RankRepository : RepositoryBase, IRankRepository
{
    private static readonly Logger logger = LogManager.GetLogger("RankRepository");

    public RankRepository(AppDbContext context) : base(context)
    {
    }

    public async Task CheckRankScore(CancellationToken token)
    {
        LogHelper.SystemLog(logger, "开始检查排名分数", TaskStatus.Pending);

        var users = context.Users.Include(u => u.Submissions).Include(u => u.Rank);

        foreach (UserInfo user in users)
        {
            int currentScore = 0;
            HashSet<int> puzzleIds = new();

            foreach (var sub in user.Submissions.OrderByDescending(s => s.Score))
            {
                if (!puzzleIds.Contains(sub.PuzzleId))
                {
                    currentScore += sub.Score;
                    puzzleIds.Add(sub.PuzzleId);
                }
            }

            user.Rank!.Score = currentScore;

            LogHelper.SystemLog(logger, $"{user.UserName} 的分数为：{currentScore}", TaskStatus.Pending);
        }

        await context.SaveChangesAsync(token);
        LogHelper.SystemLog(logger, "检查排名分数完成");
    }

    public Task<List<RankMessageModel>> GetRank(CancellationToken token, int skip = 0, int count = 100)
        => (from rank in context.Ranks.OrderByDescending(r => r.Score)
                .Include(r => r.User).ThenInclude(u => u!.Submissions)
                .Where(r => r.User!.Privilege == Privilege.User)
                .Skip(skip).Take(count)
            select new RankMessageModel
            {
                Score = rank.Score,
                UpdateTime = rank.UpdateTimeUTC,
                UserName = rank.User!.UserName,
                Descr = rank.User.Description,
                UserId = rank.UserId,
                User = rank.User
            }).ToListAsync(token);

    public async Task UpdateRank(Rank rank, int score, CancellationToken token)
    {
        rank.UpdateTimeUTC = DateTimeOffset.UtcNow;
        rank.Score += score;
        await context.SaveChangesAsync(token);
        LogHelper.SystemLog(logger, $"增加分数：{rank.User!.UserName} => {score}");
    }
}

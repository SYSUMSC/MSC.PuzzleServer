﻿using Microsoft.EntityFrameworkCore;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;
using MSC.Server.Utils;
using NLog;

namespace MSC.Server.Repositories;

public class SubmissionRepository : RepositoryBase, ISubmissionRepository
{
    private static readonly Logger logger = LogManager.GetLogger("SubmissionRepository");
    private readonly IConfiguration configuration;

    public SubmissionRepository(AppDbContext context, IConfiguration config) : base(context)
    {
        configuration = config;
    }

    public async Task AddSubmission(Submission sub, CancellationToken token)
    {
        await context.AddAsync(sub, token);
        await context.SaveChangesAsync(token);
    }

    public async Task<HashSet<int>> GetSolvedPuzzles(string userId, CancellationToken token)
        => (await (from sub in context.Submissions.Where(s => s.Solved && s.UserId == userId)
                    select sub.PuzzleId).ToListAsync(token)).ToHashSet();

    public Task<List<Submission>> GetSubmissions(CancellationToken token, int skip = 0, int count = 50, int puzzleId = 0, string userId = "All")
    {
        IQueryable<Submission> result = context.Submissions;

        if (puzzleId > 0)
            result = result.Where(s => s.PuzzleId == puzzleId);

        if (userId != "All")
            result = result.Where(s => s.UserId == userId);

        return result.OrderByDescending(s => s.SubmitTimeUTC).Skip(skip).Take(count).ToListAsync(token);
    }

    public List<TimeLineModel> GetTimeLine(UserInfo user, CancellationToken token)
    {
        if (user is null || user.Submissions is null)
            return new();

        int currentScore = 0;
        HashSet<int> puzzleIds = new();
        List<TimeLineModel> result = new();

        var success = DateTimeOffset.TryParse(configuration["StartTime"], out DateTimeOffset start);

        result.Add(new TimeLineModel
        {
            Time = (success && start > user.RegisterTimeUTC) ? start : user.RegisterTimeUTC,
            TotalScore = 0,
            PuzzleId = -1,
        });

        foreach (var sub in user.Submissions.OrderBy(s => s.SubmitTimeUTC))
        {
            if (!puzzleIds.Contains(sub.PuzzleId) && sub.Score != 0)
            {
                currentScore += sub.Score;
                result.Add(new TimeLineModel()
                {
                    PuzzleId = sub.PuzzleId,
                    Time = sub.SubmitTimeUTC,
                    TotalScore = currentScore
                });
                puzzleIds.Add(sub.PuzzleId);
            }
        }

        return result;
    }

    public Task<bool> HasSubmitted(int puzzleId, string userId, CancellationToken token)
        => context.Submissions.AnyAsync(s => s.PuzzleId == puzzleId && s.UserId == userId && s.Solved, token);
}

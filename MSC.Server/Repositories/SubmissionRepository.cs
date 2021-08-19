using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;
using MSC.Server.Utils;

namespace MSC.Server.Repositories
{
    public class SubmissionRepository : RepositoryBase, ISubmissionRepository
    {
        public SubmissionRepository(AppDbContext context) : base(context) { }

        public async Task AddSubmission(int puzzleId, string userid, string answer, VerifyResult result, CancellationToken token)
        {
            Submission sub = new()
            {
                UserId = userid,
                PuzzleId = puzzleId,
                Answer = answer,
                Solved = result.Result == AnswerResult.Accepted,
                Score = result.Score
            };
            await context.AddAsync(sub, token);
            await context.SaveChangesAsync(token);
        }

        public Task<List<Submission>> GetSubmissions(CancellationToken token, int skip = 0, int count = 50, int puzzleId = 0, string userId = "All")
        {
            IQueryable<Submission> result = context.Submissions;
            
            if (puzzleId > 0)
                result = result.Where(s => s.PuzzleId == puzzleId);

            if(userId != "All")
                result = result.Where(s => s.UserId == userId);

            return result.Skip(skip).Take(count).ToListAsync(token);
        }

        public Task<List<TimeLineModel>> GetTimeLine(string userId, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasSubmitted(int puzzleId, string userId, CancellationToken token)
            => context.Submissions.AnyAsync(s => s.PuzzleId == puzzleId && s.UserId == userId, token);
    }
}

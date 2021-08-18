using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSC.Server.Models;
using MSC.Server.Repositories.Interface;
using MSC.Server.Utils;

namespace MSC.Server.Repositories
{
    public class SubmissionRepository : RepositoryBase, ISubmissionRepository
    {
        public SubmissionRepository(AppDbContext context) : base(context) { }

        public async Task AddSubmission(int puzzleId, string userid, string answer, VerifyResult result)
        {
            Submission sub = new()
            {
                UserId = userid,
                PuzzleId = puzzleId,
                Answer = answer,
                Solved = result.Result == AnswerResult.Accepted,
                Score = result.Score
            };
            await context.AddAsync(sub);
            await context.SaveChangesAsync();
        }
    }
}

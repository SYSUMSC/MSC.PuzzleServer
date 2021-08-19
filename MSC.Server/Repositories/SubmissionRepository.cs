﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public Task<List<Submission>> GetSubmissions(int skip = 0, int count = 50, int puzzleId = 0, string userId = "All")
        {
            IQueryable<Submission> result = context.Submissions;
            
            if (puzzleId > 0)
                result = result.Where(s => s.PuzzleId == puzzleId);

            if(userId != "All")
                result = result.Where(s => s.UserId == userId);

            return result.Skip(skip).Take(count).ToListAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using MSC.Server.Utils;
using NLog;

namespace MSC.Server.Repositories
{
    public class PuzzleRepository : RepositoryBase, IPuzzleRepository
    {
        public PuzzleRepository(AppDbContext context) : base(context) { }

        public async Task<Puzzle> AddPuzzle(PuzzleBase newPuzzle)
        {
            Puzzle puzzle = new(newPuzzle);

            await context.AddAsync(puzzle);
            await context.SaveChangesAsync();

            return puzzle;
        }

        public async Task<(bool result, string title)> DeletePuzzle(int id)
        {
            Puzzle puzzle = await context.Puzzles.FirstOrDefaultAsync(x => x.Id == id);

            if (puzzle is null)
                return (false, string.Empty);

            string title = puzzle.Title;

            context.Remove(puzzle);
            await context.SaveChangesAsync();

            return (true, title);
        }

        public async Task<UserPuzzleModel> GetUserPuzzle(int id, int accessLevel)
        {
            Puzzle puzzle = await context.Puzzles.FirstOrDefaultAsync(x => x.Id == id);

            if (puzzle is null || puzzle.AccessLevel > accessLevel)
                return null;

            return new UserPuzzleModel
            {
                Title = puzzle.Title,
                Content = puzzle.Content,
                SolvedCount = puzzle.SolvedCount
            };
        }

        public async Task<Puzzle> UpdatePuzzle(int id, PuzzleBase newPuzzle)
        {
            Puzzle puzzle = await context.Puzzles.FirstOrDefaultAsync(x => x.Id == id);

            puzzle.Update(newPuzzle);
            await context.SaveChangesAsync();

            return puzzle;
        }

        public async Task<VerifyResult> VerifyAnswer(int id, string answer, int accessLevel)
        {
            if (string.IsNullOrWhiteSpace(answer))
                return new VerifyResult();

            Puzzle puzzle = await context.Puzzles.FirstOrDefaultAsync(x => x.Id == id);

            if (puzzle is null || puzzle.AccessLevel > accessLevel)
                return new VerifyResult(AnswerResult.Unauthorized);

            bool check = string.Equals(puzzle.Answer, answer.Trim());

            if (check)
                return new VerifyResult(AnswerResult.Accepted, puzzle.CurrentScore);

            return new VerifyResult(AnswerResult.WrongAnswer);
        }
    }
}

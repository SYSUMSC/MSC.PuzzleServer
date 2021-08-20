using Microsoft.EntityFrameworkCore;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;
using MSC.Server.Utils;

namespace MSC.Server.Repositories
{
    public class PuzzleRepository : RepositoryBase, IPuzzleRepository
    {
        public PuzzleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Puzzle> AddPuzzle(PuzzleBase newPuzzle, CancellationToken token)
        {
            Puzzle puzzle = new(newPuzzle);

            await context.AddAsync(puzzle, token);
            await context.SaveChangesAsync(token);

            return puzzle;
        }

        public async Task<(bool result, string title)> DeletePuzzle(int id, CancellationToken token)
        {
            Puzzle? puzzle = await context.Puzzles.FirstOrDefaultAsync(x => x.Id == id, token);

            if (puzzle is null)
                return (false, string.Empty);

            string title = puzzle.Title!;

            context.Remove(puzzle);
            await context.SaveChangesAsync(token);

            return (true, title);
        }

        public async Task<List<int>> GetAccessiblePuzzles(int accessLevel, CancellationToken token)
        {
            var puzzles = await context.Puzzles.Where(p => p.AccessLevel <= accessLevel).ToListAsync(token);

            List<int> puzzleList = new();

            foreach (var p in puzzles)
                puzzleList.Add(p.Id);

            return puzzleList;
        }

        public int GetMaxAccessLevel()
            => context.Puzzles.Max(p => p.AccessLevel);

        public async Task<UserPuzzleModel?> GetUserPuzzle(int id, int accessLevel, CancellationToken token)
        {
            Puzzle? puzzle = await context.Puzzles.FirstOrDefaultAsync(x => x.Id == id, token);

            if (puzzle is null || puzzle.AccessLevel > accessLevel)
                return null;

            return new UserPuzzleModel
            {
                Title = puzzle.Title,
                Content = puzzle.Content,
                SolvedCount = puzzle.SolvedCount
            };
        }

        public async Task<Puzzle?> UpdatePuzzle(int id, PuzzleBase newPuzzle, CancellationToken token)
        {
            Puzzle? puzzle = await context.Puzzles.FirstOrDefaultAsync(x => x.Id == id, token);

            if (puzzle is null)
                return null;

            puzzle.Update(newPuzzle);
            await context.SaveChangesAsync(token);

            return puzzle;
        }

        public async Task UpdateSolvedCount(int id, CancellationToken token)
        {
            Puzzle? puzzle = await context.Puzzles.FirstOrDefaultAsync(x => x.Id == id, token);

            if (puzzle is null)
                return;

            ++puzzle.SolvedCount;

            await context.SaveChangesAsync(token);
        }

        public async Task<VerifyResult?> VerifyAnswer(int id, string answer, int accessLevel, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(answer))
                return new VerifyResult();

            Puzzle? puzzle = await context.Puzzles.FirstOrDefaultAsync(x => x.Id == id, token);

            if (puzzle is null || puzzle.AccessLevel > accessLevel)
                return new VerifyResult(AnswerResult.Unauthorized);

            bool check = string.Equals(puzzle.Answer, answer.ToUpper());

            if (check)
                return new VerifyResult(AnswerResult.Accepted, puzzle.CurrentScore);

            return new VerifyResult(AnswerResult.WrongAnswer);
        }
    }
}
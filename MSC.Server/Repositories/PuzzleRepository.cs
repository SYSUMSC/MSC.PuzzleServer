using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

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
    }
}

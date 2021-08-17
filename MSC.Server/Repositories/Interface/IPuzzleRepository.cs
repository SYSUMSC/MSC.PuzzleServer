using MSC.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Repositories.Interface
{
    public interface IPuzzleRepository
    {
        public Task<Puzzle> AddPuzzle(Puzzle puzzle);
    }
}

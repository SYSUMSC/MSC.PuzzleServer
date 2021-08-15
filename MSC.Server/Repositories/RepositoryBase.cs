using MSC.Server.Models;

namespace MSC.Server.Repositories
{
    public class RepositoryBase
    {
        protected readonly AppDbContext context;
        public RepositoryBase(AppDbContext _context)
            => context = _context;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSC.Server.Models;
using MSC.Server.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace MSC.Server.Repositories
{
    public class LogRepository : RepositoryBase, ILogRepository
    {
        public LogRepository(AppDbContext context) : base(context) { }

        public Task<List<LogModel>> GetLogs(int skip, int count, string level)
        {
            IQueryable<LogModel> data = context.Logs;
            if (level != "All")
                data = data.Where(x => x.Level == level);
            return data.OrderByDescending(x => x.TimeUTC).Skip(skip).Take(count).ToListAsync();
        }
    }
}

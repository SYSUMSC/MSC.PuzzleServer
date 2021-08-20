using Microsoft.EntityFrameworkCore;
using MSC.Server.Models;
using MSC.Server.Repositories.Interface;

namespace MSC.Server.Repositories;

public class LogRepository : RepositoryBase, ILogRepository
{
    public LogRepository(AppDbContext context) : base(context)
    {
    }

    public Task<List<LogMessageModel>> GetLogs(int skip, int count, string level, CancellationToken token)
    {
        IQueryable<LogModel> data = context.Logs;
        if (level != "All")
            data = data.Where(x => x.Level == level);
        data = data.OrderByDescending(x => x.TimeUTC).Skip(skip).Take(count);

        return (from log in data
                select new LogMessageModel
                {
                    Time = log.TimeUTC.ToLocalTime().ToString("M/d HH:mm:ss"),
                    IP = log.RemoteIP,
                    Msg = log.Message,
                    Status = log.Status,
                    UserName = log.UserName
                }).ToListAsync(token);
    }
}

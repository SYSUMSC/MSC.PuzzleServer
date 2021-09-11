using Microsoft.AspNetCore.SignalR;
using MSC.Server.Extensions;
using MSC.Server.Hubs;
using MSC.Server.Hubs.Interface;
using MSC.Server.Models;
using NLog;

namespace MSC.Server.Services;

public class SignalRLoggingService : IDisposable
{
    private bool disposed = false;
    private IHubContext<LoggingHub, ILoggingClient> Hub { get; set; }
    public SignalRTarget? Target { get; set; }

    public SignalRLoggingService(IHubContext<LoggingHub, ILoggingClient> _Hub)
    {
        Hub = _Hub;
        Target = SignalRTarget.Instance;
        if (Target is not null)
            Target.LogEventHandler += OnLog;
    }

    ~SignalRLoggingService()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (!disposed)
        {
            disposed = true;
            if (Target is not null)
                Target.LogEventHandler -= OnLog;
            GC.SuppressFinalize(this);
        }
    }

    public async void OnLog(LogEventInfo logInfo)
    {
        if(logInfo.Level >= NLog.LogLevel.Error)
        {
            await Hub.Clients.All.ReceivedLog(
                new LogMessageModel
                {
                    Time = logInfo.TimeStamp,
                    UserName = "System",
                    IP = "-",
                    Msg = logInfo.Message,
                    Status = (string)logInfo.Properties["status"]
                });
        }
        else if(logInfo.Level >= NLog.LogLevel.Info)
        {
            await Hub.Clients.All.ReceivedLog(
                new LogMessageModel
                {
                    Time = logInfo.TimeStamp,
                    UserName = (string)logInfo.Properties["uname"],
                    IP = (string)logInfo.Properties["ip"],
                    Msg = logInfo.Message,
                    Status = (string)logInfo.Properties["status"]
                });
        }
    }
}

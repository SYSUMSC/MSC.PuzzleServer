using Microsoft.AspNetCore.SignalR;
using MSC.Server.Extensions;
using MSC.Server.Hubs;
using MSC.Server.Hubs.Interface;
using MSC.Server.Hubs.Models;
using NLog;
using System;

namespace MSC.Server.Services
{
    public class SignalRLoggingService : IDisposable
    {
        private IHubContext<LoggingHub, ILoggingClient> Hub { get; set; }
        public SignalRTarget Target { get; set; }

        public SignalRLoggingService(IHubContext<LoggingHub, ILoggingClient> _Hub)
        {
            Hub = _Hub;
            Target = SignalRTarget.Instance;
            if (Target is not null)
                Target.LogEventHandler += OnLog;
        }

        public void Dispose()
        {
            if (Target is not null)
                Target.LogEventHandler -= OnLog;
        }

        public async void OnLog(LogEventInfo logInfo)
        {
            try
            {
                await Hub.Clients.All.RecivedLog(
                    new LogMessageToSend
                    {
                        Time = logInfo.TimeStamp.ToLocalTime().ToString("M/d HH:mm:ss"),
                        UserName = (string)logInfo.Properties["uname"],
                        IP = (string)logInfo.Properties["ip"],
                        Msg = logInfo.Message,
                        Status = (string)logInfo.Properties["status"]
                    });
            }
            catch (Exception) { }
        }
    }
}
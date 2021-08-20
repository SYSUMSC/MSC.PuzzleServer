using Microsoft.AspNetCore.SignalR;
using MSC.Server.Hubs.Interface;
using MSC.Server.Services;
using MSC.Server.Utils;

namespace MSC.Server.Hubs
{
    public class LoggingHub : Hub<ILoggingClient>
    {
        private readonly SignalRLoggingService loggingEmitter;

        // This is the way to init LoggingEmitter.
        public LoggingHub(SignalRLoggingService emitter)
        {
            loggingEmitter = emitter;
        }

        public override async Task OnConnectedAsync()
        {
            if (!await HubHelper.HasAdmin(Context.GetHttpContext()!))
                Context.Abort();

            await base.OnConnectedAsync();
        }
    }
}
using NLog;
using NLog.Targets;

namespace MSC.Server.Extensions
{
    [Target("SignalR")]
    public class SignalRTarget : TargetWithLayout
    {
        public SignalRTarget() => Instance = this;

        public delegate void OnLog(LogEventInfo e);

        public event OnLog? LogEventHandler;

        public static SignalRTarget? Instance { get; private set; }

        protected override void Write(LogEventInfo logEvent)
            => LogEventHandler?.Invoke(logEvent);
    }
}
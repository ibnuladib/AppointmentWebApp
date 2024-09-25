using AppointmentWebApp.Hubs;
using Microsoft.AspNetCore.SignalR;
using static System.Reflection.Metadata.BlobBuilder;

namespace AppointmentWebApp.Services
{
    public class InMemoryAuditLog
    {
        public static List<string> Logs { get; } = new List<string>();
        private readonly IHubContext<LogHub> _hubContext;

        public InMemoryAuditLog(IHubContext<LogHub> hubContext)
        {
            _hubContext = hubContext;

        }
        public async Task Log(string log)
        {
            // Adding timestamp for each log entry.
            Logs.Add($"{DateTime.Now}: {log}");
            await _hubContext.Clients.All.SendAsync("ReceiveLogUpdate", log);
        }

        public static List<string> GetLogs()
        {
            return Logs;
        }
    }

}

namespace AppointmentWebApp.Models
{
    public static class InMemoryAuditLog
    {
        public static List<string> Logs { get; } = new List<string>();

        public static void Log(string message)
        {
            // Adding timestamp for each log entry.
            Logs.Add($"{DateTime.Now}: {message}");
        }
    }

}

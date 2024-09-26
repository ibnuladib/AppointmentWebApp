namespace AppointmentWebApp.Models
{
    public class AdminViewModel
    {
        public List<string> Logs { get; set; } // Example property for logs
        public List<AppointmentCount> AppointmentCounts { get; set; } // Ensure this matches your appointment count model
    }

}

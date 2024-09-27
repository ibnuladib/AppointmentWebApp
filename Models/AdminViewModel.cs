using System.Transactions;

namespace AppointmentWebApp.Models
{
    public class AdminViewModel
    {
        public List<string> Logs { get; set; } 
        public List<AppointmentCount> AppointmentCounts { get; set; } 

        public List<TransactionCount> TransactionCounts { get; set; }
    }

}

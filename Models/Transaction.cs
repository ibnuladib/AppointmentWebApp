using System.ComponentModel.DataAnnotations;

namespace AppointmentWebApp.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AppointmentId { get; set; }
        [Required]
        public Appointment? Appointment { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public DateTime TransactionPaidDate { get; set; }
        public string PaymentMethod { get; set; }  // E.g., Credit Card, PayPal

        public string Status { get; set; } = "Unpaid";
    }
}

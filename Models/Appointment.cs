using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppointmentWebApp.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PatientId { get; set; }
        public virtual ApplicationUser? Patient { get; set; }
        [Required]
        public string DoctorId { get; set; }
        public ApplicationUser? Doctor { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan AppointmentDuration { get; set; }
        [Required]
        public string Symptoms { get; set; }
        [Required]
        public bool IsPaid { get; set; }
        public decimal Amount { get; set; }

/*       public DateTime? AppointmentCreated { get; set; } = DateTime.Now;*/

        public Appointment() {
            Status = "Up Coming";
            IsPaid = false;
        }

    }
}

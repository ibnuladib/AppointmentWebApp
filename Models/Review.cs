using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppointmentWebApp.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        [Required]
        public string DoctorId { get; set; }
        public ApplicationUser? Doctor { get; set; } // The doctor being rated
        public string PatientId { get; set; }
        public ApplicationUser? Patient { get; set; } // The patient who gives the rating

        [Range(1, 5)]
        public decimal Rating { get; set; } // Rating score between 1 and 5
        public string Comment { get; set; } // Optional comments for the review
    }
}

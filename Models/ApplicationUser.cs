using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        // General Attributes
        public string? FirstName { get; set; } // Nullable
        public string? LastName { get; set; } // Nullable
        public string? Address { get; set; } // Nullable
        public string? Gender { get; set; } // Nullable
        public DateTime? DateOfBirth { get; set; } // Nullable
        public string? ProfilePicturePath { get; set; } // Nullable

        // Add IFormFile to handle file uploads (not saved in the database)
        [NotMapped]
        public IFormFile? ProfilePicture { get; set; } // Nullable

        public DateTime DateOfAccountCreation { get; set; }
        public DateTime? LastLoginTime { get; set; } // Nullable

        public ApplicationUser()
        {
            DateOfAccountCreation = DateTime.Now;
        }

        // Doctor-specific attributes
        public string? Specialization { get; set; } // Nullable
        public string? Qualification { get; set; } // Nullable
        public string? MedicalLicenseNumber { get; set; } // Nullable
        public DateTime? VisitingTimeStart { get; set; } // Nullable
        public DateTime? VisitingTimeEnd { get; set; } // Nullable
        public double? AverageRating { get; set; } // Nullable
        public decimal? ConsultationFeesPerHour { get; set; } // Nullable
        public int? YearsOfExperience { get; set; } // Nullable
        public int? TotalAppointments { get; set; } // Nullable
        public List<Review>? Reviews { get; set; } // Nullable

        // Patient-specific attributes
        public string? BloodGroup { get; set; } // Nullable
        public string? MedicalHistory { get; set; } // Nullable
        public string? InsuranceDetails { get; set; } // Nullable
    }
}

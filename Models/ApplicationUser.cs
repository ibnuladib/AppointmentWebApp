using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentWebApp.Models
{
    public class ApplicationUser: IdentityUser
    {
        //General Attributes
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public string ProfilePicturePath { get; set; } // This will store the file path

        // Add IFormFile to handle file uploads (not saved in the database)
        [NotMapped]
        public IFormFile ProfilePicture { get; set; } // This is for handling file upload

        public DateTime DateOfAccountCreation { get; set; }
        public DateTime LastLoginTime { get; set; }

        public ApplicationUser()
        {
            DateOfAccountCreation = DateTime.Now;
        }

        // Doctor-specific attributes
        public string Specialization { get; set; }
        public string Qualification { get; set; }
        public string MedicalLicenseNumber { get; set; }
        public DateTime VisitingTimeStart { get; set; }
        public DateTime VisitingTimeEnd { get; set; }
        public double AverageRating { get; set; }
        public decimal ConsultationFeesPerHour { get; set; }
        public int YearsOfExperience { get; set; }
        public int TotalAppointments { get; set; }
        public List<Review> Reviews { get; set; }


        //Patient

        public string BloodGroup { get; set; }
        public string MedicalHistory { get; set; }
        public string InsuranceDetails { get; set; }

    }
}

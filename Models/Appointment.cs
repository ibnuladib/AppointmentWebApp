using AppointmentWebApp.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentWebApp.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public string? PatientId { get; set; } // Foreign key for Patient
        public  ApplicationUser? Patient { get; set; }

        public string? DoctorId { get; set; } // Foreign key for Doctor
        public  ApplicationUser? Doctor { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Upcoming";

        public DateOnly DateOfAppointment { get; set; }
        public TimeSpan StartTime { get; set; }

        public DateTime AppointmentDate { get; set; } 

        [StringLength(255)]
        public string Symptoms { get; set; } // Optional symptom details

        public bool IsPaid { get; set; } = false; // Payment status of the appointment

        [Range(0, 10000)]
        public decimal Amount { get; set; } // Appointment cost

        public DoctorShift DoctorShift { get; set; } 

        public DateTime AppointmentCreated { get; set; } = DateTime.Now; 




    }
}

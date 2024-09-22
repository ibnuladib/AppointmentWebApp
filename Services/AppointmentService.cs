using AppointmentWebApp.Data;
using AppointmentWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentWebApp.Services
{
    public class AppointmentService
    {
        private readonly ApplicationDbContext _context;
            
        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            // Fetch patient and doctor from the database
            var patient = await _context.Users.FindAsync(appointment.PatientId);
            var doctor = await _context.Users.FindAsync(appointment.DoctorId);

            if (patient == null || doctor == null)
            {
                throw new ArgumentException("Patient or Doctor not found.");
            }

            // Set related entities
            appointment.Patient = patient;
            appointment.Doctor = doctor;

            try
            {
                // Add the appointment to the context
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                // Create and add the corresponding transaction
                var transaction = new Transaction
                {
                    AppointmentId = appointment.Id,
                    Amount = appointment.Amount,
                    TransactionDate = DateTime.Now,
                    Status = "Unpaid",
                    PaymentMethod = string.Empty,
                    Appointment = appointment
                };

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                // Return the created appointment
                return appointment;
            }
            catch (Exception ex)
            {
                // Handle exceptions and log the error
                throw new InvalidOperationException($"An error occurred while creating the appointment: {ex.Message}", ex);
            }
        }
    }
}

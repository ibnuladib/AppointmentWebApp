using AppointmentWebApp.Data;
using AppointmentWebApp.Migrations;
using AppointmentWebApp.Models;
using Azure.Core;
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
            var patient = await _context.Users.FindAsync(appointment.PatientId);
            var doctor = await _context.Users.FindAsync(appointment.DoctorId);

            DateTime appointmentDateTime = appointment.DateOfAppointment.ToDateTime(TimeOnly.MinValue).Add(appointment.StartTime);

            if (patient == null || doctor == null)
            {
                throw new ArgumentException("Patient or Doctor not found.");
            }
            appointment.Patient = patient;
            appointment.Doctor = doctor;
            appointment.IsPaid = false;
            appointment.AppointmentCreated = DateTime.Now;
            appointment.AppointmentDate = appointmentDateTime;
            try
            {
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

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

                return appointment;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while creating the appointment: {ex.Message}", ex);
            }
        }
    }
}

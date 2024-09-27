using AppointmentWebApp.Controllers;
using AppointmentWebApp.Data;
using AppointmentWebApp.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using System.Drawing.Text;
using System.Numerics;

namespace AppointmentWebApp.Services
{
    public class AppointmentCheckerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AppointmentCheckerService> _logger;
        private readonly AppointmentsController appointmentsController;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
      //  private readonly InMemoryAuditLog _inMemoryAuditLog;


        public AppointmentCheckerService(IServiceProvider serviceProvider, ILogger<AppointmentCheckerService> logger, IHubContext<NotificationHub> notificationHub) { 
            _serviceProvider = serviceProvider;
            _logger = logger;
            _notificationHubContext = notificationHub;
           // _inMemoryAuditLog = inMemoryAuditLog;   
        }

        protected override async Task ExecuteAsync(CancellationToken stop)
        {
            _logger.LogInformation("Execute Check Started");
            while (!stop.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        var timenow = DateTime.Now;

                        // Fetch all non-completed appointments
                        var incompleteAppointments = await dbContext.Appointments
                            .Where(a => a.Status != "Completed")
                            .ToListAsync();

                        // Filter ended appointments in memory
                        var endedAppointments = incompleteAppointments
                            .Where(a => a.AppointmentDate.AddMinutes(a.AppointmentDuration.TotalMinutes) <= timenow)
                            .ToList();

                        foreach (var appointment in endedAppointments)
                        {
                            appointment.Status = "Completed";
                            var doctor = await dbContext.Users.FindAsync(appointment.DoctorId);
                            var patient = await dbContext.Users.FindAsync(appointment.PatientId);
                            if (doctor != null)
                            {
                                doctor.TotalAppointments++;
                                dbContext.Users.Update(doctor);
                                string message = $"Completed an appointment with {patient.FirstName} {patient.LastName}";
                                await _notificationHubContext.Clients.User(appointment.DoctorId).SendAsync("ReceiveNotification", message);
                                string message1 = "Your History has been Updated";
                                await _notificationHubContext.Clients.User(appointment.PatientId).SendAsync("ReceiveNotification", message1);
                               // await _inMemoryAuditLog.Log($" ID: {patient.Id}, Email: {patient.Email} has completed an appointment with ID: {doctor.Id}, Email: {doctor.Email} .");
                            }
                            dbContext.Appointments.Update(appointment);
                            _logger.LogInformation($"Appointment ID {appointment.Id} has ended. Doctor's total appointments updated.");
                        }
                        await dbContext.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing appointments");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stop);
            }
        }
    }
}

    


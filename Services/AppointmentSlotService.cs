using AppointmentWebApp.Data;
using AppointmentWebApp.Enums;
using AppointmentWebApp.Models;
using Microsoft.AspNetCore.Identity;

public class AppointmentSlotService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AppointmentSlotService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<List<string>> GetAvailableTimeSlots(DateOnly appointmentDate, string doctorId)
    {
        ApplicationUser doctor = await _userManager.FindByIdAsync(doctorId);
        if (doctor == null)
        {
            throw new ArgumentException("Doctor not found.");
        }

        var existingAppointments = _context.Appointments
            .Where(a => a.DateOfAppointment == appointmentDate && a.DoctorId == doctorId)
            .ToList();

        TimeSpan shiftStartTime, shiftEndTime;

        if (doctor.Shift == DoctorShift.Morning)
        {
            shiftStartTime = new TimeSpan(8, 0, 0); // 8 AM
            shiftEndTime = new TimeSpan(14, 0, 0); // 2 PM
        }
        else // Evening shift
        {
            shiftStartTime = new TimeSpan(16, 0, 0); // 4 PM
            shiftEndTime = new TimeSpan(22, 0, 0); // 10 PM
        }

        List<string> availableSlots = new List<string>();

        for (var time = shiftStartTime; time < shiftEndTime; time = time.Add(TimeSpan.FromHours(1)))
        {
            if (!existingAppointments.Any(a => a.StartTime.Hours == time.Hours))
            {
                DateTime dateTime = DateTime.Today.Add(time);
                availableSlots.Add(dateTime.ToString("hh:mm tt")); 
            }
        }

        return availableSlots;
    }

}

using AppointmentWebApp.Data;
using AppointmentWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentWebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DashboardController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Doctor"))
            {
                var totalIncome = await _context.Transactions
                    .Where(t => t.Appointment.DoctorId == user.Id)
                    .SumAsync(t => t.Amount);

                var totalPatientsVisited = await _context.Appointments
                    .Where(a => a.DoctorId == user.Id)
                    .CountAsync();

                var averageRating = await _context.Reviews
                    .Where(r => r.DoctorId == user.Id)
                    .AverageAsync(r => (double?)r.Rating) ?? 0;

                var latestAppointments = await _context.Appointments
                    .Where(a => a.DoctorId == user.Id)
                    .Include(a => a.Patient)
                    .OrderByDescending(a => a.AppointmentDate)
                    .Take(5)
                    .ToListAsync();

                ViewBag.Doctor = user;  // Pass the doctor (ApplicationUser)
                ViewBag.TotalIncome = totalIncome;
                ViewBag.TotalPatientsVisited = totalPatientsVisited;
                ViewBag.AverageRating = averageRating;
                ViewBag.LatestAppointments = latestAppointments;

                return View("DoctorDashboard");
            }
            else if (roles.Contains("Patient"))
            {
                var allAppointments = await _context.Appointments
                    .Where(a => a.PatientId == user.Id)
                    .Include(a => a.Doctor)
                    .ToListAsync();

                var lastAppointment = allAppointments
                    .OrderByDescending(a => a.AppointmentDate)
                    .FirstOrDefault();

                ViewBag.Patient = user;  // Pass the patient (ApplicationUser)
                ViewBag.LastAppointment = lastAppointment;
                ViewBag.AllAppointments = allAppointments;

                return View("PatientDashboard");
            }

            return View("AccessDenied");
        }
    }
}

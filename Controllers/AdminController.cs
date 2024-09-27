using Microsoft.AspNetCore.Mvc;
using AppointmentWebApp.Models; // Ensure this is included for InMemoryAuditLog
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AppointmentWebApp.Data;
using AppointmentWebApp.Services;

namespace AppointmentWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;
        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // This action method returns the logs to a partial view
        public IActionResult _PartialLogs()
        {
            var logs = InMemoryAuditLog.GetLogs(); // Replace with actual log retrieval
            return PartialView("_PartialLogs", logs);
        }

        public async Task<IActionResult> Index()
        {
            var logs = InMemoryAuditLog.GetLogs(); // Assuming this returns a List<string>
            var appointmentCounts = await GetDailyAppointmentCountsAsync(DateTime.Today.AddDays(-7), DateTime.Today);

            var viewModel = new AdminViewModel
            {
                Logs = logs,
                AppointmentCounts = appointmentCounts
            };

            return View(viewModel); // Pass the view model to the main view
        }


        public async Task<List<AppointmentCount>> GetDailyAppointmentCountsAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Appointments
                .Where(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate)
                .GroupBy(a => a.AppointmentDate.Date) // Group by the date only
                .Select(g => new AppointmentCount
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(ac => ac.Date)
                .ToListAsync();
        }

        public async Task<IActionResult> _PartialGraph()
        {
            var today = DateTime.Today;
            var appointmentCounts = await GetDailyAppointmentCountsAsync(today.AddDays(-7), today); // Last 7 days

            // Debugging: Log or check the count
            if (appointmentCounts == null || !appointmentCounts.Any())
            {
                // Optionally, log this to console or a logger
                _logger.LogInformation("No appointment counts available.");
            }

            return PartialView("_PartialGraph", appointmentCounts); // Ensure appointmentCounts is of type List<AppointmentCount>
        }

    }
}

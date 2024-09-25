using Microsoft.AspNetCore.Mvc;
using AppointmentWebApp.Models; // Ensure this is included for InMemoryAuditLog
using System.Collections.Generic;
using AppointmentWebApp.Services;

namespace AppointmentWebApp.Controllers
{
    public class AdminController : Controller
    {
        // This action method returns the logs to a partial view
        public IActionResult _PartialLogs()
        {
            // Get logs from the in-memory log storage
            var logs = InMemoryAuditLog.GetLogs();

            // Pass the logs to the partial view
            return PartialView("_PartialLogs", logs);
        }

        public IActionResult Index()
        {
            var logs = InMemoryAuditLog.GetLogs();
            return View(logs);
        }
    }
}

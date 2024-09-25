using Microsoft.AspNetCore.Mvc;
using AppointmentWebApp.Models;

namespace AppointmentWebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Logs()
        {
            // Passing the logs to the view.
            return View(InMemoryAuditLog.Logs);
        }
    }
}

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
            var logs = InMemoryAuditLog.GetLogs(); 
            var appointmentCounts = await GetDailyAppointmentCountsAsync(DateTime.Today.AddDays(-7), DateTime.Today.AddDays(1));
            var transactionCounts = await GetDailyTransactionCountsAsync(DateTime.Today.AddDays(-7), DateTime.Today.AddDays(1));

            var viewModel = new AdminViewModel
            {
                Logs = logs,
                AppointmentCounts = appointmentCounts,
                TransactionCounts = transactionCounts
            };

            return View(viewModel);
        }


        public async Task<List<AppointmentCount>> GetDailyAppointmentCountsAsync(DateTime startDate, DateTime endDate)
        {
            var allDates = Enumerable.Range(0, (endDate - startDate).Days )
                                      .Select(offset => startDate.AddDays(offset).Date)
                                      .ToList();

            var appointmentData = await _context.Appointments
                .Where(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate)
                .GroupBy(a => a.AppointmentDate.Date)  
                .Select(g => new AppointmentCount
                {
                    Date = g.Key,        
                    Count = g.Count()     
                })
                .ToListAsync();

            var completeData = allDates.GroupJoin(
                appointmentData,
                date => date,
                appointment => appointment.Date,
                (date, appointments) => appointments.FirstOrDefault() ?? new AppointmentCount
                {
                    Date = date,
                    Count = 0
                })
                .OrderBy(ac => ac.Date)
                .ToList();

            return completeData;
        }


        public async Task<IActionResult> _PartialGraph()
        {
            var today = DateTime.Today;
            var appointmentCounts = await GetDailyAppointmentCountsAsync(today.AddDays(-7), today.AddDays(1));

            if (appointmentCounts == null || !appointmentCounts.Any())
            {
                _logger.LogInformation("No appointment counts available.");
            }

            return PartialView("_PartialGraph", appointmentCounts); 
        }


        public async Task<IActionResult> _PartialChart()
        {
            var today = DateTime.Today;
            var transactionCounts = await GetDailyTransactionCountsAsync(today.AddDays(-7), today);

            if (transactionCounts == null || !transactionCounts.Any())
            {
                _logger.LogInformation("No transaction counts available for the last 7 days.");

            }

            return PartialView("_PartialChart", transactionCounts);
        }

        public async Task<List<TransactionCount>> GetDailyTransactionCountsAsync(DateTime startDate, DateTime endDate)
        {
            var allDates = Enumerable.Range(0, (endDate - startDate).Days)
                                      .Select(offset => startDate.AddDays(offset).Date)
                                      .ToList();

            var transactionData = await _context.Transactions
                .Where(a => a.TransactionPaidDate >= startDate && a.TransactionPaidDate <= endDate)
                .GroupBy(a => a.TransactionPaidDate.Date)  
                .Select(g => new TransactionCount
                {
                    Date = g.Key,                 
                    Count = g.Count(),        
                    TotalEarnings = g.Sum(a => a.Amount) 
                })
                .ToListAsync();

            var completeData = allDates.GroupJoin(
                transactionData,
                date => date,
                transaction => transaction.Date,
                (date, transactions) => transactions.FirstOrDefault() ?? new TransactionCount
                {
                    Date = date,
                    Count = 0,
                    TotalEarnings = 0
                })
                .OrderBy(ac => ac.Date)
                .ToList();

            return completeData;
        }





    }
}

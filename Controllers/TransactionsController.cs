using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppointmentWebApp.Models;
using AppointmentWebApp.Data;
using AppointmentWebApp.Services;
using System.Numerics;
using System.Security.Claims;

namespace AppointmentWebApp.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly InMemoryAuditLog _inMemoryAuditLog;
        public TransactionsController(ApplicationDbContext context, InMemoryAuditLog inMemoryAuditLog)
        {
            _context = context;
            _inMemoryAuditLog = inMemoryAuditLog;   
        }

        // GET: Transactions
        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm = null, string statusFilter = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID
            IQueryable<Transaction> transactionsQuery = _context.Transactions
                .Include(t => t.Appointment)
                    .ThenInclude(a => a.Patient) // Include Patient details
                .Include(t => t.Appointment)
                    .ThenInclude(a => a.Doctor); // Include Doctor details

            // Check if the user is an Admin
            bool isAdmin = User.IsInRole("Admin");

            // Filter transactions based on the user's role
            if (!isAdmin)
            {
                // If not admin, filter transactions involving the specific patient or doctor
                transactionsQuery = transactionsQuery.Where(t =>
                    t.Appointment.PatientId == userId || t.Appointment.DoctorId == userId);
            }

            // Search by Patient/Doctor FirstName + LastName, Patient/Doctor Id, TransactionId
            if (!string.IsNullOrEmpty(searchTerm))
            {
                transactionsQuery = transactionsQuery.Where(t =>
                    t.Appointment.Patient.FirstName.Contains(searchTerm) ||
                    t.Appointment.Patient.LastName.Contains(searchTerm) ||
                    t.Appointment.Doctor.FirstName.Contains(searchTerm) ||
                    t.Appointment.Doctor.LastName.Contains(searchTerm) ||
                    t.Appointment.PatientId.ToString() == searchTerm ||
                    t.Appointment.DoctorId.ToString() == searchTerm ||
                    (t.Appointment.Patient.FirstName + " " + t.Appointment.Patient.LastName).Contains(searchTerm) ||
                    (t.Appointment.Doctor.FirstName + " " + t.Appointment.Doctor.LastName).Contains(searchTerm) ||
                    t.Appointment.Patient.Email.ToString() == searchTerm ||
                    t.Appointment.Doctor.Email.ToString() == searchTerm ||
                    t.Id.ToString() == searchTerm);
            }

            // If a status filter is provided, apply it
            if (!string.IsNullOrEmpty(statusFilter))
            {
                transactionsQuery = transactionsQuery.Where(t => t.Status == statusFilter);
            }

            return View(await transactionsQuery.ToListAsync());
        }


        [HttpGet]
        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Appointment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "Id", "Id");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppointmentId,Amount,TransactionDate,PaymentMethod,Status")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "Id", "Id", transaction.AppointmentId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "Id", "Id", transaction.AppointmentId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppointmentId,Amount,TransactionDate,PaymentMethod,Status")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "Id", "Id", transaction.AppointmentId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Appointment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTransactionStatus(int Id, [FromBody] Transaction transaction)
        {
            // Fetch the existing transaction from the database using the given ID
            var existingTransaction = await _context.Transactions.FindAsync(Id);
            var existingAppointment = await _context.Appointments.FindAsync(Id);
            if (existingTransaction == null)
            {
                return NotFound("Transaction not found");
            }

            // Update the status to "Paid" if the existing status is not already "Paid"
            if (existingTransaction.Status == "Paid")
            {
                return BadRequest("Transaction is already marked as 'Paid'");
            }

            existingTransaction.Status = "Paid";
            existingTransaction.PaymentMethod = transaction.PaymentMethod;
            if (existingAppointment != null)
            {
                existingAppointment.IsPaid = true;
            }
            existingTransaction.TransactionPaidDate = DateTime.Now;
            _context.Transactions.Update(existingTransaction);

            // Save changes to the database
            await _context.SaveChangesAsync();
            await _inMemoryAuditLog.Log($" Transaction {existingTransaction.Id} has been paid for Appointment, ID:{existingTransaction.AppointmentId}");


            return Ok("Transaction status updated successfully");
        }

    }
}

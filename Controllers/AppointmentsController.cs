using AppointmentWebApp.Data;
using AppointmentWebApp.Hubs;
using AppointmentWebApp.Models;
using AppointmentWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentWebApp.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly AppointmentService _appointmentService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly InMemoryAuditLog _inMemoryAuditLog;

        public AppointmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, AppointmentService appointmentService, IHubContext<NotificationHub> notificationHubContext, InMemoryAuditLog inMemoryAuditLog ) /*IHubContext<NotificationHub> notificationHubContext*/
        {
            _appointmentService = appointmentService;
            _context = context;
            _userManager = userManager;
            _notificationHubContext = notificationHubContext;
            _inMemoryAuditLog = inMemoryAuditLog;
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }


        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if (await _userManager.IsInRoleAsync(user, "Patient"))
            {
                var appointments = await _context.Appointments
                    .Where(a => a.PatientId == userId)
                    .Include(a => a.Doctor)
                    .OrderByDescending(a => a.AppointmentDate) 
                    .ToListAsync();

                return View(appointments);
            }
            else if (await _userManager.IsInRoleAsync(user, "Doctor"))
            {
                var appointments = await _context.Appointments
                    .Where(a => a.DoctorId == userId)
                    .Include(a => a.Patient)
                    .OrderByDescending(a => a.AppointmentDate) 
                    .ToListAsync();
                return View(appointments);
            }
            else
            {
                var appointments = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToListAsync();

                return View(appointments);
            }
        }


        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                //  .Include(a => a.Transaction)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        [Authorize(Roles = "Admin, Patient")]

        // GET: Appointments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Use the AppointmentService to create the appointment and transaction
                    var createdAppointment = await _appointmentService.CreateAppointmentAsync(appointment);

                    var doctorId = appointment.DoctorId;
                    var patientId = appointment.PatientId;
                    var doctor = await _userManager.FindByIdAsync(doctorId);
                    var patient = await _userManager.FindByIdAsync(patientId);
                    if (doctor != null && patient != null)
                    {
                        string message = $"You have a new appointment with {patient.UserName} on {appointment.AppointmentDate}.\n Check Appointments for details";
                        await _notificationHubContext.Clients.User(doctorId).SendAsync("ReceiveNotification", message);
                        string message1 = "You have a new Transaction.";
                        await _notificationHubContext.Clients.User(patientId).SendAsync("ReceiveNotification", message1);
                    }
                    await _inMemoryAuditLog.Log($" ID: {patientId}, Email: {patient.Email} booked an appointment with ID: {doctorId}, Email: {doctor.Email}");
                    // Redirect to a confirmation page or the index page
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(appointment);
        }

/*        public IActionResult CreateFromDoctor()
        {
            return View();
        }*/

        public async Task<IActionResult> CreateFromDoctor(string? doctorId = null, string? patientId = null)
        {
            var patient = await _userManager.FindByIdAsync(patientId);
            var doctor = await _userManager.FindByIdAsync(doctorId);
            var amount = doctor.ConsultationFeesPerHour;
            var appointment = new Appointment
            {
                DoctorId = doctorId,
                PatientId = patientId,
                Amount = (decimal)amount,
                Doctor = doctor,
                Patient = patient

            };

            return View(appointment);  // Ensure the `Appointment` model is passed here.
        }



        [Authorize(Roles = "Admin, Patient")]

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
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

            return View(appointment);
        }



    [Authorize(Roles = "Admin")]

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                //.Include(a => a.Transaction)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

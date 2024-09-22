using AppointmentWebApp.Data;
using AppointmentWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppointmentWebApp.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ReviewController> _logger;
        public ReviewController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<ReviewController> logger)
        {
            _context = context; 
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Review review)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingReview = await _context.Reviews
                        .FirstOrDefaultAsync(r => r.DoctorId == review.DoctorId && r.PatientId == _userManager.GetUserId(User));

                    if (existingReview != null)
                    {
                        return Json(new { success = false, message = "You have already reviewed the Doctor" });
                    }

                      // Call to update average rating
                    _context.Add(review);
                    await _context.SaveChangesAsync();
                    await UpdateAverageRating(review.DoctorId);
                    return Json(new { success = true, message = "Review submitted successfully!" });
                }

                return Json(new { success = false, message = "Invalid review data." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating review for Doctor ID: {DoctorId}", review.DoctorId);
                return Json(new { success = false, message = "An error occurred while submitting the review." });
            }
        }

       
        public async Task UpdateAverageRating(string doctorId)
        {
            try
            {
                var reviews = await _context.Reviews.Where(r => r.DoctorId == doctorId).ToListAsync();
                var averageRating = reviews.Average(r => r.Rating);

                var doctor = await _userManager.FindByIdAsync(doctorId);
                if (doctor == null)
                {
                    _logger.LogInformation("Doctor not found.");
                    return;
                }

                doctor.AverageRating = averageRating;

                _context.Users.Update(doctor);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Doctor's average rating updated to {AverageRating}.", averageRating);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating average rating for Doctor ID: {DoctorId}", doctorId);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRecentComments()
        {
            var recentComments = await _context.Reviews
                //.OrderByDescending(r => r.Date) // Assuming you have a Date property
                .Take(10)
                .ToListAsync();

            var patientIds = recentComments.Select(r => r.PatientId).Distinct();
            var patients = new List<ApplicationUser>();

            foreach (var patientId in patientIds)
            {
                var patient = await _userManager.FindByIdAsync(patientId);
                if (patient != null)
                {
                    patients.Add(patient);
                }
            }

            var viewModel = new CommentViewModel
            {
                Comments = recentComments,
                Patients = patients
            };

            return PartialView("_RecentCommentsPartial", viewModel);
        }





    }
}

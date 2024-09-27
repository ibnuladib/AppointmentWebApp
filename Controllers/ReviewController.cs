using AppointmentWebApp.Data;
using AppointmentWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppointmentWebApp.Services;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AppointmentWebApp.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ReviewController> _logger;
        private readonly InMemoryAuditLog _inMemoryAuditLog;
        public ReviewController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<ReviewController> logger, InMemoryAuditLog inMemoryAuditLog)
        {
            _context = context; 
            _userManager = userManager;
            _logger = logger;
            _inMemoryAuditLog = inMemoryAuditLog;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Review review)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var patientId = _userManager.GetUserId(User);
                    var patient = await _userManager.GetUserAsync(User);
                    

                    var existingReview = await _context.Reviews
                        .FirstOrDefaultAsync(r => r.DoctorId == review.DoctorId && r.PatientId == patientId);

                    if (existingReview != null)
                    {
                        return Json(new { success = false, message = "You have already reviewed the Doctor" });
                    }

                    review.PatientId = patientId;
                    review.DateCreated = DateTime.Now;

                    _context.Add(review);
                    await _context.SaveChangesAsync();
                    await UpdateAverageRating(review.DoctorId);
                    await _inMemoryAuditLog.Log($" ID: {patientId}, Email: {patient.Email} booked an appointment with ID: {review.DoctorId}, Email: {review.Doctor.Email}");
                    return Json(new { success = true, message = "Review submitted successfully!" });
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    _logger.LogWarning("Model state is invalid: {Errors}", string.Join(", ", errors));
                    return Json(new { success = false, message = "Invalid review data.", errors });
                }
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
        public async Task<IActionResult> GetRecentComments(string doctorId)
        {
            var recentComments = await _context.Reviews
                .Where(r => r.DoctorId == doctorId)
                .OrderByDescending(r => r.DateCreated)
                .Take(10)
                .ToListAsync();

            var patientIds = recentComments.Select(r => r.PatientId).Distinct();
            var patients = await _userManager.Users
                .Where(u => patientIds.Contains(u.Id))
                .ToListAsync();

            var viewModel = new CommentViewModel
            {
                Comments = recentComments,
                Patients = patients
            };

            return PartialView("_RecentCommentsPartial", viewModel);
        }
        public async Task<IActionResult> Index()
        {
            var reviews = _context.Reviews.ToListAsync();    
            return View(await reviews);

        }

        public async Task<IActionResult> HasReview(string doctorId)
        {
            var patientId = _userManager.GetUserId(User);
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.DoctorId == doctorId && r.PatientId == patientId);

            if (review != null)
            {
                return Json(new { success = true, reviewId = review.ReviewId, rating = review.Rating, comment = review.Comment });
            }

            return Json(new { success = false });
        }

        [HttpPut]
        public async Task<IActionResult> Update(int reviewId, [FromBody] Review review)
        {
            try
            {
                _logger.LogInformation("Updating review with ID: {ReviewId}", reviewId);
                var existingReview = await _context.Reviews.FindAsync(reviewId);
                
                if (existingReview == null || existingReview.PatientId != _userManager.GetUserId(User))
                {
                    return Json(new { success = false, message = "Review not found." });
                }

                if (string.IsNullOrEmpty(review.Comment) || review.Rating == 0)
                {
                    return Json(new { success = false, message = "Invalid review data." });
                }

                existingReview.Rating = review.Rating;
                existingReview.Comment = review.Comment;
                existingReview.DateCreated = DateTime.Now; 

                _context.Reviews.Update(existingReview);
                await _context.SaveChangesAsync();
                await UpdateAverageRating(existingReview.DoctorId);

                return Json(new { success = true, message = "Review updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating review ID: {ReviewId}", reviewId);
                return Json(new { success = false, message = "An error occurred while updating the review." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }



    }
}

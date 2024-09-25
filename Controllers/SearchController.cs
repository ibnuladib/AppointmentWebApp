using AppointmentWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentWebApp
{
    public class SearchController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SearchController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchString, string specialization, string filterBy)
        {
            var allUsers = await _userManager.Users
                .Include(u => u.Reviews) // Ensure Reviews are included
                .ToListAsync();
            var doctorUsers = allUsers.Where(u => _userManager.IsInRoleAsync(u, "Doctor").Result).ToList();

            // Search by searchString (ID, Name, Specialization, or Email)
            if (!string.IsNullOrEmpty(searchString))
            {
                doctorUsers = doctorUsers.Where(u => u.UserName.Contains(searchString) ||
                                                     u.Email.Contains(searchString) ||
                                                     u.FirstName.Contains(searchString) ||
                                                     u.LastName.Contains(searchString) ||
                                                     u.Specialization.Contains(searchString))
                                         .ToList();
            }

            // Filter by specialization
            if (!string.IsNullOrEmpty(specialization))
            {
                doctorUsers = doctorUsers.Where(u => u.Specialization == specialization).ToList();
            }

            // Apply filters (Rating or Experience) on the current search/specialization results
            if (!string.IsNullOrEmpty(filterBy))
            {
                if (filterBy == "rating")
                {
                    doctorUsers = doctorUsers.OrderByDescending(u => u.AverageRating).ToList();
                }
                else if (filterBy == "experience")
                {
                    doctorUsers = doctorUsers.OrderByDescending(u => u.YearsOfExperience).ToList();
                }
            }

            var specializationCounts = allUsers.GroupBy(u => u.Specialization)
                                                  .Select(g => new
                                                  {
                                                      Specialization = g.Key,
                                                      //Count = g.Count()
                                                  })
                                                  .ToList();

            ViewBag.SpecializationCounts = specializationCounts;
            ViewBag.SearchString = searchString;
            ViewBag.Specialization = specialization; // Keep the selected specialization
            ViewBag.FilterBy = filterBy; // Pass the current filter applied

            return View(doctorUsers);
        }


        [HttpGet]
        public JsonResult GetSuggestions(string query)
        {
            var suggestions = _userManager.Users
                .Where(u => u.FirstName.Contains(query) ||
                             u.LastName.Contains(query) ||
                             u.Specialization.Contains(query) ||
                             (u.FirstName + " " + u.LastName).Contains(query))
                .Select(u => new
                {
                    id = u.Id,
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    specialization = u.Specialization
                })
                .ToList();

            return Json(suggestions);
        }

    }
}

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

            var allUsers = await _userManager.Users.ToListAsync();

            var doctorUsers = new List<ApplicationUser>();
            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "Doctor"))
                {
                    doctorUsers.Add(user);
                }
            }

            var specializationCounts = doctorUsers
                .GroupBy(u => u.Specialization)
                .Select(g => new
                {
                    Specialization = g.Key,
                    Count = g.Count()
                })
                .ToList();

            // Store searchString in ViewBag
            ViewBag.SearchString = searchString;

            // Apply filters based on the parameters
            if (!string.IsNullOrEmpty(searchString))
            {
                doctorUsers = doctorUsers.Where(u => u.UserName.Contains(searchString) ||
                                                      u.Email.Contains(searchString) ||
                                                      u.FirstName.Contains(searchString) ||
                                                      u.LastName.Contains(searchString) ||
                                                      u.Specialization.Contains(searchString)).ToList();
            }

            if (!string.IsNullOrEmpty(specialization))
            {
                doctorUsers = doctorUsers.Where(u => u.Specialization == specialization).ToList();
            }

            if (filterBy == "rating")
            {
                doctorUsers = doctorUsers.OrderByDescending(u => u.AverageRating).ToList(); // Adjust to your rating property
            }
            else if (filterBy == "experience")
            {
                doctorUsers = doctorUsers.OrderByDescending(u => u.YearsOfExperience).ToList(); // Adjust to your experience property
            }

            ViewBag.SpecializationCounts = specializationCounts; // Pass the specialization counts to the view

            return View(doctorUsers);
        }



        public JsonResult GetSuggestions(string query)
        {
            var suggestions = _userManager.Users
                .Where(u => u.FirstName.Contains(query) ||
                             u.LastName.Contains(query) ||
                             u.Specialization.Contains(query))
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

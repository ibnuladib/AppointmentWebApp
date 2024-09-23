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

        public async Task<IActionResult> Index(string searchString)
        {
            var allUsers = await _userManager.Users.ToListAsync();

            // Filter only doctors
            var doctorUsers = new List<ApplicationUser>();
            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "Doctor"))
                {
                    doctorUsers.Add(user);
                }
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                doctorUsers = doctorUsers.Where(u => u.UserName.Contains(searchString) ||
                                          u.Email.Contains(searchString) ||
                                          u.FirstName.Contains(searchString) ||
                                          u.LastName.Contains(searchString) ||
                                          u.Specialization.Contains(searchString)).ToList();
            }

            return View(doctorUsers.ToList());
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

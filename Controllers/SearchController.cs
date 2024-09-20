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
            var users = await _userManager.Users.ToListAsync();
            var doctors = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Doctor"))
                {
                    doctors.Add(user);
                }
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                doctors = doctors.Where(d =>
                    d.UserName.ToLower().Contains(searchString) ||
                    d.Id.Contains(searchString) ||
                    d.Email.ToLower().Contains(searchString) // Adjust if you have specialty in a custom property
                ).ToList();
            }

            return View(doctors);
        }

        [HttpGet]
        public async Task<IActionResult> GetSuggestions(string query)
        {
            var users = await _userManager.Users
                .Where(u => u.UserName.Contains(query) || u.Email.Contains(query) /* Add more filters as needed */)
                .Select(u => new { id = u.Id, name = u.UserName }) // or any other property you want to display
                .ToListAsync();

            return Json(users);
        }

    }
}

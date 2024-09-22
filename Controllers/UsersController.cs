using AppointmentWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace AppointmentWebApp.Controllers
{

    //[Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new Dictionary<ApplicationUser, IList<string>>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user] = roles;
            }

            ViewBag.UserRoles = userRoles;
            return View(users);
        }

        // Action to display users with the "Doctor" role
        public async Task<IActionResult> Doctors()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Doctor");

            if (usersInRole == null || !usersInRole.Any())
            {
                Console.WriteLine("No users found in Doctor role");
            }
            else
            {
                foreach (var user in usersInRole)
                {
                    Console.WriteLine($"User found: {user.Email}");
                }
            }

            return View("Doctors", usersInRole);
        }


        public async Task<IActionResult> Patients()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Patient");

            if (usersInRole == null || !usersInRole.Any())
            {
                Console.WriteLine("No users found in Doctor role");
            }
            else
            {
                foreach (var user in usersInRole)
                {
                    Console.WriteLine($"User found: {user.Email}");
                }
            }

            return View("Patients", usersInRole);

        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

    }
}
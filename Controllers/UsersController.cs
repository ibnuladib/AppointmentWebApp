using AppointmentWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> UserDetails(string id, bool rate = false)
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

            ViewData["Rate"] = rate;
            return View(user);
        }
        // GET: Users/Delete/id
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required for deletion.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index)); 
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Index", _userManager.Users);
        }


        [HttpPost]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(u => u.FirstName.Contains(searchTerm) ||
                                          u.LastName.Contains(searchTerm) ||
                                          u.Email.Contains(searchTerm) ||
                                          u.PhoneNumber.Contains(searchTerm) ||
                                          (u.FirstName + " " + u.LastName).Contains(searchTerm) ||
                                          u.Id.Contains(searchTerm));
            }

            return View("Index", await users.ToListAsync());
        }
        // GET: Users/Update/id
        [HttpGet]
        public async Task<IActionResult> Update(string id)
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

        // POST: Users/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByIdAsync(user.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.Address = user.Address;
                existingUser.Gender = user.Gender;
                existingUser.DateOfBirth = user.DateOfBirth;

                

                var result = await _userManager.UpdateAsync(existingUser);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(user);
        }

    }
}
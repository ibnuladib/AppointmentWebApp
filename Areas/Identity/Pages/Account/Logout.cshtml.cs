#nullable disable

using System;
using System.Threading.Tasks;
using AppointmentWebApp.Models;
using AppointmentWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AppointmentWebApp.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly InMemoryAuditLog _inMemoryAuditLog;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger, UserManager<ApplicationUser> userManager, InMemoryAuditLog inMemoryAuditLog)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _inMemoryAuditLog = inMemoryAuditLog;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);
            await _inMemoryAuditLog.Log($"ID: {user.Id}, Email: {user?.UserName ?? "Unknown"} signed out.");
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}

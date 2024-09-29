﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using AppointmentWebApp.Models;
using AppointmentWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace AppointmentWebApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly InMemoryAuditLog _inMemoryAuditLog;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            InMemoryAuditLog inMemoryAuditLog)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _inMemoryAuditLog = inMemoryAuditLog;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [ProtectedPersonalData]
            [StringLength(11, ErrorMessage = "It needs to be 11 digits", MinimumLength = 11)]
            [Display(Name = "Phone Number")]
            public virtual string PhoneNumber { get; set; }

            [Display(Name = "Date of Birth")]
            public DateTime DateofBirth { get; set; }

            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Qualification")]
            //[Required(ErrorMessage = "Qualification is required.")]
            public string Qualification { get; set; }

            [Display(Name = "Specialization")]
           // [Required(ErrorMessage = "Specialization is required.")]
            public string Specialization { get; set; }

            [Display(Name = "Years of Experience")]
            //[Required(ErrorMessage = "Years of Experience is required.")]
            public int YearsOfExperience { get; set; }

            [Display(Name = "Consultation Fees (Per Hour)")]
            [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid amount")]
            public decimal ConsultationFeesPerHour { get; set; }

            [Display(Name = "Medical License Number")]
            //[Required(ErrorMessage = "Medical License Number is required.")]
            public string MedicalLicenseNumber { get; set; }

            [Display(Name = "Gender")]
            //[Required(ErrorMessage = "Gender is required")]
            public string Gender { get; set; } // Assume values will be "Male" or "Female"

            [Display(Name = "Medical History")]
            //[Required(ErrorMessage = "Medical History is required.")]
            public string MedicalHistory { get; set; }

            [Display(Name = "Blood Group")]
           // [Required(ErrorMessage = "Blood Group is required.")]
            public string BloodGroup { get; set; }

            [Display(Name = "Insurace Details")]
          //  [Required(ErrorMessage = "Insurance Details is required.")]
            public string InsuranceDetails { get; set; }

            [Display(Name = "Address")]
           // [Required(ErrorMessage = "Address is required")]
            public string Address { get; set; }

            [Display(Name = "Visiting Time Start Hour")]
          //  [Required(ErrorMessage = "Start Hour is required.")]
            public int VisitingTimeStartHour { get; set; }

            [Display(Name = "Visiting Time End Hour")]
           // [Required(ErrorMessage = "End Hour is required.")]
            public int VisitingTimeEndHour { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Landing", "Home"); // Redirect to Home if authenticated
            }

            ReturnUrl = returnUrl;

            // Fetch external authentication schemes
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return Page(); // Return the page if not authenticated
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var role = Request.Form["role"].ToString();
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    PhoneNumber = Input.PhoneNumber,
                    DateOfBirth = Input.DateofBirth,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Address = Input.Address,
                    Gender = Input.Gender
                };

                // Assign fields based on role
                if (role == "Doctor")
                {
                    user.Qualification = Input.Qualification;
                    user.Specialization = Input.Specialization;
                    user.YearsOfExperience = Input.YearsOfExperience;
                    user.ConsultationFeesPerHour = Input.ConsultationFeesPerHour;
                    user.MedicalLicenseNumber = Input.MedicalLicenseNumber;
                    //user.VisitingTimeStart = new DateTime(1, 1, 1, Input.VisitingTimeStartHour, 0, 0); // Hour only
                    //user.VisitingTimeEnd = new DateTime(1, 1, 1, Input.VisitingTimeEndHour, 0, 0); // Hour only
                }
                else if (role == "Patient")
                {
                    user.MedicalHistory = Input.MedicalHistory;
                    user.BloodGroup = Input.BloodGroup;
                    user.InsuranceDetails = Input.InsuranceDetails;
                }

                var result = await _userManager.CreateAsync(user, Input.Password);

                /*                if (result.Succeeded)
                                {
                                    await _userManager.AddToRoleAsync(user, role);
                                    _logger.LogInformation("User created a new account with password.");

                                    var userId = await _userManager.GetUserIdAsync(user);
                                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                                    var callbackUrl = Url.Page(
                                        "/Account/ConfirmEmail",
                                        pageHandler: null,
                                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                                        protocol: Request.Scheme);

                                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                                    {
                                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                                    }
                                    else
                                    {
                                        await _signInManager.SignInAsync(user, isPersistent: false);
                                        return LocalRedirect(returnUrl);
                                    }
                                }*/
                if (result.Succeeded)
                {
                    // Mark the email as confirmed
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);

                    await _userManager.AddToRoleAsync(user, role);
                    _logger.LogInformation("User created a new account with password.");
                    await _inMemoryAuditLog.Log($" ID: {user.Id}, Email: {user.Email} has been created.");

                    // Automatically sign in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    _logger.LogInformation("User cannot be created");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                _logger.LogInformation("Model Invalid");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}

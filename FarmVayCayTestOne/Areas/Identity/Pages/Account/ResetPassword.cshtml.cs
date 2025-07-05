// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Farm.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FarmVayCayTestOne.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public ResetPasswordModel(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public IActionResult OnGet(string email )
        {
            if(string.IsNullOrEmpty(email))
            {
                TempData["error"] = "Invalid request.";
                return RedirectToPage("./ForgotPassword");
            }

            Input = new()
            {
                Email = email
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var userCodeAndExp = _unitOfWork.ApplicationUserRepository.Get(u => u.Email == Input.Email);

            var Appuser = await _userManager.FindByEmailAsync(Input.Email);
            if (Appuser == null)
            {
                
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            if (string.IsNullOrEmpty(Input.Password) || string.IsNullOrEmpty(Input.ConfirmPassword))
            {
                ModelState.AddModelError("", "All fields are required.");
                return Page();
            }

            if (Input.Password != Input.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return Page();
            }

            
            var newPasswordHash = _userManager.PasswordHasher.HashPassword(Appuser, Input.Password);
            
            Appuser.PasswordHash = newPasswordHash;

            userCodeAndExp.PasswordRestCode = null;
            userCodeAndExp.PasswordRestExpiry = null;

            var result = await _userManager.UpdateAsync(Appuser);

            if (result.Succeeded)
            {
                TempData["success"] = "Your password has been successfully reset!";
                return RedirectToPage("./ResetPasswordConfirmation");
            }
            else
            {
                ModelState.AddModelError("", "An error occurred while resetting the password.");
                return Page();
            }
        }

    }
}

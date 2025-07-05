// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Farm.DataAccess.Repository.IRepository;
using FarmVayCayTestOne.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmVayCayTestOne.Areas.Identity.Pages.Account
{
    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// 
        private readonly IUnitOfWork _unitOfWork;
        public ForgotPasswordConfirmation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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
            public string CodeFromEmail { get; set; }
            public string Email {  get; set; }
        }
        public IActionResult OnGet(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                
                return RedirectToPage("./ForgotPassword");
            }

            Input = new()
            {
                Email = email
            }; 
            return Page();
        }

        public IActionResult OnPost()
        
        {
            if (string.IsNullOrEmpty(Input.Email))
            {
                
                return RedirectToPage("./ForgotPassword");
            }
            var email = Input.Email.Trim().ToLower();
            ApplicationUser user = _unitOfWork.ApplicationUserRepository.Get(u => u.Email == Input.Email);

            if (user == null || string.IsNullOrEmpty(user.PasswordRestCode))
            {
                ModelState.AddModelError("Input.Email", "Invalid email or expired code."); 
                return Page();
            }

            if (Input.CodeFromEmail == user.PasswordRestCode)
            {
                
                return RedirectToPage("./ResetPassword", new { email = Input.Email });
            }
            else
            {
                ModelState.AddModelError("Input.CodeFromEmail", "Invalid Code! Please try again.");
                
                return Page();
            }
        }
        public IActionResult ResendCode()
        {
            return Page();
        }

    }
}

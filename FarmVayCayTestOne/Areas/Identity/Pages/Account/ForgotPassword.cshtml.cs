// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Farm.DataAccess.Repository;
using Farm.DataAccess.Repository.IRepository;
using FarmVayCayTestOne.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json.Serialization;

namespace FarmVayCayTestOne.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailSender;

        public ForgotPasswordModel(IUnitOfWork unitOfWork, EmailService emailSender)
        {
           _unitOfWork = unitOfWork;
            _emailSender = emailSender;
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
           
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = _unitOfWork.ApplicationUserRepository.Get(u => u.Email == Input.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Input.Email", "This email address was not found.");
                    return Page();
                }

                var verificationCode = new Random().Next(1000, 9999).ToString();
                user.PasswordRestCode = verificationCode;
                user.PasswordRestExpiry = DateTime.UtcNow.AddMinutes(5);
                _unitOfWork.ApplicationUserRepository.Update(user);
                _unitOfWork.Save();

                var success = await _emailSender.SendEmailAsync(
                    Input.Email,
                    "🔐 Password Reset Code - FarmVayCay",
                    $@"
            <div style='font-family:Arial, sans-serif; color:#333;'>
                <h2 style='color:#3dd5bf;'>Password Reset Request</h2>
                <p>Hello,</p>
                <p>We received a request to reset your password. Please use the code below to complete the process:</p>

                <div style='font-size:24px; color:#3dd5bf; font-weight:bold; margin:20px 0;'>{verificationCode}</div>

                <p>If you did not request a password reset, please ignore this email. Your account is safe and no changes have been made.</p>

                <p style='margin-top:30px;'>Thank you for using <strong>FarmVayCay</strong>!</p>
                <p>
                    Best regards,<br/>
                    FarmVayCay Support Team<br/>
                    <a href='mailto:farmVayCay@gmail.com'>farmVayCay@gmail.com</a>
                </p>
            </div>"
                );

                if (!success)
                {
                    TempData["error"] = "Failed to send the email. Please try again later.";
                    
                }
                else
                {
                    //  TempData["success"] = "A reset code has been sent to your email.";
                    return RedirectToPage("./ForgotPasswordConfirmation", new { email = Input.Email });

                }


            }

            return Page();
        }

    }
}

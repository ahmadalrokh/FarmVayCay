using System.Security.Claims;
using Farm.DataAccess.Repository.IRepository;
using Farm.Models.VM;
using FarmVayCayTestOne.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FarmVayCayTestOne.Areas.Custmor.Controllers
{
    [Area("Custmor")]
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        //Dependancy Ingiction
        public ProfileController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // Display the profile page with user data
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var clams = (ClaimsIdentity)User.Identity;
            string userId = clams.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var model = new ProfileViewModel
            {
                User = applicationUser,
            };
            applicationUser.FirstName = applicationUser.FirstName;
            applicationUser.LastName = applicationUser.LastName;
            applicationUser.ImageUrl = applicationUser.ImageUrl;
            applicationUser.PhoneNumber = user.PhoneNumber;
            applicationUser.City = applicationUser.City;
            applicationUser.StreetAddress = applicationUser.StreetAddress;
            return View(model);
        }
        // Update User Image Handlar
        public IActionResult UpdateImage(ProfileViewModel model, IFormFile? file)
        {
            var clams = (ClaimsIdentity)User.Identity;
            string userId = clams.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == userId);
            model.User = applicationUser;

            string wwwrootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                string userPath = @"UserImage\User-" + applicationUser.Email;
                string finalPath = Path.Combine(wwwrootPath, userPath);

                if (!Directory.Exists(finalPath))
                {
                    Directory.CreateDirectory(finalPath);
                }
                // if Update
                if (!string.IsNullOrEmpty(model.User.ImageUrl))
                {
                    //Delete Old File
                    string oldImagePath = Path.Combine(wwwrootPath, applicationUser.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                // if Insert
                using (FileStream fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                model.User.ImageUrl = @"\UserImage\User-"+applicationUser.Email+@"\"+ fileName;
            }
            _unitOfWork.ApplicationUserRepository.Update(model.User);
            _unitOfWork.Save();
            TempData["success"] = "image updated successfully!";
            return RedirectToAction(nameof(Index));
        }    
        // Update profile information
        [HttpPost]
        public IActionResult UpdateProfile(ProfileViewModel model)
        {
            var clams = (ClaimsIdentity)User.Identity;
            string userId = clams.FindFirst(ClaimTypes.NameIdentifier).Value;
           
            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == userId);
            if (applicationUser == null)
            {
                return NotFound("User not found.");
            }

            applicationUser.FirstName = model.User.FirstName;
            applicationUser.LastName = model.User.LastName;
            
            applicationUser.PhoneNumber = model.User.PhoneNumber;

            applicationUser.City = model.User.City;
            applicationUser.StreetAddress = model.User.StreetAddress;
      
            _unitOfWork.ApplicationUserRepository.Update(applicationUser);
            _unitOfWork.Save();            
           
            TempData["success"] = "Profile updated successfully!";
            
            return RedirectToAction("Index");
        }
        // Change password
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ProfileViewModel model)
        {
            var clams = (ClaimsIdentity)User.Identity;
            string userId = clams.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == userId);
            model.User = applicationUser;

            if (string.IsNullOrEmpty(model.CurrentPassword) ||
                string.IsNullOrEmpty(model.NewPassword) ||
                string.IsNullOrEmpty(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "All fields are required.");
                return View("Index", model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View("Index", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Index", model);
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["success"] = "Password changed successfully!";
            return RedirectToAction("Index");
        }
    }
    
}

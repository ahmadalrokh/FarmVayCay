using Farm.DataAccess.Repository.IRepository;
using Farm.Models.Models;
using Farm.Models.VM;
using Farm.Utility;
using FarmVayCayTestOne.Models;
using FarmVayCayTestOne.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FarmVayCayTestOne.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RequestsFarmsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly EmailService _emailSender;
        public RequestsFarmsController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment,EmailService emailSender)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }
        public IActionResult Index(string? status)
        {
            //All
            List<RequestFarms> RequestFarms = _unitOfWork.RequestFarmsRepository.GetAll(includePropertes: "Farm").ToList();
            //Approved Farms
            if (status == "Approved")
                RequestFarms = _unitOfWork.RequestFarmsRepository.GetAll(u => u.Farm.ApprovalStatus == "Approved", includePropertes: "Farm").ToList();
            //Pending Farms
            if (status == "Pending")
                RequestFarms = _unitOfWork.RequestFarmsRepository.GetAll(u => u.Farm.ApprovalStatus == "Pending", includePropertes: "Farm").ToList();
            //Approved Farms
            if (status == "Rejected")
                RequestFarms = _unitOfWork.RequestFarmsRepository.GetAll(u => u.Farm.ApprovalStatus == "Rejected", includePropertes: "Farm").ToList();
           
            return View(RequestFarms);
        }
        public IActionResult DeleteRequestFarms(int farmId)
        {

            var farmFrmoDb = _unitOfWork.RequestFarmsRepository.Get(f=>f.FarmId == farmId);
            _unitOfWork.RequestFarmsRepository.Remove(farmFrmoDb);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        public IActionResult ViewFarmDetales(int farmId)
        {
            Farms farmFromDb = _unitOfWork.FarmsRepository.Get(f => f.Id == farmId , includePropertes: "User");
            //Get the Owner
            ApplicationUser Owner = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == farmFromDb.UserId);
            farmFromDb.User = Owner;

            FarmVM farmVM = new()
            {
                Farms = farmFromDb,
                farmImages=_unitOfWork.FarmImagesRepository.GetAll(f=>f.FarmId== farmId).ToList()
            };
            return View(farmVM);
        }
        public IActionResult RejectFarm(int farmId) 
        {
            Farms farmFormDb = _unitOfWork.FarmsRepository.Get(f => f.Id == farmId);
            if (farmFormDb == null)
            {
                return NotFound();
            }
            farmFormDb.IsApproved = false;
            farmFormDb.ApprovalStatus=SD.Status_Rejected;
            _unitOfWork.FarmsRepository.Update(farmFormDb);
            _unitOfWork.Save();
            TempData["success"] = "the farm rejected !!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AcceptFarm(int farmId)
        {
            var farm = _unitOfWork.FarmsRepository.Get(f => f.Id == farmId);
            if (farm == null)
            {
                return NotFound();
            }
            farm.IsApproved = true;
            farm.ApprovalStatus=SD.Status_Approved;
            farm.IsAvailable = SD.Status_Available;
            var AppUser = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == farm.UserId);
            if (AppUser == null)
            {
                return NotFound();
            }
            _unitOfWork.FarmsRepository.Update(farm);
            _unitOfWork.Save();

            AppUser.IsFarmOwner = true;
            _unitOfWork.ApplicationUserRepository.Update(AppUser);
            _unitOfWork.Save();

            var user = await _userManager.FindByIdAsync(farm.UserId);
            if (user == null)
            {
                return NotFound();
            }
           
            if (await _userManager.IsInRoleAsync(user, SD.Role_Custmor))
            {
               
                await _userManager.RemoveFromRoleAsync(user, SD.Role_Custmor);

                
                await _userManager.AddToRoleAsync(user, SD.Role_FarmOwner);
            }


            //send Email
            try
            {
                await _emailSender.SendEmailAsync(
                AppUser.Email,
                $"Your Farm '{farm.FarmName}' Has Been Approved 🎉",
                $@"
                <div style='font-family:Arial,sans-serif; color:#333'>
                    <h2 style='color:#3dd5bf;'>Your Farm is Now Live!</h2>
                    <p>Dear {AppUser.FirstName},</p>

                    <p>We are pleased to inform you that your farm <strong>{farm.FarmName}</strong> has been reviewed and approved by our team.</p>

                    <p>It is now visible to all users on <strong>FarmVayCay</strong>, and customers can start viewing and booking your farm.</p>

                    <p>Make sure to keep your calendar updated and respond to bookings promptly to maintain a great hosting experience.</p>

                    <p>Thank you for choosing <strong>FarmVayCay</strong>!</p>

                    <p style='margin-top:30px;'>Best regards,<br/>
                    FarmVayCay Team<br/>
                    <a href='mailto:farmVayCay@gmail.com'>farmVayCay@gmail.com</a>
                    </p>
                </div>"
                );

            }
            catch
            {
                TempData["error"] = "Reservation confirmed, but failed to send confirmation email.";
            }
            TempData["success"] = "the farm accepted !!";
            return RedirectToAction("Index");
            
        }
        
    }
}

using Farm.DataAccess.Repository;
using Farm.DataAccess.Repository.IRepository;
using FarmVayCayTestOne.Data;
using FarmVayCayTestOne.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace FarmVayCayTestOne.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        public UsersController(ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string? Role)
        {

            List<ApplicationUser> users = new List<ApplicationUser>();

            users=_db.applicationUsers.ToList();

            var userRole = _db.UserRoles.ToList();

            var roles = _db.Roles.ToList();
            
            foreach (var user in users)
            {
                
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;

                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }
            //Fillter Section
            if (Role== "Custmor")
            {
                users = users.Where(u => u.Role == "Custmor").ToList();
            }
            if (Role == "FarmOwner")
            {
                users = users.Where(u => u.Role == "FarmOwner").ToList();
            }
            if (Role == "Admin")
            {
                users = users.Where(u => u.Role == "Admin").ToList();
            }
            return View(users);
        }

        public IActionResult ViewUserDetales(string userId)
        {
            ApplicationUser? user = _db.applicationUsers.FirstOrDefault(u => u.Id == userId);
            var userRole = _db.UserRoles.ToList();

            var roles = _db.Roles.ToList();

            var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
            user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            
            if (user == null)
                return NotFound();
            else
            {
                return View(user);
            }
        }
        public IActionResult DeleteUser(string userId)
        {
            var userFormDb = _db.applicationUsers.FirstOrDefault(u => u.Id == userId);
            if (userFormDb == null)
                return NotFound();
            var farmsList = _db.farms.Where(f => f.UserId == userId).ToList();
            if(farmsList!=null)
            {
                foreach(var farms in farmsList) 
                {
                    try
                    {
                        var images = _unitOfWork.FarmImagesRepository.GetAll(f => f.FarmId == farms.Id);
                        _unitOfWork.FarmImagesRepository.RemoveRange(images);

                        var favarate = _unitOfWork.FavoriteRepository.GetAll(f => f.FarmId == farms.Id);
                        if (favarate != null)
                            _unitOfWork.FavoriteRepository.RemoveRange(favarate);

                        var booking = _unitOfWork.BokingRepository.GetAll(f => f.FarmId == farms.Id);
                        if (booking != null)
                            _unitOfWork.BokingRepository.RemoveRange(booking);

                        var rating = _unitOfWork.RatingRepository.GetAll(f => f.FarmId == farms.Id);
                        if (rating != null)
                            _unitOfWork.RatingRepository.RemoveRange(rating);

                        var request = _unitOfWork.RequestFarmsRepository.Get(f => f.FarmId == farms.Id);
                        if (request != null)
                            _unitOfWork.RequestFarmsRepository.Remove(request);

                        var dates = _unitOfWork.UnavailableDatesRepository.GetAll(f => f.FarmId == farms.Id);
                        if (dates != null)
                            _unitOfWork.UnavailableDatesRepository.RemoveRange(dates);

                        _unitOfWork.FarmsRepository.Remove(farms);
                        _unitOfWork.Save();

                        TempData["success"] = $"Farm {farms.FarmName} Deleted!!";
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        TempData["error"] = $"Somthig Wrong Please Try Again !!";
                        return RedirectToAction("Index");
                    }
                }
               
            }
            _db.applicationUsers.Remove(userFormDb);
            _db.SaveChanges();
            TempData["success"] = $"User {userFormDb.Email} Deleted !!";
            return RedirectToAction("Index");
        }
    }
}

using System.Security.Claims;
using Farm.DataAccess.Repository.IRepository;
using Farm.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmVayCayTestOne.Areas.Custmor.Controllers
{
    [Area("Custmor")]
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        // Dependancy Ingection
        public FavoriteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // Retern Farm Page
        public IActionResult Index()
        {
            // Get The User id
            var  clamsIdentity = (ClaimsIdentity)User.Identity;
            string userId = clamsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            List<Favorite> favoritesList = _unitOfWork.FavoriteRepository.GetAll(u=>u.UserId==userId,
                includePropertes: "Farm,Farm.FarmImages").ToList();
            
            return View(favoritesList);
        }
        //Remove Farm From Favarate
        public IActionResult Remove(int farmId) 
        {
            var clamsIdentity = (ClaimsIdentity)User.Identity;
            string userId = clamsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            Favorite favoriteFromDb = _unitOfWork.FavoriteRepository.Get(f => f.FarmId == farmId && f.UserId == userId, includePropertes: "Farm");
           
            _unitOfWork.FavoriteRepository.Remove(favoriteFromDb);
            _unitOfWork.Save();
            
            return RedirectToAction("Index");
        }
    }
}

using Farm.DataAccess.Repository.IRepository;
using Farm.Models.Models;
using Farm.Models.VM;
using Farm.Utility;
using FarmVayCayTestOne.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FarmVayCayTestOne.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AvailableFarmsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        // Depandance Ingection
        public AvailableFarmsController(IUnitOfWork unitOfWork,ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        // To Return Farms From DataBase
        public IActionResult Index(string? status)
        {
            List<Farms> farms = _unitOfWork.FarmsRepository.GetAll(f=>f.IsApproved== true , includePropertes: "User").ToList();
            if(status== "UnAvalabel")
            {
                farms = _unitOfWork.FarmsRepository.GetAll(f => f.IsAvailable==SD.Status_UnAvailable, includePropertes: "User").ToList();
            }
            if (status == "Avalabel")
            {
                farms = _unitOfWork.FarmsRepository.GetAll(f => f.IsAvailable == SD.Status_Available, includePropertes: "User").ToList();
            }
            return View(farms);
        }
        // To Delete Farm from database
        public IActionResult DeleteFarm(int farmId)
        {
            var farmFromDb=_unitOfWork.FarmsRepository.Get(f=>f.Id==farmId);
            if(farmFromDb == null) 
                return NotFound();
            else
            {
                try
                {
                    var images = _unitOfWork.FarmImagesRepository.GetAll(f => f.FarmId == farmId);
                    _unitOfWork.FarmImagesRepository.RemoveRange(images);

                    var favarate = _unitOfWork.FavoriteRepository.GetAll(f => f.FarmId == farmId);
                    if (favarate != null)
                        _unitOfWork.FavoriteRepository.RemoveRange(favarate);

                    var booking = _unitOfWork.BokingRepository.GetAll(f => f.FarmId == farmId);
                    if (booking != null)
                        _unitOfWork.BokingRepository.RemoveRange(booking);

                    var rating = _unitOfWork.RatingRepository.GetAll(f => f.FarmId == farmId);
                    if (rating != null)
                        _unitOfWork.RatingRepository.RemoveRange(rating);

                    var request = _unitOfWork.RequestFarmsRepository.Get(f => f.FarmId == farmId);
                    if (request != null)
                        _unitOfWork.RequestFarmsRepository.Remove(request);

                    var dates = _unitOfWork.UnavailableDatesRepository.GetAll(f => f.FarmId == farmId);
                    if (dates != null)
                        _unitOfWork.UnavailableDatesRepository.RemoveRange(dates);

                    _unitOfWork.FarmsRepository.Remove(farmFromDb);
                    _unitOfWork.Save();

                    // To Delete farm image file 
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string farmPath = @"FarmsImage\FarmId-" + farmId;
                    string finalPath = Path.Combine(wwwRootPath, farmPath);
                    if(Directory.Exists(finalPath))
                    {
                        Directory.Delete(finalPath, true);
                    }

                    TempData["success"] = $"Farm {farmFromDb.FarmName} Deleted!!";
                    return RedirectToAction("Index");
                }
                catch(Exception e)
                {
                    TempData["error"] = $"Somthig Wrong Please Try Again !!";
                    return RedirectToAction("Index");
                }
                

            } 
            
        }
        // Farm deltales
        public IActionResult ViewFarmsDetales(int farmId)
        {
            var farmFromDb = _unitOfWork.FarmsRepository.Get(f => f.Id == farmId);
            var user = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == farmFromDb.UserId);
            farmFromDb.User = user;
            FarmVM farmVM = new()
            {
                Farms = farmFromDb,
                farmImages = _unitOfWork.FarmImagesRepository.GetAll(f => f.FarmId == farmId).ToList()
            };
            
            return View(farmVM);
        }
        //Make farm Unavalabel
        public IActionResult UnAvailable(int farmId) 
        {
            var farmFromDb=_unitOfWork.FarmsRepository.Get(f=>f.Id == farmId);
            if (farmFromDb == null)
                return NotFound();
            farmFromDb.IsApproved = true;
            farmFromDb.IsAvailable = "UnAvailable";
            _unitOfWork.FarmsRepository.Update(farmFromDb);
            _unitOfWork.Save();
            
            TempData["success"] = $"Farm {farmFromDb.FarmName} UnAvailable!!";
            return RedirectToAction("Index");
        }
        //Make farm avalabel
        public IActionResult Available(int farmId)
        {
            var farmFromDb = _unitOfWork.FarmsRepository.Get(f => f.Id == farmId);
            if (farmFromDb == null)
                return NotFound();
            farmFromDb.IsApproved = true;
            farmFromDb.IsAvailable = SD.Status_Available;
            _unitOfWork.FarmsRepository.Update(farmFromDb);
            _unitOfWork.Save();
            TempData["success"] = $"Farm {farmFromDb.FarmName} Available!!";
            return RedirectToAction("Index");
        }
    }
}

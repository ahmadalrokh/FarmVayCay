using System.Security.Claims;
using Farm.DataAccess.Repository.IRepository;
using Farm.Models.Models;
using Farm.Models.VM;
using Farm.Utility;
using FarmVayCayTestOne.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace FarmVayCayTestOne.Areas.FarmOwner.Controllers
{
    [Area("FarmOwner")]
    public class Ownerfeatures : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //Depandancy Injuction
        public Ownerfeatures(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;   
        }
        [HttpGet]
        // Get Maintenance 
        public IActionResult Maintenance(string? mager)
        {
            var list=_unitOfWork.MaintenanceRepository.GetAll().ToList();
            if(mager!=null)
            {
                if(mager== "gar")
                {
                    list=_unitOfWork.MaintenanceRepository.GetAll(m=>m.Magor== "Garden Design").ToList();
                }
                if (mager == "iron")
                {
                    list = _unitOfWork.MaintenanceRepository.GetAll(m => m.Magor == "Blacksmeth").ToList();
                }
                if (mager == "water")
                {
                    list = _unitOfWork.MaintenanceRepository.GetAll(m => m.Magor == "").ToList();
                }
                if (mager == "Elec")
                {
                    list = _unitOfWork.MaintenanceRepository.GetAll(m => m.Magor == "Electrician").ToList();
                }
            }

            return View(list);
        }
        [HttpGet]
        // Get My Farms Page 
        public IActionResult MyFarms()
        {
            var claims = (ClaimsIdentity)User.Identity;
            string userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var Onwer =  _unitOfWork.ApplicationUserRepository.Get(o => o.Id == userID);

            if (User.IsInRole(SD.Role_FarmOwner) || User.IsInRole(SD.Role_Admin))
            {
                List<Farms> farms = _unitOfWork.FarmsRepository.GetAll(f=>f.UserId==userID,includePropertes: "FarmImages").ToList();
                
                return View(farms);
            }
            return NotFound();
        }
        [HttpGet]
        // Edit Farm Get
        public IActionResult EditFarm(int farmId)
        {
            var farm = _unitOfWork.FarmsRepository.Get(f => f.Id == farmId);
            if (farm == null)
            {
                return RedirectToAction("MyFarms");
            }
            var farmImage = _unitOfWork.FarmImagesRepository.GetAll(f => f.FarmId == farmId);
            FarmVM model = new()
            {
                Farms = farm,
                farmImages = farmImage.ToList(),
                boking = null
            };
            return View(model);
                
        }
        [HttpPost]
        // Edit Farm Post
        public IActionResult EdimFarm(FarmVM model)
        {
            if (model.Farms.Id != 0)
            {
                var farmFromDb=_unitOfWork.FarmsRepository.Get(f=>f.Id == model.Farms.Id);

                if (farmFromDb == null)
                {
                    return NotFound();
                }

                farmFromDb.FarmName=model.Farms.FarmName;
                farmFromDb.PricePerDay = model.Farms.PricePerDay;
                farmFromDb.Governorates = model.Farms.Governorates;
                farmFromDb.LocationUrl = model.Farms.LocationUrl;
                farmFromDb.Room = model.Farms.Room;
                farmFromDb.PathRoom = model.Farms.PathRoom;
                farmFromDb.Flors = model.Farms.Flors;
                farmFromDb.Guests = model.Farms.Guests;
                farmFromDb.SwimingPool = model.Farms.SwimingPool;
                farmFromDb.BarbecueArea = model.Farms.BarbecueArea;
                farmFromDb.ChildrenArea = model.Farms.ChildrenArea;
                farmFromDb.FootballField = model.Farms.FootballField;
                farmFromDb.BingPongTabel = model.Farms.BingPongTabel;
                farmFromDb.BilliardoTabel = model.Farms.BilliardoTabel;
                farmFromDb.AirCondtioner = model.Farms.AirCondtioner;
                farmFromDb.WIFI = model.Farms.WIFI;
                farmFromDb.Description = model.Farms.Description;

                _unitOfWork.FarmsRepository.Update(farmFromDb);
                _unitOfWork.Save();

                TempData["success"] = $"Farm {farmFromDb.FarmName} Updation Successfully!";

                return RedirectToAction("MyFarms");
            }
            TempData["error"] = $"Somthing Wrong Pleas Try Again Later!";
            return RedirectToAction("MyFarms");
        }
        // Delet Farm Image Post
        public IActionResult DeleteImage(int imageId)
        {
            FarmImages images=_unitOfWork.FarmImagesRepository.Get(i=>i.Id == imageId);
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            if (images != null)
            {
               // string farmPath = @"FarmsImage\FarmId-" + images.FarmId;
                
                string oldImagePath = Path.Combine(wwwRootPath, images.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                _unitOfWork.FarmImagesRepository.Remove(images);
                _unitOfWork.Save();
                return RedirectToAction("EditFarm", new { farmId = images.FarmId });
            }
            TempData["error"] = "Somthing Wrong Plese Try Again !!!";
            return RedirectToAction("EditFarm", new { farmId = images.FarmId });
        }
        [HttpGet]
        // Update Image Get
        public IActionResult UpdateImage(int imageId)
        {
            FarmImages model = _unitOfWork.FarmImagesRepository.Get(i => i.Id == imageId);
            if (model == null)
            {
                TempData["error"] = "Somthing Wrong please Try agin !";
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        // Update Image Post
        public IActionResult UpdateImage(FarmImages model , IFormFile? file)
        {
            FarmImages image = _unitOfWork.FarmImagesRepository.Get(i => i.Id ==model.Id);

            string wwwrootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                string farmPath = @"FarmsImage\FarmId-" + image.FarmId;
                string finalPath = Path.Combine(wwwrootPath, farmPath);

                if (!Directory.Exists(finalPath))
                {
                    Directory.CreateDirectory(finalPath);
                }
                
                //Delete Old File
                string oldImagePath = Path.Combine(wwwrootPath, image.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
   
                //  Insert
                using (FileStream fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                image.ImageUrl = $"\\{farmPath}\\{fileName}";
              
                _unitOfWork.FarmImagesRepository.Update(image);
                _unitOfWork.Save();
                TempData["success"] = "image updated successfully!";
                return RedirectToAction("EditFarm", new { farmId = image.FarmId });
            }
            TempData["error"] = "You shoud shose image first";
            return RedirectToAction("EditFarm", new { farmId = image.FarmId });

        }
        [HttpGet]
        // Adding Nuw Image Get
        public IActionResult AddImage(int farmId)
        {
            FarmImages model = new FarmImages();
            model.FarmId = farmId;
            return View(model);
        }
        [HttpPost]
        // Adding Nuw Image Post
        [HttpPost]
        public IActionResult AddImage(List<IFormFile> images, int farmId)
        {
            string wwwrootPath = _webHostEnvironment.WebRootPath;

            if (images != null && images.Count > 0 && farmId != 0)
            {
                string farmPath = Path.Combine("FarmsImage", "FarmId-" + farmId);
                string finalPath = Path.Combine(wwwrootPath, farmPath);

                if (!Directory.Exists(finalPath))
                {
                    Directory.CreateDirectory(finalPath);
                }

                foreach (var image in images)
                {
                    if (image != null && image.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        string fullFilePath = Path.Combine(finalPath, fileName);

                        using (FileStream fileStream = new FileStream(fullFilePath, FileMode.Create))
                        {
                            image.CopyTo(fileStream);
                        }

                        var farmImage = new FarmImages()
                        {
                            ImageUrl = "\\"+Path.Combine(farmPath, fileName),
                            FarmId = farmId
                        };

                        _unitOfWork.FarmImagesRepository.Add(farmImage);
                    }
                }

                _unitOfWork.Save();
                TempData["success"] = "Images added successfully!";
                return RedirectToAction("EditFarm", new { farmId = farmId });
            }

            TempData["error"] = "You must choose at least one image.";
            return RedirectToAction("EditFarm", new { farmId = farmId });
        }

        [HttpGet]
        // Disable Farm For A day Get
        public IActionResult Disable(int farmId)
        {
            if (farmId == 0)
            {
                TempData["error"] = "Somthig wrong please try Again!";
                return RedirectToAction("MyFarms");
            }
            FarmUnavailableDate model= new FarmUnavailableDate();
            model.FarmId = farmId;
            return View(model);
        }
        [HttpPost]
        // Disable Farm For A day Post
        public IActionResult Disable(FarmUnavailableDate model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.UnavailableDatesRepository.Add(model);
                _unitOfWork.Save();
                return RedirectToAction("MyFarms");
            }
            ModelState.AddModelError("", "Somthing Wrong try Again!!");
            return View(model);
        }
        // Delate Farm 
        public IActionResult Delete(int farmId)
        {
            var farmFromDb = _unitOfWork.FarmsRepository.Get(f => f.Id == farmId);
            if (farmFromDb == null)
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
                    if(dates != null)
                        _unitOfWork.UnavailableDatesRepository.RemoveRange(dates);

                    _unitOfWork.FarmsRepository.Remove(farmFromDb);
                    _unitOfWork.Save();

                    TempData["success"] = $"Farm {farmFromDb.FarmName} Deleted!!";
                    return RedirectToAction("MyFarms");
                }
                catch (Exception e)
                {
                    TempData["error"] = $"Somthig Wrong Please Try Again !!";
                    return RedirectToAction("MyFarms");
                }

            }
            
        }
        public IActionResult BookingHistory()
        {
            var calims=(ClaimsIdentity)User.Identity;
            string userId = calims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var bookingList=_unitOfWork.BokingRepository.GetAll(b=>b.Farm.UserId==userId,includePropertes: "Custmor,Farm").ToList();
            return View(bookingList);
        }
    }
}

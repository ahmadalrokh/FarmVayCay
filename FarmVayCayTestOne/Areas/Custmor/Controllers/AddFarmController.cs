using System.Security.Claims;
using Farm.DataAccess.Repository.IRepository;
using Farm.Models.Models;
using Farm.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmVayCayTestOne.Areas.Custmor.Controllers
{
    [Area("Custmor")]
    public class AddFarmController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        // Depandancy Injection
        public AddFarmController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        // To Return Add Farm page 
        public IActionResult Index()
        {
            //Return New Farm To Add
            Farms farm = new Farms();
            return View(farm);
        }
        // Adding New Farm Post
        [HttpPost]
        public  IActionResult Index(Farms farm , List<IFormFile> files )
        {
            //Farm To Add
            var clamsIdentity = (ClaimsIdentity)User.Identity;
            string userId = clamsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            farm.UserId = userId;
            farm.Status = "";

            if (ModelState.IsValid)
            {
                //Add Farm to request tabel
                RequestFarms request = new()
                {
                    FarmId = farm.Id,
                    OwnerId = userId,
                    Farm = farm,
                    IsApproved = false,
                    RequestDate = DateTime.Now
                };
                _unitOfWork.RequestFarmsRepository.Add(request);
                _unitOfWork.Save();

                //Handel the file from the model
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string farmPath = @"FarmsImage\FarmId-" + farm.Id;
                        string finalPath = Path.Combine(wwwRootPath, farmPath);

                        if (!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }

                        using (FileStream fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        FarmImages farmImages = new()
                        {
                            ImageUrl = @"\" + farmPath + @"\" + fileName,
                            FarmId = farm.Id
                        };

                        if (farm.FarmImages == null)
                        {
                            farm.FarmImages = new List<FarmImages>();
                        }

                        farm.FarmImages.Add(farmImages);
                        _unitOfWork.FarmImagesRepository.Add(farmImages);
                        _unitOfWork.Save();
                    }
                    _unitOfWork.FarmsRepository.Update(farm);
                    _unitOfWork.Save();
                }
                TempData["success"] = "Farm Added Requset Succsessfuly The Admin will reply soon ";
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = "somthing Wrong Please Try Again!";
            return RedirectToAction("Index");
        }   
    }

}

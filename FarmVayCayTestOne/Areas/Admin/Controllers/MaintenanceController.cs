using Farm.DataAccess.Repository.IRepository;
using Farm.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace FarmVayCayTestOne.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MaintenanceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MaintenanceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var maintenanceList = _unitOfWork.MaintenanceRepository.GetAll().ToList();
            return View(maintenanceList);
        }
        public IActionResult Upsert(int id)
        {
            var maintenanceList = _unitOfWork.MaintenanceRepository.Get(t => t.Id == id);

            if (id == 0)
            {
                //Add
                return View(new Maintenance());
            }
            else
            {
                return View(maintenanceList);
                //Update

            }
        }
        [HttpPost]
        public IActionResult Upsert(Maintenance model)
        {
            var maintenanceFromDb = _unitOfWork.MaintenanceRepository.Get(t => t.Id == model.Id);
            if (!ModelState.IsValid)
            {
                TempData["error"] = "InValid";
                return View(maintenanceFromDb);
            }
            if (maintenanceFromDb == null)
            {
                //Add
                maintenanceFromDb = model;
                _unitOfWork.MaintenanceRepository.Add(maintenanceFromDb);
                _unitOfWork.Save();
                TempData["success"] = $"Maintenance {maintenanceFromDb.Name} Added ";
                return RedirectToAction("Index");
            }
            else
            {
                //Update

                maintenanceFromDb.Name = model.Name;
                maintenanceFromDb.PhoneNumber = model.PhoneNumber;
                maintenanceFromDb.Location = model.Location;
                maintenanceFromDb.Magor = model.Magor;

                _unitOfWork.MaintenanceRepository.Update(maintenanceFromDb);
                _unitOfWork.Save();
                TempData["success"] = $"Maintenance {maintenanceFromDb.Name} Updated ";
                return RedirectToAction("Index");

            }
        }
        public IActionResult Delete(int id)
        {
            var maintenanceFromDb = _unitOfWork.MaintenanceRepository.Get(t => t.Id == id);
            _unitOfWork.MaintenanceRepository.Remove(maintenanceFromDb);
            _unitOfWork.Save();
            TempData["success"] = $"Maintenance {maintenanceFromDb.Name} Updated ";
            return RedirectToAction("Index");
        }
    }
}

using Farm.DataAccess.Repository.IRepository;
using Farm.Models.VM;
using Farm.Utility;
using Microsoft.AspNetCore.Mvc;

namespace FarmVayCayTestOne.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BokingListController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BokingListController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string ? status)
        {
            var bookingList = _unitOfWork.BokingRepository.GetAll(includePropertes: "Custmor").ToList();
            if (status!=null)
            {
                if(status== "Completed")
                {
                    bookingList = _unitOfWork.BokingRepository.
                        GetAll(b => b.BookingStatus == SD.Booking_Completed, includePropertes: "Custmor").ToList();
                }
                if (status == "Canceled")
                {
                    bookingList = _unitOfWork.BokingRepository.
                        GetAll(b => b.BookingStatus == SD.Booking_Canceled, includePropertes: "Custmor").ToList();
                }
                if (status == "Booked")
                {
                    bookingList = _unitOfWork.BokingRepository.
                        GetAll(b => b.BookingStatus == SD.Booking_Booked, includePropertes: "Custmor").ToList();
                }
            }
            
            return View(bookingList);
        }
        public IActionResult DeleteBooking(int bookingId)
        {
            var bookingFromDb = _unitOfWork.BokingRepository.Get(b => b.Id == bookingId);
            if(bookingFromDb.BookingStatus==SD.Booking_Completed)
            {
                TempData["error"] = "This Booking is Completed You Can't Delete it";
                return RedirectToAction("Index");
            }
            TempData["success"] = $"Booking Id: {bookingId} Deleteed Successfuly!";
            _unitOfWork.BokingRepository.Remove(bookingFromDb);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
        public IActionResult Details(int bookingId) 
        {
            if(bookingId==0)
            {
                TempData["error"] = "Somthing Wrong Please Try Again !";
                return RedirectToAction("Index");
            }
            var booking = _unitOfWork.BokingRepository.Get(b => b.Id == bookingId,includePropertes: "Custmor,Farm");
            return View(booking);
        }
    }
}

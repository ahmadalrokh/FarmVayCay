using System.Security.Claims;
using Farm.DataAccess.Repository.IRepository;
using Farm.Models.Models;
using Farm.Models.VM;
using Farm.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmVayCayTestOne.Areas.Custmor.Controllers
{
    [Area("Custmor")]
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        //Dependanacy Injuction
        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // Get all You Booking
        [Authorize]
        public IActionResult MyBoking()
        {
            var claim=(ClaimsIdentity)User.Identity;
            string userId = claim.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Boking> bokingList = _unitOfWork.BokingRepository.GetAll(b => b.CustmorId == userId && b.IsDeleted==false,includePropertes:"Farm").ToList();
            if (bokingList != null)
            {
                List<FarmVM> models=new List<FarmVM>();
                foreach (var b in bokingList)
                {
                    var farmId = b.FarmId;
                    var farm = _unitOfWork.FarmsRepository.Get(f => f.Id == farmId);
                    var Images = _unitOfWork.FarmImagesRepository.GetAll(i => i.FarmId == farmId).ToList();
                    b.Farm= farm;
                    if(b.BookingStatus!=SD.Booking_Completed)
                    {
                        if (b.StartDate < DateTime.Now && b.BookingStatus != SD.Booking_Canceled)
                        {
                            b.IsCompleted = true;
                            b.BookingStatus = SD.Booking_Completed;
                            _unitOfWork.BokingRepository.Update(b);
                            _unitOfWork.Save(); 
                        }
                    }

                    FarmVM model = new()
                    {
                        Farms = farm,
                        boking = b,
                        farmImages = Images
                    };
                    models.Add(model);
                }

               return View(models);
            }
            
            return View();
        }
        [HttpGet]
        // Open Rating page
        public IActionResult Rating(int bookingId,int farmId)
        {
            var farm = _unitOfWork.FarmsRepository.Get(f => f.Id == farmId);
            var booking = _unitOfWork.BokingRepository.Get(b => b.Id == bookingId);

            RatingVM model = new()
            {
                Farm = farm,
                Boking=booking,
                Rating = new()
            };
            return View(model);
        }
        [HttpPost]
        // Rating Page Post
        public IActionResult Rating(RatingVM model)
        {
            var booking = _unitOfWork.BokingRepository.Get(b => b.Id == model.Boking.Id);
            var farm = _unitOfWork.FarmsRepository.Get(f => f.Id == booking.FarmId);
            if (model.Rating != null)
            {
                var Feedback = new Rating();
                Feedback.UserId = booking.CustmorId;
                Feedback.FarmId = booking.FarmId;
                Feedback.BookingId = booking.Id;
                Feedback.IsClean=model.Rating.IsClean;
                Feedback.IsFacilities=model.Rating.IsFacilities;
                Feedback.IsPrivacy=model.Rating.IsPrivacy;
                Feedback.Comment= model.Rating.Comment;
                Feedback.stars=model.Rating.stars;
                booking.IsRating = true;

                var ratingList = _unitOfWork.RatingRepository.GetAll(r => r.FarmId == farm.Id).ToList();

                var ratingCount=RatingCalculation(ratingList.Count+1, farm.Rating, Feedback.stars);
                farm.Rating = ratingCount;
                _unitOfWork.FarmsRepository.Update(farm);
                _unitOfWork.BokingRepository.Update(booking);
                _unitOfWork.RatingRepository.Add(Feedback);
                _unitOfWork.Save();

                TempData["success"] = "Your rating done succssefully";
                return RedirectToAction("MyBoking");     
            }
            TempData["error"] = "Somthing plest try again";
            return View(model);
        }
        // Calculate The Rating Avg
        public static double RatingCalculation(int count, double currentAverage, double newRating)
        {
            var result = ((currentAverage * (count - 1)) + newRating) / count;
            return Math.Round(result, 2); 
        }

        // Cancele Yor Booking 
        public IActionResult Cancelation(int BookingId)
        {
            var booking=_unitOfWork.BokingRepository.Get(b=>b.Id == BookingId);
            if (booking == null)
            {
                return NotFound();
            }
            booking.UpdateAt = DateTime.Now;
            booking.BookingStatus=SD.Booking_Canceled;
            _unitOfWork.BokingRepository.Update(booking);
            _unitOfWork.Save();
            TempData["success"] = "you have canceled your reservation succsessfully";
            return RedirectToAction("MyBoking");
        }
        // Remove Booking from MyBooking 
        public IActionResult DeleteBooking(int BookingId)
        {
            var booking = _unitOfWork.BokingRepository.Get(b => b.Id == BookingId);
            booking.IsDeleted = true;
            _unitOfWork.BokingRepository.Update(booking);
            _unitOfWork.Save();
            TempData["success"] = $"Booking ID: {BookingId} Deleted Successefully !";
            return RedirectToAction("MyBoking");
        }
       

    }
}

using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Farm.DataAccess.Repository.IRepository;
using Farm.Models.Models;
using Farm.Models.VM;
using Farm.Utility;
using FarmVayCayTestOne.Models;
using FarmVayCayTestOne.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FarmVayCayTestOne.Areas.Custmor.Controllers
{
    [Area("Custmor")]
    public class FarmsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailSender;
        //Dependancy Ingection
        public FarmsController(IUnitOfWork unitOfWork,EmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailService;
        }
        // Return Farms Page And Handel The Filter
        public IActionResult Index(string? members, string? budget, string? rating)
        {
            IQueryable<Farms> farmList = _unitOfWork.FarmsRepository.GetAll(f => f.IsApproved == true && f.IsAvailable == SD.Status_Available, includePropertes: "FarmImages");
            // members Filter
            if (!string.IsNullOrEmpty(members))
            {
                if (members == "0") // less 10
                {
                    farmList = farmList.Where(f => f.IsApproved == true && f.Guests <= 10);
                }
                else if (members == "1") //10-20
                {
                    farmList = farmList.Where(f => f.IsApproved == true && f.Guests <= 20 && f.Guests >= 10);
                }
                else if (members == "2")//more-20
                {
                    farmList = farmList.Where(f => f.IsApproved == true && f.Guests >= 20);
                }
            }
            // budget Filter
            if (!string.IsNullOrEmpty(budget))
            {
                if (budget == "0")//low
                {
                    farmList = farmList.Where(f => f.IsApproved == true && f.PricePerDay <= 100);
                }
                else if (budget == "1")//Mediom
                {
                    farmList = farmList.Where(f => f.IsApproved == true && f.PricePerDay >= 100 && f.PricePerDay <= 300);
                }
                else if (budget == "2")//High
                {
                    farmList = farmList.Where(f => f.IsApproved == true && f.PricePerDay >= 300);
                }
            }
            // rating Filter
            if (!string.IsNullOrEmpty(rating))
            {
                if (rating == "0")// 5 stars
                {
                    farmList = farmList.Where(f => f.IsApproved && f.Rating <= 5 && f.Rating > 4);       
                }
                else if (rating == "1") // 4 stars
                {
                    farmList = farmList.Where(f => f.IsApproved && f.Rating <= 4 && f.Rating > 3);
                }
                else if (rating == "2")// 3 stars
                {
                    farmList = farmList.Where(f => f.IsApproved && f.Rating > 2 && f.Rating <= 3 );
                }
                else if (rating == "3")// 2 stars
                {
                    farmList = farmList.Where(f => f.IsApproved && f.Rating > 1 && f.Rating <= 2);
                }
                else if (rating == "4")// 1 stars
                {
                    farmList = farmList.Where(f => f.IsApproved && f.Rating <= 1);
                }
            }
            
            return View(farmList.ToList());
        }
        // Farm Datals Paged get
        [HttpGet]
        public IActionResult Details(int FarmId)
        {
            Farms farmFromDb = _unitOfWork.FarmsRepository.Get(f => f.Id == FarmId);
            var user = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == farmFromDb.UserId);
            var feedbackes = _unitOfWork.RatingRepository.GetAll(r => r.FarmId == FarmId && r.Comment!=null).ToList();
            if (user != null)
            {
                farmFromDb.User = user;
            }
            if(feedbackes!=null)
            {
                foreach(var rating in feedbackes)
                {
                    ApplicationUser Custmor = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == rating.UserId);
                    rating.User = Custmor;
                }
            }
            var booking = _unitOfWork.BokingRepository.GetAll(b => b.FarmId == FarmId && b.BookingStatus!=SD.Booking_Canceled).ToList();
           
            var reservedDates = booking
                .SelectMany(b => Enumerable.Range(0, 1 + (b.EndDate - b.StartDate).Days)
                .Select(offset => b.StartDate.AddDays(offset)))
                .Select(d => d.ToString("yyyy-MM-dd"))
                .ToList();

            var unavailableRanges = _unitOfWork.UnavailableDatesRepository
            .GetAll(f => f.FarmId == FarmId)
            .ToList(); 

            var blockedDates = unavailableRanges
                .SelectMany(dateRange =>
                    Enumerable.Range(0, (dateRange.UnavailableDateTo - dateRange.UnavailableDateFrom).Days + 1)
                              .Select(offset => dateRange.UnavailableDateFrom.AddDays(offset).ToString("yyyy-MM-dd"))
                )
            .ToList();
            var allUnavailableDates = reservedDates.Concat(blockedDates).Distinct().ToList();

            DetailsVM model = new()
            {
                Farms = farmFromDb,
                farmImages = _unitOfWork.FarmImagesRepository.GetAll(img => img.FarmId == FarmId).ToList(),
                rating = feedbackes,
                ResevedDate = allUnavailableDates,
                Count=_unitOfWork.RatingRepository.GetAll(r=>r.FarmId==FarmId).ToList().Count()
            };
           
            return View(model);
        }
        // Add To Favarate Logic
        [Authorize]
        public IActionResult AddToFa(int FarmId)
        {
            var clamsIdentity = (ClaimsIdentity)User.Identity;
            var userId = clamsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            var farmFromDb = _unitOfWork.FavoriteRepository.Get(f => f.FarmId == FarmId && f.UserId == userId);
            if (farmFromDb == null)
            {
                Farms farm = _unitOfWork.FarmsRepository.Get(f => f.Id == FarmId);
                Favorite favorite = new()
                {
                    UserId = userId,
                    FarmId = FarmId,
                    Farm = farm
                };
                _unitOfWork.FavoriteRepository.Add(favorite);
                _unitOfWork.Save();
                TempData["success"] = $"Farm {farm.FarmName} Added To Favorite ";
                return RedirectToAction("Index", "Favorite");
            }
            else
            {
                //If Farms is Alredy Exisit in Favartt
                TempData["success"] = $"This Farm is in Favorite ";
                return RedirectToAction("Index");
            }

        }
        // Booking Function Post
        [Authorize]
        [HttpPost]
        [ActionName("Details")]
        public async Task <IActionResult> Reservation(FarmVM model,int farmId)
        {
            //TO Regect 01.01.0001
            if (model.boking.StartDate < DateTime.Now && model.boking.EndDate < DateTime.Now)
            {
                TempData["error"] = "you must choose a chick-in date and a check-out date !";
                return RedirectToAction("Details", new { farmId = model.Farms.Id });

            }

            var booking = _unitOfWork.BokingRepository.GetAll(b => b.FarmId == farmId && b.BookingStatus != SD.Booking_Canceled).ToList();

            var reservedDates = booking
                .SelectMany(b => Enumerable.Range(0, 1 + (b.EndDate - b.StartDate).Days)
                .Select(offset => b.StartDate.AddDays(offset)))
                .Select(d => d.ToString("yyyy-MM-dd"))
                .ToList();

            var unavailableRanges = _unitOfWork.UnavailableDatesRepository
             .GetAll(f => f.FarmId == farmId)
             .ToList();

            var blockedDates = unavailableRanges
                .SelectMany(dateRange =>
                    Enumerable.Range(0, (dateRange.UnavailableDateTo - dateRange.UnavailableDateFrom).Days + 1)
                              .Select(offset => dateRange.UnavailableDateFrom.AddDays(offset).ToString("yyyy-MM-dd"))
                )
            .ToList();

            var allUnavailableDates = reservedDates.Concat(blockedDates).Distinct().ToList();
            DateTime startData = model.boking.StartDate;
            DateTime endData = model.boking.EndDate;

            var userDates = Enumerable.Range(0, 1 + (endData - startData).Days)
                .Select(offset => startData.AddDays(offset).ToString("yyyy-MM-dd"))
                .ToList();

            foreach (var date in userDates)
            {
                if (allUnavailableDates.Contains(date))
                {
                    TempData["error"] = "One or more of the days you selected is already booked or unavailable. Please choose different dates.";
                    return RedirectToAction("Details", new { FarmId = farmId }); 
                }
            }
            
            //Error Handel 
            if (farmId == 0)
            {
                
                TempData["error"] = "Somthing Wrong Please try again.";
                return RedirectToAction("Index");
            }

            var farm = _unitOfWork.FarmsRepository.Get(f => f.Id == farmId);
            if (farm == null)
            {
                TempData["error"] = "Somthing Wrong Please try again.";
                return RedirectToAction("Index");
            }

            var claims=(ClaimsIdentity)User.Identity;
            string userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == userId);

            var Booking = new Boking()
            {
                FarmId = farmId,
                Farm = farm,
                CustmorId = userId,
                Custmor = user,
                CreatedAt = DateTime.Now,
                StartDate = model.boking.StartDate.Date.AddHours(9),  
                EndDate = model.boking.EndDate.Date.AddHours(9),     
                Guests = model.boking.Guests,
                PaymentStatus = SD.Payment_Pending,
                TotalPrice = PriceCalculation(farm.PricePerDay, model.boking.StartDate, model.boking.EndDate),
                BookingStatus = SD.Booking_Booked,
            };

            _unitOfWork.BokingRepository.Add(Booking);
            _unitOfWork.Save();
            TempData["success"] = "You must pay to continue booking. ";

            ApplicationUser Owner=_unitOfWork.ApplicationUserRepository.Get(u=>u.Id==farm.UserId);
            //Email To Send
            if (Owner != null)
            {
                try
                {
                    await _emailSender.SendEmailAsync(
                        Owner.Email,
                        $"Reservation Confirmation for Your Farm - {farm.FarmName}",
                        $@"
                        <p>Dear {Owner.FirstName},</p>
                        <p>We are pleased to inform you that your farm, <strong>{farm.FarmName}</strong>, has been successfully booked by one of our customers. Below are the details of the reservation:</p>
                        <ul>
                            <li><strong>Reservation Date:</strong> {Booking.StartDate}</li>
                            <li><strong>Time:</strong> 9:00 AM</li>
                            <li><strong>Number of Guests:</strong> {Booking.Guests}</li>
                            <li><strong>Reservation Duration:</strong> {(Math.Abs(model.boking.EndDate.Day - model.boking.StartDate.Day))} days</li>
                        </ul>
                        <p>We appreciate your cooperation and look forward to a fruitful experience with the guests. If you need to modify or cancel the reservation, please feel free to contact us directly.</p>
                        <p>Should you need any further assistance or details, don’t hesitate to reach out.</p>
                        <p>Best regards,<br/>FarmVayCay<br/>[farmVayCay@gmail.com]</p>"
                    );


                }
                catch
                {
                    // Avoid breaking the process
                    TempData["error"] = "Reservation completed, but failed to send confirmation email to the farm owner.";
                }
            }

            return RedirectToAction("ConfirmReservation", new { farmId = farmId, bookintId = Booking.Id });
            
        }
        // Calculate The Price
        public static double PriceCalculation(double pricePerDay, DateTime startDate, DateTime endDate)
        {
            int totalDays = (endDate - startDate).Days;
            if (totalDays <= 0) return 0; 
            return totalDays * pricePerDay;
        }
        // Conferm Resevation Function Get
        [HttpGet]
        public IActionResult ConfirmReservation(int farmId, int bookintId)
        {

            var farm = _unitOfWork.FarmsRepository.Get(
                 f => f.Id == farmId,
                 includePropertes: "FarmImages"
             );

            if (farm == null)
            {
                TempData["error"] = "Farm not found.";
                return RedirectToAction("Index");
            }

            var booking = _unitOfWork.BokingRepository.Get(b => b.Id == bookintId);
            if (booking == null)
            {
                TempData["error"] = "Booking not found.";
                return RedirectToAction("Index");
            }

            var model = new PaymentVM
            {
                Farms = farm,
                Boking = booking,
                VayCayBank = new(), 
                
                
            };

            return View(model);
        }

        // Conferm Resevation Function Post
        [HttpPost]
        public async Task<IActionResult> ConfirmReservation(PaymentVM model, int farmId, int bookingId)
        {
            var bankAccount = _unitOfWork.BankRepository.Get(a => a.CardNumber == model.VayCayBank.CardNumber);
            var booking = _unitOfWork.BokingRepository.Get(b => b.Id == bookingId);
            var customer = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == booking.CustmorId);
            var farm = _unitOfWork.FarmsRepository.Get(f => f.Id == booking.FarmId, includePropertes: "FarmImages");

            if (booking == null || customer == null || farm == null)
            {
                TempData["error"] = "Invalid reservation data.";
                return RedirectToAction("Index");
            }

            
            booking.Custmor = customer;
            booking.Farm = farm;

            if (bankAccount == null)
            {
                TempData["error"] = "This card is not found.";
                return RedirectToAction("ConfirmReservation", new { farmId = farmId, bookintId = booking.Id });
            }

            double remainingBalance = bankAccount.Balance - booking.TotalPrice;

            if (remainingBalance < 0)
            {
                TempData["error"] = "Failed: Your balance is not enough!";
                return RedirectToAction("ConfirmReservation", new { farmId = farmId, bookintId = bookingId });
            }

            bankAccount.Balance = remainingBalance;
            booking.PaymentStatus = SD.Payment_Approved;

            _unitOfWork.BokingRepository.Update(booking);
            _unitOfWork.BankRepository.Update(bankAccount);
            _unitOfWork.Save();

            //send Email
            try
            {
                await _emailSender.SendEmailAsync(
                    booking.Custmor.Email,
                    $"Your Reservation is Confirmed - {booking.Farm.FarmName}",
                    $@"
            <div style='font-family:Arial,sans-serif; color:#333'>
                <h2 style='color:#3dd5bf;'>Reservation Confirmed</h2>
                <p>Dear {booking.Custmor.FirstName},</p>
                <p>Thank you for booking <strong>{booking.Farm.FarmName}</strong>!</p>

                <ul>
                    <li><strong>Date:</strong> {booking.StartDate:yyyy-MM-dd}</li>
                    <li><strong>Guests:</strong> {booking.Guests}</li>
                    <li><strong>Duration:</strong> {Math.Abs((booking.EndDate - booking.StartDate).Days)} days</li>
                    <li><strong>Time:</strong> 9:00 AM</li>
                </ul>

                <p>We look forward to having you!</p>
                <p>Best regards,<br/>FarmVayCay Team<br/><a href='mailto:farmVayCay@gmail.com'>farmVayCay@gmail.com</a></p>
            </div>"
                );
            }
            catch
            {
                TempData["error"] = "Reservation confirmed, but failed to send confirmation email.";
            }

                TempData["success"] = "Payment is paid successfully!";
            return RedirectToAction("PaymentSccessfull");
        }

        // Open Payment Sccessfully Page
        public async Task<IActionResult> PaymentSccessfull()
        {
            return View();
        }

    }
}

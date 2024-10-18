using DataAccess.Repository.IRepository;
using Hotels_Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Models;
using Models.ViewModel;
using Newtonsoft.Json;
using System.Diagnostics;
using Utility;

namespace Hotels_Booking.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController(ILogger<HomeController> logger,
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork) : Controller
    {
        #region Start Up
        private readonly ILogger<HomeController> _logger = logger;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        #endregion
        #region Home
        [HttpGet]
        public IActionResult Index()
        {
            #region Recive filteration data
            var homeFiltrationJson = TempData["HomeFiltrationView"] as string;
            var HomeFiltrationView = JsonConvert.DeserializeObject<HomeFiltrationView>(homeFiltrationJson ?? "");
            HomeFiltrationView ??= new HomeFiltrationView { StateId = 0 };
            #endregion
            #region prepare HomeView varible  
            var HomeView = new HomeView
            {
                HomeFiltrationView = HomeFiltrationView,
                States = _unitOfWork.Hotels.Get(
                e =>
                e.Status == Status.Published,
                e => e.State,
                e => e.State.Country
                ).Select(e => e.State
                ).Distinct(
                ).ToList()
            };
            #endregion
            #region Filtration

            var Hotels = _unitOfWork.Hotels.Get(
                e => e.Status == Status.Published &&
                (HomeFiltrationView.StateId == 0 || e.StateId == HomeFiltrationView.StateId) &&
                e.Rooms.Any(
                    room => room.RoomViews.Any(
                        view => view.RoomUnits.Any(
                            unit => !unit.ReservationRooms.Any(
                                reservationRoom =>
                                    (reservationRoom.Reservation.ReservationStatus == ReservationStatus.Pending ||
                                    reservationRoom.Reservation.ReservationStatus == ReservationStatus.Confirmed) &&
                                    HomeFiltrationView.CheckInDate != null && HomeFiltrationView.CheckOutDate != null &&
                                    reservationRoom.Reservation.CheckInDate <= HomeFiltrationView.CheckOutDate &&
                                    reservationRoom.Reservation.CheckOutDate >= HomeFiltrationView.CheckInDate
                            )
                        )
                    )
                ),
                e => e.State,
                e => e.State.Country,
                e => e.HotelGalleries.Where(gallery => gallery.IsMainImage)
            ).ToList();

            foreach (var hotel in Hotels)
                {
                    var hotelHome = new HotelHomeView
                    {
                        HotelId = hotel.HotelId,
                        HotelName = hotel.HotelName,
                        HotelRating = hotel.Rating,
                        HotelImage = hotel.HotelGalleries.First().ImageName,
                        HotelState = $"{hotel.State.Country.Name}, {hotel.State.Name}",
                        HotelStartPrice = _unitOfWork.Rooms.Get(
                            e =>
                            e.HotelId == hotel.HotelId,
                            e => e.RoomViews)
                            .SelectMany(room => room.RoomViews)
                            .Min(rv => rv.Price)
                    };
                    HomeView.HotelHomeViews.Add(hotelHome);
                }      
            #endregion

            return View(HomeView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FilterHome(HomeFiltrationView HomeFiltrationView)
        {
            if (HomeFiltrationView == null) return BadRequest();
            if(HomeFiltrationView.CheckInDate != null && HomeFiltrationView.CheckOutDate != null
                && !(HomeFiltrationView.CheckInDate >= DateTime.Now &&
                HomeFiltrationView.CheckInDate <= HomeFiltrationView.CheckOutDate))
                return BadRequest();

            TempData["HomeFiltrationView"] = JsonConvert.SerializeObject(HomeFiltrationView);

            return RedirectToAction("Index");
        }
        #endregion
        #region Hotel Details
        [Authorize]
        public async Task<IActionResult> Hotel(HotelDetailsFiltrationn HotelDetailsFiltrationn)
        {
            #region Check If the Sended Data Is Vailed
            if (
                HotelDetailsFiltrationn.HotelId == 0 ||
                (
                HotelDetailsFiltrationn != null &&
                (
                HotelDetailsFiltrationn.CheckInDate == null && HotelDetailsFiltrationn.CheckOutDate != null ||
                HotelDetailsFiltrationn.CheckInDate != null && HotelDetailsFiltrationn.CheckOutDate == null ||
                (
                HotelDetailsFiltrationn.CheckInDate != null && HotelDetailsFiltrationn.CheckOutDate != null &&
                (
                HotelDetailsFiltrationn.CheckInDate < DateTime.Today ||
                HotelDetailsFiltrationn.CheckInDate > HotelDetailsFiltrationn.CheckOutDate
                )
                )
                )
                )
            )
                return BadRequest();
            #endregion

            #region set return variable and get the Hotel intail data 

            HotelDetailsFiltrationn ??= new HotelDetailsFiltrationn();

            var HotelDetails = new HotelDetailsUser
            {
                HotelDetailsFiltrationn = new HotelDetailsFiltrationn(HotelDetailsFiltrationn),
                Hotel = _unitOfWork.Hotels.GetOne(
                    e => e.HotelId == HotelDetailsFiltrationn.HotelId && e.Status == Status.Published,
                    e => e.Facilities,
                    e => e.State,
                    e => e.State.Country,
                    e => e.HotelGalleries
                )
            };

            if (HotelDetails.Hotel == null) return BadRequest();
            #endregion

            #region get hotel reviews
            var reviews = _unitOfWork.UserReviews.Get(
                e => 
                    e.HotelId == HotelDetailsFiltrationn.HotelId &&
                    e.ReviewStatus == ReviewStatus.Approved,
                reservation => reservation.Reservation
                ).ToList();

            foreach(var review in reviews)
            {
                #region get user
                var user = await _userManager.FindByIdAsync(review.Reservation.UserId);

                if (user == null) continue;
                #endregion

                var reviewview = new ReviewHotelUser
                {
                    Name = review.Name ?? "",
                    Review = review.Review ?? "",
                    CreatedAt = review.CreatedAt?.ToString("yyyy-MM-dd") ?? "",
                    Rating = review.Rating,
                    UserProfilePhoto = user.ProfileImage
                };

                HotelDetails.ReviewHotelUsers.Add(reviewview);
            }
            #endregion

            #region Rooms Filteration
            HotelDetails.Hotel.Rooms = _unitOfWork.Rooms.Get(
                e => 
                    e.HotelId == HotelDetailsFiltrationn.HotelId &&
                    (HotelDetailsFiltrationn.RoomType == 0 ||
                    e.RoomType.RoomTypeId == HotelDetailsFiltrationn.RoomType),
                roomimages => roomimages.RoomGalleries,
                RoomType => RoomType.RoomType
                ).ToList();

            foreach(var room in HotelDetails.Hotel.Rooms)
            {
                room.RoomViews = _unitOfWork.RoomViews.Get(
                    e => 
                    e.RoomId == room.RoomID &&
                     (HotelDetailsFiltrationn.RoomView == 0 ||
                    e.LocationViewId == HotelDetailsFiltrationn.RoomView),
                    locationview => locationview.LocationView
                    ).ToList();

                HotelDetails.RoomTypes.Add(room.RoomType);

                foreach (var roomview in room.RoomViews)
                {
                    roomview.RoomUnits = _unitOfWork.RoomUnits.Get(
                        e => 
                        e.RoomViewId == roomview.RoomViewId &&
                        !e.ReservationRooms.Any(
                            reservationRoom =>
                                    (reservationRoom.Reservation.ReservationStatus == ReservationStatus.Pending ||
                                    reservationRoom.Reservation.ReservationStatus == ReservationStatus.Confirmed) &&
                                    HotelDetails.HotelDetailsFiltrationn.CheckInDate != null &&
                                    HotelDetails.HotelDetailsFiltrationn.CheckOutDate != null &&
                                    reservationRoom.Reservation.CheckInDate <= HotelDetails.HotelDetailsFiltrationn.CheckOutDate &&
                                    reservationRoom.Reservation.CheckOutDate >= HotelDetails.HotelDetailsFiltrationn.CheckInDate
                            )
                        ).ToList();

                    if(!HotelDetails.LocationViews.Contains(roomview.LocationView))
                        HotelDetails.LocationViews.Add(roomview.LocationView);
                }
            }
            #endregion

            return View(HotelDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RoomFiltration(HotelDetailsFiltrationn HotelDetailsFiltrationn)
        {
            return RedirectToAction("Hotel",new
            {
                HotelDetailsFiltrationn.RoomView,
                HotelDetailsFiltrationn.RoomType,
                HotelDetailsFiltrationn.CheckInDate,
                HotelDetailsFiltrationn.CheckOutDate,
                HotelDetailsFiltrationn.HotelId
            });
        }
        #endregion
        #region Contact
        [Authorize(Roles = "User")]
        public IActionResult Contact() => View(new ContactView { States = _unitOfWork.States.Get(null, c => c.Country).ToList() });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateContact(Contact Contact)
        {
            if (ModelState.IsValid)
            {
                Contact.UserId = _userManager.GetUserId(User) ?? "";
                _unitOfWork.Contacts.Add(Contact);
                TempData["ConfirmContact"] = "Thank you! Your message has been submitted. We'll respond via email shortly.";
            }
            return RedirectToAction("Contact");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateHotelRequest(HotelRequest HotelRequest)
        {
            if (ModelState.IsValid)
            {
                HotelRequest.UserId = _userManager.GetUserId(User) ?? "";
                _unitOfWork.HotelRequests.Add(HotelRequest);
                TempData["ConfirmContact"] = "Thank you! Your Request has been submitted. We'll respond via email shortly.";
            }
            return RedirectToAction("Contact");
        }
        #endregion
        #region #
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}

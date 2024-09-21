using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Models.ViewModel;
using Utility;

namespace Hotels_Booking.Areas.HotelManager.Controllers
{
    [Area("HotelManager")]
    [Authorize(Roles = "HotelManager")]
    public class HotelManagementController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork) : Controller
    {
        #region Start Up
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private int HotelId;
        private Status Status;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null) HotelId = user.HotelId ?? 0;

            Status = _unitOfWork.Hotels.GetOne(e => e.HotelId == HotelId)?.Status ?? Status;
            if (Status == Status.AdminReview)
                TempData["EditData"] = "No Changes Can Be Made";

            await next();
        }
        #endregion
        public IActionResult Index()
        {
            HotelStatusView hotelStatusView = new();
            
            var hotel = _unitOfWork.Hotels.GetOne(
                e => e.HotelId == HotelId,
                e => e.Facilities,
                e => e.HotelGalleries
                );
            
            if( hotel == null ) return NotFound();

            hotel.Rooms = _unitOfWork.Rooms.Get(
                e => e.HotelId == HotelId,
                e => e.RoomGalleries,
                e => e.RoomViews
                ).ToList();

            hotelStatusView.HotelName = hotel.HotelName;
            hotelStatusView.HotelId = HotelId;
            hotelStatusView.HotelStatus = hotel.Status;
            hotelStatusView.HotelFacilities = hotel.Facilities.Count;
            hotelStatusView.HotelMainImage = hotel.HotelGalleries.Any(e => e.IsMainImage);
            hotelStatusView.HotelImages = hotel.HotelGalleries.Count(hotel => !hotel.IsMainImage);
            hotelStatusView.Rooms = hotel.Rooms.Count;
            hotelStatusView.RoomsMainImages = hotel.Rooms.All(e => e.RoomGalleries.Count(e => e.IsMainImage) == 1) && hotelStatusView.Rooms > 0;
            hotelStatusView.RoomsTotalImageNumber = hotel.Rooms.Sum(e => e.RoomGalleries.Count(e => !e.IsMainImage));
            hotelStatusView.AllRoomsHave_5_Images = hotel.Rooms.All(e => e.RoomGalleries.Count(e => !e.IsMainImage) >= 5) && hotelStatusView.Rooms > 0;
            hotelStatusView.AllRoomsHaveView = hotel.Rooms.All(e => e.RoomViews.Count >= 1) && hotelStatusView.Rooms > 0;
            hotelStatusView.TotalHotelRoomUnits = hotel.Rooms.Sum(e => e.RoomViews.Sum(e => e.NumberRooms));
            hotelStatusView.UnitAtLeast_10 = hotelStatusView.TotalHotelRoomUnits >= 10;

            return View(hotelStatusView);
        }
        public IActionResult AdminReview(int id)
        {
            if(id != HotelId) return BadRequest();

            var hotel = _unitOfWork.Hotels.GetOne(e => e.HotelId == id);

            if(hotel == null) return BadRequest();

            if (hotel.Status != Status.Adminsuspended)
            {
                hotel.Status = hotel.Status ==
                Status.AdminReview ? Status.InProgress : Status.AdminReview;
                _unitOfWork.Hotels.Update(hotel);

                if (hotel.Status == Status.InProgress)
                    TempData["EditData"] = null;
            }

            return RedirectToAction("Index");
        }
        public IActionResult SuspendPublish(int id)
        {
            if (id != HotelId) return BadRequest();

            var hotel = _unitOfWork.Hotels.GetOne(e => e.HotelId == id);

            if (hotel == null) return BadRequest();

            if (hotel.Status != Status.Adminsuspended)
            {
                hotel.Status = hotel.Status ==
                    Status.Published ? Status.Managersuspended : Status.Published;
                _unitOfWork.Hotels.Update(hotel);
            }

            return RedirectToAction("Index");
        }
    }
}

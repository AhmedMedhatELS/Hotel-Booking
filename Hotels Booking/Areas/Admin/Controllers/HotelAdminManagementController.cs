using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Utility;

namespace Hotels_Booking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HotelAdminManagementController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : Controller
    {
        #region Start Up
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        #endregion
        public IActionResult HotelList() => View(_unitOfWork.Hotels.Get(null).ToList());
        public IActionResult HotelDetail(int id)
        {
            var hotel = _unitOfWork.Hotels.GetOne(
                e => e.HotelId == id,
                e => e.Facilities,
                e => e.HotelGalleries
                );

            if (hotel == null) return BadRequest();

            if(hotel.Status == Status.InProgress) return RedirectToAction("HotelList");

            var state = _unitOfWork.States.GetOne(e => e.StateId == hotel.StateId, e => e.Country);

            if (state != null)
                hotel.State = state;

            hotel.Rooms = _unitOfWork.Rooms.Get(
                e => e.HotelId ==  id,
                e => e.RoomViews,
                e => e.LocationViews,
                e => e.RoomGalleries
                ).ToList();

            return View(hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateHotelStatus(int HotelId,Status Status)
        {
            if(!ModelState.IsValid) return BadRequest();

            var hotel = _unitOfWork.Hotels.GetOne(e => e.HotelId == HotelId);

            if(hotel == null) return BadRequest();

            hotel.Status = Status;

            _unitOfWork.Hotels.Update(hotel);

            return RedirectToAction("HotelDetail", new {id = HotelId});
        }

    }
}

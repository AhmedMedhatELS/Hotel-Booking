using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Hotels_Booking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReservationController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : Controller
    {
        #region Start Up
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        #endregion
        #region Get Reservations For Sepecific Hotel
        public async Task<IActionResult> ResrevationHotel(int id)
        {
            if (_unitOfWork.Hotels.GetOne(e => e.HotelId == id) == null) return BadRequest();

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.HotelId = id;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("AllReservation", "Reservation",new {area = "HotelManager" });
        }
        #endregion
        #region Get All Reservations
        public async Task<IActionResult> AllResrevationHotel()
        {            
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.HotelId = null;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("AllReservation", "Reservation", new { area = "HotelManager" });
        }
        #endregion
    }
}

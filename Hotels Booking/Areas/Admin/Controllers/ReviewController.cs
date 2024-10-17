using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Models;
using Models.ViewModel;
using System.Text.Encodings.Web;
using Utility;

namespace Hotels_Booking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReviewController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IEmailSender emailSender) : Controller
    {
        #region Start Up
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IEmailSender _emailSender = emailSender;
        #endregion
        public IActionResult Reviews()
        {

            var reviews = _unitOfWork.UserReviews.Get(
                null,
                reservation => reservation.Reservation);

            return View();
        }
    }
}

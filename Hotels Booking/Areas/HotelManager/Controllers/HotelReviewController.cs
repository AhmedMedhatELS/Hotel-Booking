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
    public class HotelReviewController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork) : Controller
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
            if (Status == Utility.Status.AdminReview)
                TempData["EditData"] = "No Changes Can Be Made";

            await next();
        }
        #endregion
        public async Task<IActionResult> Reviews()
        {
            var reviews = _unitOfWork.UserReviews.Get(
                e =>
                    e.HotelId == HotelId &&
                    e.ReviewStatus == ReviewStatus.Approved,
                reservation => reservation.Reservation
                    ).ToList();

            List<ReviewHotelMangerView> ReviewHotelMangerViews = [];

            foreach(var review in reviews)
            {
                #region get user
                var user = await _userManager.FindByIdAsync(review.Reservation.UserId);

                if (user == null) continue;
                #endregion

                var reviewview = new ReviewHotelMangerView 
                {
                    Name = review.Name ?? "",
                    Rating = review.Rating,
                    Review = review.Review ?? "",
                    CreatedAt = review.CreatedAt?.ToString("yyyy-MM-dd") ?? "",
                    CheckInDate = review.Reservation.CheckInDate.ToString("yyyy-MM-dd"),
                    CheckOutDate = review.Reservation.CheckOutDate.ToString("yyyy-MM-dd"),
                    UserProfilePhoto = user.ProfileImage
                };

                ReviewHotelMangerViews.Add(reviewview);
            }

            return View(ReviewHotelMangerViews);
        }
    }
}

using Azure;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Models;
using Models.ViewModel;
using Stripe;
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
        #region Get all the Reviews
        public async Task<IActionResult> Reviews()
        {
            #region Get all the reviews
            var reviews = _unitOfWork.UserReviews.Get(
                e => e.ReviewStatus != ReviewStatus.InProgress,
                reservation => reservation.Reservation,
                hotel => hotel.Hotel);
            #endregion
            // List for the view
            List<ReviewAdminView> ReviewAdminViews = [];
            #region prepare each review to the view
            foreach (var review in reviews)
            {
                #region get user
                var user = await _userManager.FindByIdAsync(review.Reservation.UserId);

                if (user == null) continue;
                #endregion
                #region prepare the each review for the view 
                var reviewview = new ReviewAdminView
                {
                    Name = review.Name ?? "",
                    Review = review.Review ?? "",
                    CreatedAt = review.CreatedAt?.ToString("yyyy-MM-dd") ?? "",
                    CheckInDate = review.Reservation.CheckInDate.ToString("yyyy-MM-dd"),
                    CheckOutDate = review.Reservation.CheckOutDate.ToString("yyyy-MM-dd"),
                    Rating = review.Rating,
                    ReviewId = review.UserReviewId,
                    HotelName = review.Hotel.HotelName,
                    UserProfilePhoto = user.ProfileImage,
                    ReviewStatus = review.ReviewStatus                   
                };
                #endregion
                //add view to list 
                ReviewAdminViews.Add(reviewview);
            }
            #endregion
            return View(ReviewAdminViews);
        }
        #endregion
        #region Send Review Response
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendReviewResponse(ReviewResponseView ReviewResponseView)
        {
            // cheack for the vaildation of the recived data
            if (!ModelState.IsValid) return BadRequest();
            #region get the review and cheack on it
            var review = _unitOfWork.UserReviews.GetOne(
                e =>
                    e.UserReviewId == ReviewResponseView.ReviewId,
                reservation => reservation.Reservation);            

            if (review == null) return BadRequest();
            #endregion
            #region get the user and cheack on it
            var user = await _userManager.FindByIdAsync(review.Reservation.UserId);

            if (user == null) return BadRequest();
            #endregion
            #region Send the response
            await _emailSender.SendEmailAsync(
                 user.Email ?? "",
                 "Response to Your Review",
                 $@"
                <p>Dear {review.Name},</p>
    
                <p>Thank you for your feedback on your recent stay. Your review has been:</p>
                <blockquote>
                    <p><strong>{review.Review}</strong></p>
                </blockquote>
    
                <p>We would like to inform you that your review status has been updated to: <strong>{ReviewResponseView.ReviewStatus}</strong>.</p>
    
                <p>Admin Response:</p>
                <blockquote>
                    <p>{ReviewResponseView.Response}</p>
                </blockquote>
    
                <p>If you have any further questions or need additional assistance, feel free to contact us.</p>
    
                <p>Best regards,<br>
                Hotel Booking Team</p>
                "
             );
            #endregion
            #region update the review status
            review.ReviewStatus = ReviewResponseView.ReviewStatus;

            _unitOfWork.UserReviews.Update(review);
            #endregion
            //redirect to the total reviews
            return RedirectToAction("Reviews");
        }
        #endregion 
        #region Delete Review
        public IActionResult DeleteReview(int reviewId)
        {
            #region cheack the valition of the data
            if (reviewId == 0) return BadRequest();

            var review = _unitOfWork.UserReviews.GetOne(
                    e => e.UserReviewId == reviewId,
                    reservation => reservation.Reservation
                );

            if (review == null) return NotFound();
            #endregion

            #region Delete the Review
            review.Reservation.IsReviewOn = false;
            _unitOfWork.Reservations.Update(review.Reservation);

            _unitOfWork.UserReviews.Remove(review);
            #endregion
            return RedirectToAction("Reviews");
        }
        #endregion
    }
}

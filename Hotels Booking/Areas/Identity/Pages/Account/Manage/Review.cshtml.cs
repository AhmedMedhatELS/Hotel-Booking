// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using Models;
using Models.ViewModel;
using Newtonsoft.Json;
using Utility;

namespace Hotels_Booking.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ReviewModel(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork) : PageModel
    {
        #region Start Up
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public List<ReviewProfileView> ReviewProfileViews { get; set; } = [];
        #endregion
        #region Input
        [BindProperty(SupportsGet = true)]
        public int ReviewId { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            public int ReviewId { get; set; }
            [Required]
            [StringLength(15, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 15 characters.")]
            public string Name { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 15, ErrorMessage = "Review must be between 15 and 100 characters.")]
            public string Review { get; set; }

            [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
            public int Rating { get; set; }
        }
        #endregion
        #region Get All The Reviews
        public IActionResult OnGetAsync()
        {
            #region Get user id and cheack it
            var userId = _userManager.GetUserId(User);

            if (userId == null) return NotFound();
            #endregion
            #region get all the reviews for the user
            var reservations = _unitOfWork.Reservations.Get(
                    e => 
                        e.UserId == userId &&
                        e.CheckInDate <= DateTime.Now &&
                        e.ReservationStatus == ReservationStatus.Confirmed &&
                        e.IsReviewOn,
                    review => review.UserReview
                ).ToList();
            #endregion
            #region cheack for each review and prepare it to be in the View
            foreach (var reservation in reservations) 
            {
                #region cheack if the review exists if not make new one
                if (reservation.UserReview == null)
                {
                    reservation.UserReview = new UserReview
                    {
                        ReservationId = reservation.ReservationId,
                        CreatedAt = reservation.CheckOutDate,
                        HotelId = _unitOfWork.ReservationRooms.Get(
                            e => e.ReservationId == reservation.ReservationId,
                            roomunits => roomunits.RoomUnit,
                            roomview => roomview.RoomUnit.RoomView,
                            locationview => locationview.RoomUnit.RoomView.LocationView,
                            room => room.RoomUnit.RoomView.Room,
                            hotel => hotel.RoomUnit.RoomView.Room.Hotel
                            ).ToList()[0].RoomUnit.RoomView.Room.HotelId,
                    };

                    _unitOfWork.UserReviews.Add(reservation.UserReview);
                }
                #endregion
                #region make the review for the view and add it to the list
                var reviewView = new ReviewProfileView 
                { 
                    CheckInDate = reservation.CheckInDate.ToString("yyyy-MM-dd"),
                    CheckOutDate = reservation.CheckOutDate.ToString("yyyy-MM-dd"),
                    ReservationId = reservation.ReservationId,
                    CreatedAt = reservation.UserReview.CreatedAt?.ToString("yyyy-MM-dd"),
                    HotelName = _unitOfWork.Hotels.GetOne(
                                    e =>
                                        e.HotelId == reservation.UserReview.HotelId).HotelName,
                    Name = reservation.UserReview.Name,
                    Rating = reservation.UserReview.Rating,
                    Review = reservation.UserReview.Review,
                    ReviewId = reservation.UserReview.UserReviewId,
                    Status = reservation.UserReview.ReviewStatus                   
                };

                ReviewProfileViews.Add(reviewView);
                #endregion
            }
            #endregion
            return Page();
        }
        #endregion
        #region Save Changes
        public IActionResult OnPostAsync()
        {
            #region Check the Vildetion for all the data
            if (Input ==  null) return BadRequest();

            var userid = _userManager.GetUserId(User);
            if (userid == null) return NotFound();

            if (!ModelState.IsValid)  return Page();

            var review = _unitOfWork.UserReviews.GetOne(
                e => 
                    e.UserReviewId == Input.ReviewId,
                reservation => reservation.Reservation);

            if(review == null || review.Reservation.UserId != userid) return NotFound();

            if(review.ReviewStatus == ReviewStatus.Pending || review.ReviewStatus == ReviewStatus.Edited) return BadRequest();
            #endregion

            #region Save Changes 
            if (Input.Name != review.Name ||
                Input.Review !=  review.Review ||
                Input.Rating != review.Rating)
            {
                review.Review = Input.Review;
                review.Rating = Input.Rating;
                review.Name = Input.Name;
                review.ReviewStatus = 
                    review.ReviewStatus == ReviewStatus.InProgress ?
                        ReviewStatus.Pending : ReviewStatus.Edited;

                _unitOfWork.UserReviews.Update(review);
            }
            #endregion
            return RedirectToPage();
        }
        #endregion
        #region Delete Review
        public IActionResult OnGetDeleteReviewAsync()
        {
            #region cheack the valition of the data
            if (ReviewId == 0) return BadRequest();

            var userId = _userManager.GetUserId(User);
            if(userId == null) return NotFound();

            var review = _unitOfWork.UserReviews.GetOne(
                    e => e.UserReviewId == ReviewId,
                    reservation => reservation.Reservation
                );

            if(review == null || review.Reservation.UserId != userId) return NotFound();
            #endregion

            #region Delete the Review
            review.Reservation.IsReviewOn = false;
            _unitOfWork.Reservations.Update(review.Reservation);

            _unitOfWork.UserReviews.Remove(review);
            #endregion
            return RedirectToPage();
        }
        #endregion
    }
}

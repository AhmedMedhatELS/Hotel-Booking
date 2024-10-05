using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Stripe.Checkout;
using Stripe;
using Models.ViewModel;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Utility;

namespace Hotels_Booking.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class PaymentController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : Controller
    {
        #region Start Up
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
		#endregion
		#region Pay
		[HttpPost]
        public IActionResult Pay(GuestInformation GuestInformation)
        {
            if (GuestInformation == null || GuestInformation.ReservationFormView == null) return BadRequest();

            TempData["Reservation"] = JsonConvert.SerializeObject(GuestInformation);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = ["card"],
                LineItems =
                [
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Total"
                            },
                            UnitAmount = (long)GuestInformation.ReservationFormView.Total * 100
                        },
                        Quantity = 1,
                    },
                ],
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Reservation/CompleateReservation?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Payment/cancel",
            };
            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }
		#endregion
		#region Success , Cancel and Auto cancel
		public IActionResult Success()
        {
            return View();
        }
		public IActionResult Cancel()
		{
			return View();
		}
        public IActionResult AutoCancel()
        {
            return View();
        }
        #endregion
        #region Refund
        public IActionResult Refund(string paymentIntentId,ViewNavigation viewNavigation)
		{
			var refundOptions = new RefundCreateOptions
			{
				PaymentIntent = paymentIntentId
			};

			var refundService = new RefundService();
			var refund = refundService.Create(refundOptions);

            if (viewNavigation == ViewNavigation.AutoCancel)
                return RedirectToAction("AutoCancel");
            else if (viewNavigation == ViewNavigation.ManagerReservation)
                return RedirectToAction("AllReservation", "Reservation", new { area = "HotelManager" });
            else if (viewNavigation == ViewNavigation.UserReservation)
                return RedirectToPage("/Account/Manage/Reservation", new { area = "Identity" });
            else
                return NotFound();
		}
		#endregion

	}

}

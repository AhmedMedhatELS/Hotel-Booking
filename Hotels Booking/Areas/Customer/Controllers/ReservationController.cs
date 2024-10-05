using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModel;
using Utility;
using Stripe.Checkout;
using Stripe;
using Newtonsoft.Json;

namespace Hotels_Booking.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ReservationController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : Controller
    {
        #region Start Up
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        #endregion
        #region Reservation Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReservationDetails(ReservationFormView ReservationFormView)
        {
            #region Check the vildition of the recevied Data
            if (
                ReservationFormView == null ||
                ReservationFormView.Total <= 0 ||
                ReservationFormView.RoomViewData == null ||
                string.IsNullOrEmpty(ReservationFormView.RoomViewData) ||
                ReservationFormView.CheckInDate > ReservationFormView.CheckOutDate
                )
                return BadRequest();
            #endregion
            #region Get User Data
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return BadRequest();
            #endregion
            #region Get Hotel Details
            var hotel = _unitOfWork.Hotels.GetOne(
                e =>
                    e.HotelId == ReservationFormView.HotelId,
                state => state.State,
                countery => countery.State.Country
                );

            if (hotel == null) return BadRequest();
            #endregion
            #region  Prepare reservationview
            var reservationview = new ReservationDetailsView
            {
                HotelName = hotel.HotelName,
                HotelAddress = $"{hotel.Address}, {hotel.State.Name}, {hotel.State.Country.Name}",
                HotelLocation = hotel.LocationLink,
                HotelStars = hotel.Stars,
                NumberNights = (ReservationFormView.CheckOutDate - ReservationFormView.CheckInDate).Days + 1,
                GuestInformation = new GuestInformation
                {
                    Name = user.FullName,
                    Email = user.Email,
                    CounteryId = _unitOfWork.States.GetOne(
                    e =>
                        user.StateId == null ||
                        e.StateId == user.StateId)?.CountryId,
                    PhoneNumber = user.PhoneNumber,
                    ReservationFormView = new ReservationFormView(ReservationFormView)
                },
                Countries = _unitOfWork.Countries.Get(null).ToList()
            };
            #endregion
            #region Prepare Rooms List

            var reservationRoomViews = ReservationFormView.RoomViewData.Split(',');
            foreach (var reservationroomview in reservationRoomViews)
            {
                var parts = reservationroomview.Split('-');
                var roomViewId = int.Parse(parts[0]);
                var quantity = int.Parse(parts[1]);

                var roomview = _unitOfWork.RoomViews.GetOne(
                    e =>
                        e.RoomViewId == roomViewId,
                    room => room.Room,
                    location => location.LocationView
                    );

                if (roomview == null) BadRequest();

                reservationview.NumberRooms += quantity;
                reservationview.Rooms.Add($"{quantity} {roomview?.Room.Name} ({roomview?.LocationView.Name})");
            }
            #endregion
            
            return View(reservationview);
        }
        #endregion
        #region Reservation        
        public IActionResult CompleateReservation(string session_id)
        {
			#region Get Payment Id
			if (string.IsNullOrEmpty(session_id))
				return BadRequest("Session ID is missing.");

			var service = new SessionService();
			var session = service.Get(session_id);

			// Access payment details from the session
			var paymentIntentId = session.PaymentIntentId;
			#endregion
			#region receavie the data
			if (TempData["Reservation"] is not string serializedReservation)
				return RedirectToAction("Refund", "Payment", new { paymentIntentId, viewNavigation = ViewNavigation.AutoCancel });

		    var guestInformation = JsonConvert.DeserializeObject<GuestInformation>(serializedReservation);

			if (guestInformation == null ||
                guestInformation.ReservationFormView is not ReservationFormView ReservationFormView)
				return RedirectToAction("Refund", "Payment", new { paymentIntentId, viewNavigation = ViewNavigation.AutoCancel });
			#endregion
			#region Check the vildition of the recevied Data			
			if (
                ReservationFormView.Total <= 0 ||
                ReservationFormView.RoomViewData == null ||
                string.IsNullOrEmpty(ReservationFormView.RoomViewData) ||
                ReservationFormView.CheckInDate > ReservationFormView.CheckOutDate
                )
				return RedirectToAction("Refund", "Payment", new { paymentIntentId, viewNavigation = ViewNavigation.AutoCancel });
			#endregion
			#region Make Reservation
			var reservarion = new Reservation
            {
                Name = guestInformation.Name,
                Email = guestInformation.Email,
                PhoneNumber = guestInformation.PhoneNumber,
                CountryId = guestInformation.CounteryId,
                SpecialRequests = guestInformation.SpecialRequests,
                ReservationStatus = ReservationStatus.Pending,
                CheckInDate = ReservationFormView.CheckInDate,
                CheckOutDate = ReservationFormView.CheckOutDate,
                TotalAmount = ReservationFormView.Total,
                UserId = _userManager?.GetUserId(User) ?? "",
                PaymentId = paymentIntentId,
                PaymentDate = DateTime.Now
            };            
            #endregion
            #region Prepre lists
            var reservationRoomViews = ReservationFormView.RoomViewData.Split(',');
            List<RoomView> RoomViews = [];
            //key is roomviewid and value is the quantity
            var roomViewSelectionsList = new Dictionary<int, int>();
            #endregion
            #region Check for the Units
            foreach (var reservationoomview in reservationRoomViews) 
            {
                var parts = reservationoomview.Split('-');
                var roomViewId = int.Parse(parts[0]);
                var quantity = int.Parse(parts[1]);

                var roomview = _unitOfWork.RoomViews.GetOne(e => e.RoomViewId == roomViewId);

                if(roomview == null) return RedirectToAction("Refund", "Payment", new { paymentIntentId, viewNavigation = ViewNavigation.AutoCancel });

				roomview.RoomUnits = _unitOfWork.RoomUnits.Get(
                    e => 
                    e.RoomViewId == roomViewId &&
                    !e.ReservationRooms.Any(
                            reservationRoom =>
                                    (reservationRoom.Reservation.ReservationStatus == ReservationStatus.Pending ||
                                    reservationRoom.Reservation.ReservationStatus == ReservationStatus.Confirmed) &&
                                    reservationRoom.Reservation.CheckInDate <= ReservationFormView.CheckOutDate &&
                                    reservationRoom.Reservation.CheckOutDate >= ReservationFormView.CheckInDate
                    )
                ).ToList();

                if(roomview.RoomUnits.Count < quantity) return RedirectToAction("Refund", "Payment", new { paymentIntentId, viewNavigation = ViewNavigation.AutoCancel });

				RoomViews.Add( roomview );

                if (roomViewSelectionsList.ContainsKey(roomViewId))
					return RedirectToAction("Refund", "Payment", new { paymentIntentId, viewNavigation = ViewNavigation.AutoCancel });
				else
                    roomViewSelectionsList[roomViewId] = quantity;
            }
            #endregion
            #region Add Reservation to DB and make reservations for the units

            _unitOfWork.Reservations.Add(reservarion);

            foreach ( var roomview in RoomViews)
            {
                var quantity = roomViewSelectionsList[roomview.RoomViewId];
                var roomUnitsList = roomview.RoomUnits.ToList();
                List<ReservationRoom> ReservationRooms = [];

                for ( var i = 0; i < quantity; i++ )
                {
                    var reservationroom = new ReservationRoom
                    {
                        ReservationId = reservarion.ReservationId,
                        RoomUnitId = roomUnitsList[i].RoomUnitId
                    };

                    ReservationRooms.Add( reservationroom );
                }

                _unitOfWork.ReservationRooms.AddRange( ReservationRooms );
            }
            #endregion

            return RedirectToAction("Success", "Payment");
        }
        #endregion       
    }
}

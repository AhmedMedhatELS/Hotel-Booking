using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Models.ViewModel;
using Newtonsoft.Json;
using Utility;

namespace Hotels_Booking.Areas.HotelManager.Controllers
{
    [Area("HotelManager")]
    [Authorize(Roles = "HotelManager, Admin")]
    public class ReservationController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork) : Controller
    {
        #region Start Up
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private int? HotelId;
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null) HotelId = user.HotelId;

            await next();
        }
        #endregion
        #region Get All The Reservations
        public IActionResult AllReservation()
        {
            #region Get Reservations

            var reservations = new List<Reservation>();

            if (User.IsInRole("Admin") && HotelId == null)
            {
                reservations = _unitOfWork.Reservations.Get(null).ToList();
            }
            else
            {
                reservations = _unitOfWork.Reservations.Get(
                e => e.ReservationRooms.Any(
                    roomunit =>
                        roomunit.RoomUnit.RoomView.Room.HotelId == HotelId)
                ).ToList();
            }            

            if (reservations.Count == 0) return View();
            #endregion
            #region Prepare View List
            var hotelreservationlist = new List<HotelReservationList>();
            
            foreach ( var reservation in reservations)
            {
                #region Get the Rest of the Reservation Info
                reservation.ReservationRooms = _unitOfWork.ReservationRooms.Get(
                    e => e.ReservationId == reservation.ReservationId,
                    roomunits => roomunits.RoomUnit,
                    roomview => roomview.RoomUnit.RoomView,
                    locationview => locationview.RoomUnit.RoomView.LocationView,
                    room => room.RoomUnit.RoomView.Room,
                    hotel => hotel.RoomUnit.RoomView.Room.Hotel
                    ).ToList();
                #endregion
                #region Prepare Reservaion view one by one
                var reservationview = new HotelReservationList 
                {
                    HotelId = HotelId,
                    HotelName = reservation.ReservationRooms.ToList()[0].RoomUnit.RoomView.Room.Hotel.HotelName,
                    CheckInDate = reservation.CheckInDate.ToString("yyyy-MM-dd"),
                    CheckOutDate = reservation.CheckOutDate.ToString("yyyy-MM-dd"),
                    Email = reservation.Email ?? "",
                    GuestName = reservation.Name ?? "",
                    PaymentId = reservation.PaymentId ?? "",
                    PaymentDate = reservation.PaymentDate?.ToString("yyyy-MM-dd") ?? "",
                    Phone = reservation.PhoneNumber ?? "",
                    Status = reservation.ReservationStatus.ToString(),
                    Total = reservation.TotalAmount,
                    Requests = reservation.SpecialRequests ?? "None",
                    ReservationId = reservation.ReservationId,
                    Nights = (reservation.CheckOutDate - reservation.CheckInDate).Days + 1,
                    NumRooms = reservation.ReservationRooms.Count,
                    Rooms = []
                };

                foreach(var reservationroom in reservation.ReservationRooms)
                {
                    var roomtype = $"{reservationroom.RoomUnit.RoomView.Room.Name} ({reservationroom.RoomUnit.RoomView.LocationView.Name})";

                    var room = reservationview.Rooms.FirstOrDefault(e => e.Name == roomtype);

                    if (room == null)
                        reservationview.Rooms.Add(new RoomViewModel { Name = roomtype, Quantity = 1 });
                    else
                        room.Quantity += 1;
                }

                hotelreservationlist.Add(reservationview);
                #endregion
            }
            #endregion

            ViewBag.ReservationData = JsonConvert.SerializeObject(hotelreservationlist);

            return View();
        }
        #endregion
        #region Confirm Reservaion
        [Authorize(Roles = "HotelManager")]
        public IActionResult ConfirmReservation(int id)
        {
            //get the reservation
            var reservation = _unitOfWork.Reservations.GetOne(e => e.ReservationId == id);
            //check if the reservation exists
            if (reservation == null) return NotFound();
            //change the status of the reservation
            reservation.ReservationStatus = ReservationStatus.Confirmed;
            //save changes into the DB
            _unitOfWork.Reservations.Update(reservation);
            //go to reservations List
            return RedirectToAction("AllReservation");
        }
        #endregion
        #region Cancel Reservation
        public IActionResult CancelReservation(int id)
        {
            //get the reservation
            var reservation = _unitOfWork.Reservations.GetOne(e => e.ReservationId == id);
            //check if the reservation exists
            if (reservation == null) return NotFound();
            //change the status of the reservation
            reservation.ReservationStatus = ReservationStatus.Canceled;
            //save changes into the DB
            _unitOfWork.Reservations.Update(reservation);
            //go to Refound Then will go to the Reservation List
            return RedirectToAction("Refund", "Payment", new { area = "Customer", paymentIntentId = reservation.PaymentId, viewNavigation = ViewNavigation.ManagerReservation });
        }

        public IActionResult UserCancel(int id)
        {
            //get the reservation
            var reservation = _unitOfWork.Reservations.GetOne(
                e =>
                    e.ReservationId == id &&
                    e.UserId == _userManager.GetUserId(User) &&
                    e.ReservationStatus == ReservationStatus.Pending);
            //check if the reservation exists
            if (reservation == null) return NotFound();
            //change the status of the reservation
            reservation.ReservationStatus = ReservationStatus.Canceled;
            //save changes into the DB
            _unitOfWork.Reservations.Update(reservation);
            //go to Refound Then will go to the Reservation List
            return RedirectToAction("Refund", "Payment", new { area = "Customer", paymentIntentId = reservation.PaymentId, viewNavigation = ViewNavigation.UserReservation });
        }
        #endregion
    }
}

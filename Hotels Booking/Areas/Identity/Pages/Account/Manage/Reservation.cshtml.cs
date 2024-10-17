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
using Models;
using Models.ViewModel;
using Newtonsoft.Json;
using Utility;

namespace Hotels_Booking.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ReservationModel(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork) : PageModel
    {
        #region Start Up
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public string ReservationData { get; set; }
        #endregion    

        public IActionResult OnGetAsync()
        {
            #region Get Reservations
            var userId = _userManager.GetUserId(User);

            var reservations = _unitOfWork.Reservations.Get(
                e => e.UserId == userId
                ).ToList();

            if (reservations.Count == 0) return Page();
            #endregion

            #region Prepare View List
            var hotelreservationlist = new List<HotelReservationList>();

            foreach (var reservation in reservations)
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
                    HotelId = reservation.ReservationRooms.ToList()[0].RoomUnit.RoomView.Room.Hotel.HotelId,
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

                foreach (var reservationroom in reservation.ReservationRooms)
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

            ReservationData = JsonConvert.SerializeObject(hotelreservationlist);

            return Page();
        }       
    }
}

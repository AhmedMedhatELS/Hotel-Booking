using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class HotelReservationList
    {
        public int? HotelId { get; set; } = null;
        public string HotelName { get; set; } = null!;
        public int ReservationId { get; set; }
        public string GuestName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string CheckInDate { get; set; } = null!;
        public string CheckOutDate { get; set; } = null!;
        public int Total { get; set; }
        public string PaymentId { get; set; } = null!;
        public string PaymentDate { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Requests { get; set; } = null!;
        public int Nights { get; set; }
        public int NumRooms { get; set; }
        public List<RoomViewModel> Rooms { get; set; } = [];
    }

    public class RoomViewModel
    {
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
    }
}

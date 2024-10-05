using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Models
{
    public class Reservation
    {
        #region Properities
        public int ReservationId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime? PaymentDate { get; set; } = null;
        public string? PaymentId { get; set; }
        public int TotalAmount { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SpecialRequests { get; set; }
        public ReservationStatus ReservationStatus { get; set; } = ReservationStatus.Booking;
        #endregion

        #region Foreign Keys
        public string UserId { get; set; } = null!;
        public int? CountryId { get; set; } = null;
        #endregion

        #region Relations
        public ApplicationUser User { get; set; } = null!;
        public Country Country { get; set; } = null!;
        public ICollection<ReservationRoom> ReservationRooms { get; set; } = [];
        #endregion
    }
}

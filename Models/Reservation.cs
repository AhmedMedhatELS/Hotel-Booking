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
        public DateTime PaymentDate { get; set; }
        public string? PaymentId { get; set; }
        public int TotalAmount { get; set; }
        public ReservationStatus ReservationStatus { get; set; } = ReservationStatus.Booking;
        #endregion

        #region Foreign Keys
        public string UserId { get; set; } = null!;
        #endregion

        #region Relations
        public ApplicationUser? User { get; set; }
        public ICollection<ReservationRoom> ReservationRooms { get; set; } = [];
        #endregion
    }
}

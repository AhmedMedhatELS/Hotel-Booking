using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserReview
    {
        #region Properties
        public int UserReviewId { get; set; }
        public string? Review { get; set; }
        public int Rating { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        #endregion

        #region Foreign Keys
        public int ReservationId { get; set; }
        public int HotelId { get; set; }
        #endregion

        #region Relations
        public Reservation Reservation { get; set; } = null!;
        public Hotel Hotel { get; set; } = null!;
        #endregion
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Models.ViewModel
{
    public class ReviewProfileView
    {
        public int ReviewId { get; set; }
        public string HotelName { get; set; } = null!;
        public int ReservationId {  get; set; }
        public string CheckInDate { get; set; } = null!;
        public string CheckOutDate { get; set;} = null!;
        public string CreatedAt { get; set; } = null!;
        public string? Name { get; set;}
        public string? Review { get; set; }
        public ReviewStatus Status { get; set; }
        public int Rating { get; set; }
    }
}

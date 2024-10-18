using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Models.ViewModel
{
    public class ReviewAdminView
    {
        public int ReviewId { get; set; }
        public ReviewStatus ReviewStatus { get; set; }
        public string Name { get; set; } = null!;
        public string Review { get; set; } = null!;
        public int Rating { get; set; }
        public string CreatedAt { get; set; } = null!;
        public string HotelName { get; set; } = null!;
        public string CheckInDate { get; set; } = null!;
        public string CheckOutDate { get; set; } = null!;
        public string? UserProfilePhoto { get; set; }
    }
}

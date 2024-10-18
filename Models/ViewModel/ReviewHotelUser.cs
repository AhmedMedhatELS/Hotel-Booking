using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class ReviewHotelUser
    {
        public string Name { get; set; } = null!;
        public string Review { get; set; } = null!;
        public int Rating { get; set; }
        public string CreatedAt { get; set; } = null!;
        public string? UserProfilePhoto { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class ReservationDetailsView
    {
        public string HotelName { get; set; } = null!;
        public string HotelAddress { get; set; } = null!;
        public string HotelLocation { get; set; } = null!;
        public int HotelStars { get; set; }
        public int NumberNights { get; set; }
        public int NumberRooms { get; set; }
        public List<string> Rooms { get; set; } = [];
        public GuestInformation GuestInformation { get; set; } = null!;
        public List<Country> Countries { get; set; } = [];
    }
}

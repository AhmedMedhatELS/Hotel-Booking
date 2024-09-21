using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class HotelHomeView
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; } = null!;
        public string HotelImage { get; set; } = null!;
        public string HotelState { get; set; } = null!;
        public decimal HotelRating { get; set; }
        public int HotelStartPrice { get; set; }
    }
}

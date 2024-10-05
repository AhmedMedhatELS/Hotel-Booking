using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class HotelDetailsUser
    {
        public Hotel? Hotel { get; set; }
        public HotelDetailsFiltrationn? HotelDetailsFiltrationn { get; set; }
        public List<LocationView> LocationViews { get; set; } = [];
        public List<RoomType> RoomTypes { get; set; } = [];
    }
}

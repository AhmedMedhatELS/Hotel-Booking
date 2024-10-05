using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class HotelDetailsFiltrationn
    {
        public HotelDetailsFiltrationn() { }
        public HotelDetailsFiltrationn(HotelDetailsFiltrationn hotelDetailsFiltrationn) 
        {
            CheckInDate = hotelDetailsFiltrationn.CheckInDate;
            CheckOutDate = hotelDetailsFiltrationn.CheckOutDate;
            RoomView = hotelDetailsFiltrationn.RoomView;
            RoomType = hotelDetailsFiltrationn.RoomType;
            HotelId = hotelDetailsFiltrationn.HotelId;
        }

        public DateTime? CheckInDate { get; set; } = null;
        public DateTime? CheckOutDate { get; set; } = null;
        public int RoomType { get; set; }
        public int RoomView { get; set; }
        public int HotelId { get; set; }
    }
}

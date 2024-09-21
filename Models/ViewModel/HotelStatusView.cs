using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Models.ViewModel
{
    public class HotelStatusView
    {
        public string HotelName { get; set; } = null!;
        public int HotelId { get; set; }
        public Status HotelStatus { get; set; }
        public int HotelFacilities { get; set; }
        public bool HotelMainImage { get; set; }
        public int HotelImages { get; set; }
        public int Rooms { get; set; }
        public bool RoomsMainImages { get; set; }
        public int RoomsTotalImageNumber { get; set; }
        public bool AllRoomsHave_5_Images { get; set; }
        public bool AllRoomsHaveView {  get; set; }
        public int TotalHotelRoomUnits { get; set; }
        public bool UnitAtLeast_10 {  get; set; }
    }
}

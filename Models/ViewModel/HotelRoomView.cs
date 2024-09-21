using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class HotelRoomView
    {
        public Room Room { get; set; } = null!;
        public List<Room> Rooms { get; set; } = [];
        public List<RoomType> RoomsTypes { get; set;} = [];
    }
}

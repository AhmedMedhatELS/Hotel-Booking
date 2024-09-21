using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class RoomUnitLocationView
    {
        public RoomView RoomView { get; set; } = null!;
        public List<Room> Rooms { get; set; } = [];
        public List<LocationView> Locations { get; set; } = [];
    }
}

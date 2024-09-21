using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class LocationView
    {
        #region Properities
        public int LocationViewId { get; set; }
        public string Name { get; set; } = null!;
        #endregion

        #region Relations
        public ICollection<RoomView> RoomViews { get; set; } = [];
        public ICollection<Room> Rooms { get; set; } = [];
        #endregion
    }
}

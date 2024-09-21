using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RoomUnit
    {
        #region Properities
        public int RoomUnitId { get; set; }
        #endregion

        #region Foreign Keys
        public int RoomViewId { get; set; }
        #endregion

        #region Relations
        public RoomView RoomView { get; set; } = null!;
        public ICollection<ReservationRoom> ReservationRoomst { get; set; } = [];
        #endregion
    }
}

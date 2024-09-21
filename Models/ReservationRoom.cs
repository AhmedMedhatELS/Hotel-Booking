using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ReservationRoom
    {
        #region Properities
        public int ReservationRoomId { get; set; }
        #endregion

        #region Foreign Keys
        public int ReservationId { get; set; }
        public int RoomUnitId { get; set; }
        #endregion

        #region Relations
        public Reservation? Reservation { get; set; }
        public RoomUnit? RoomUnit { get; set; }
        #endregion
    }
}

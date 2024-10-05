using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class ReservationFormView
    {
        public ReservationFormView() { }
        public ReservationFormView(ReservationFormView reservationFormView) 
        {
            RoomViewData = reservationFormView.RoomViewData;
            CheckInDate = reservationFormView.CheckInDate;
            CheckOutDate = reservationFormView.CheckOutDate;
            HotelId = reservationFormView.HotelId;
            Total = reservationFormView.Total;
        }
        public string RoomViewData { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int HotelId { get; set; }
        public int Total {  get; set; }
    }
}

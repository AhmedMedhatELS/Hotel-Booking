using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class HomeFiltrationView
    {
        public int StateId { get; set; }
        public DateTime? CheckInDate { get; set; } = null;
        public DateTime? CheckOutDate { get; set; } = null;
    }
}

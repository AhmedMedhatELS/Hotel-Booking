using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class ContactView
    {
        public Contact Contact { get; set; } = null!;
        public HotelRequest HotelRequest { get; set; } = null!;
        public List<State> States { get; set; } = [];
    }
}

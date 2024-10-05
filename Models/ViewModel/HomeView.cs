using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class HomeView
    {
        public HomeFiltrationView HomeFiltrationView { get; set; } = null!;
        public List<HotelHomeView> HotelHomeViews { get; set; } = [];
        public List<State> States { get; set; } = [];
    }
}

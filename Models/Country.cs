using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Country
    {
        #region Properities
        public int CountryId { get; set; }
        public string Name { get; set; } = null!;
        #endregion
        #region Relations
        public ICollection<State> State { get; set; } = [];
        public ICollection<Reservation> Reservations { get; set; } = [];
        #endregion
    }
}

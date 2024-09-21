using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class State
    {
        #region Properities
        public int StateId {  get; set; }
        public string Name { get; set; } = null!;
        #endregion
        #region Relations
        public int CountryId { get; set; }
        public Country Country { get; set; } = null!;
        #endregion
    }
}

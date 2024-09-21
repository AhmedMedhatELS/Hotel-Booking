using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RoomType
    {
        #region Properities
        [ValidateNever]
        public int RoomTypeId { get; set; }
        public string Name { get; set; } = null!;
        #endregion
        #region Relations
        [ValidateNever]
        public ICollection<Room> Rooms { get; set; } = [];
        #endregion
    }
}

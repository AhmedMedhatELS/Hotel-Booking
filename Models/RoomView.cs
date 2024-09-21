using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RoomView
    {
        #region Properities
        [ValidateNever]
        public int RoomViewId { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int NumberRooms { get; set; }
        #endregion

        #region Foreign Keys
        [Required]
        public int RoomId { get; set; }
        [Required]
        public int LocationViewId { get; set; }
        #endregion

        #region Relations
        [ValidateNever]
        public Room Room { get; set; } = null!;
        [ValidateNever]
        public LocationView LocationView { get; set; } = null!;
        [ValidateNever]
        public ICollection<RoomUnit> RoomUnits { get; set; } = [];
        #endregion
    }
}

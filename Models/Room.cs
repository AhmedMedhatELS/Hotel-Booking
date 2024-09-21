using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Room
    {
        #region Properities
        [ValidateNever]
        public int RoomID { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        [Range(1, int.MaxValue)]
        public int BedsNumber { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int MaxPersons { get; set; }
        [Required]
        [Range(5, int.MaxValue)]
        public int SquareMeter { get; set; }
        #endregion

        #region Foreign Keys
        [Required]
        public int RoomTypeId { get; set; }
        [ValidateNever]
        public int HotelId { get; set; }
        #endregion

        #region Relations
        [ValidateNever]
        public Hotel Hotel { get; set; } = null!;
        [ValidateNever]
        public RoomType RoomType { get; set; } = null!;
        [ValidateNever]
        public ICollection<RoomGallery> RoomGalleries { get; } = [];
        [ValidateNever]
        public ICollection<RoomView> RoomViews { get; set; } = [];
        [ValidateNever]
        public ICollection<LocationView> LocationViews { get; set; } = [];
        #endregion
    }
}

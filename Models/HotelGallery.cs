using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class HotelGallery
    {
        #region Properities
        [ValidateNever]
        public int HotelGalleryId {  get; set; }
        [ValidateNever]
        public string ImageName { get; set; } = null!;
        [ValidateNever]
        public bool IsMainImage { get; set; } = false;
        #endregion
        #region Relations
        [ValidateNever]
        public int HotelId { get; set; }
        [ValidateNever]
        public Hotel? Hotel { get; set; }
        #endregion
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Facility
    {
        #region Properities
        [ValidateNever]
        public int FacilityId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        #endregion
        #region Relations
        [ValidateNever]
        public int HotelId { get; set; }
        [ValidateNever]
        public Hotel? Hotel { get; set; }
        #endregion
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Models
{
    public class HotelRequest
    {
        #region Properities
        public int HotelRequestId { get; set; }
        [Required]
        public string HotelName { get; set; } = null!;
        [Required]
        [Range(3,7)]
        public int Stars { get; set; }
        [Required]
        public string LocationLink { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public string HotelHotLine { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string HotelContactEmail { get; set; } = null!;
        public string? Reply { get; set; }
        public Status Status { get; set; } = Status.Pending;
        [Required]
        public string Message { get; set; } = null!;
        #endregion
        #region Relations
        [Required]
        public int StateId { get; set; }
        [ValidateNever]
        public State State { get; set; } = null!;
        [ValidateNever]
        public string UserId { get; set; } = null!;
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; } = null!;
        #endregion
    }
}

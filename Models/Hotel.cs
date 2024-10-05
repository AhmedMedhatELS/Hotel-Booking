using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Models
{
    public class Hotel
    {
        #region Properities
        public int HotelId { get; set; }
        [Required]
        public string HotelName { get; set; } = null!;
        [Required]
        [Range(3, 7)]
        public int Stars { get; set; }
        [Required]
        public string LocationLink { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public string HotelHotLine { get; set; } = null!;
        [Required]
        public string HotelContactEmail { get; set; } = null!;
        [ValidateNever]
        public decimal Rating { get; set; }
        [ValidateNever]
        public Status Status { get; set; } = Status.InProgress;
        #endregion
        #region Foreign Keys
        [Required]
        public int StateId { get; set; }
        #endregion
        #region Relations
        [ValidateNever]
        public State State { get; set; } = null!;
        [ValidateNever]
        public ICollection<HotelGallery> HotelGalleries { get; } = [];
        [ValidateNever]
        public List<ApplicationUser> ApplicationUsers { get; set; } = [];
        [ValidateNever]
        public ICollection<Facility> Facilities { get; } = [];
        [ValidateNever]
        public ICollection<Room> Rooms { get; set; } = [];
        [ValidateNever]
        public ICollection<UserReview> UserReviews { get; set; } = [];
        #endregion
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class ApplicationUser : IdentityUser
    {
        #region Properities
        public string? FullName { get; set; }
        public string? ProfileImage {  get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        #endregion

        #region Foreign Keys
        public int? StateId { get; set; }
        public int? HotelId { get; set; }
        #endregion

        #region Relations
        public State? State { get; set; }
        public Hotel? Hotel { get; set; }
        public ICollection<Contact> Contacts { get; } = [];
        public ICollection<HotelRequest> HotelRequests { get; } = [];
        public ICollection<Reservation>? Reservations { get; set; }
        #endregion
    }
}

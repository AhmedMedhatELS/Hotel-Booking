using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class GuestInformation
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Email { get; set; }
        [ValidateNever]
        public int? CounteryId { get; set; } = null;
        [ValidateNever]
        public string? SpecialRequests { get; set; }
        [ValidateNever]
        public ReservationFormView ReservationFormView { get; set; } = null!;
    }
}

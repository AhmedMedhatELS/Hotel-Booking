using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Models.ViewModel
{
    public class HotelRequestView
    {
        [Required]
        public int RequestId { get; set; }
        [Required]
        public string Reply { get; set; } = null!;
        [Required]
        public Status status { get; set; }
        [ValidateNever]
        public List<HotelRequest> HotelRequests { get; set; } = [];
    }
}

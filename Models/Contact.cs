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
    public class Contact
    {
        #region Properities
        public int ContactId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Message { get; set; } = null!;
        public string? Reply { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        #endregion
        #region Relations
        [ValidateNever]
        public string UserId { get; set; } = null!;
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; } = null!;
        #endregion
    }
}

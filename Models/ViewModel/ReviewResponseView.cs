using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Models.ViewModel
{
    public class ReviewResponseView
    {
        [Required]
        public int ReviewId { get; set; }
        [Required]
        public string Response { get; set; } = null!;
        [Required]
        public ReviewStatus ReviewStatus { get; set; }
    }
}

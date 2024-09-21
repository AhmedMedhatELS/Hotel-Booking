using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class FacilitiesView
    {
        public Facility NewFacility { get; set; } = null!;
        [ValidateNever]
        public List<Facility> Facilities { get; set; } = [];
    }
}

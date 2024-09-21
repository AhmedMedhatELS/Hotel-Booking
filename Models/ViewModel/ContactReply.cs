using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class ContactReply
    {
        [Required]
        public int contactId {  get; set; }
        [Required]
        public string? Reply { get; set; }
        [ValidateNever]
        public List<Contact> Contacts { get; set; } = [];
    }
}

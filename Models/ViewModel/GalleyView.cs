using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class GalleyView
    {
        [ValidateNever]
        public IFormFile? MainImageFile { get; set; }
        [ValidateNever]
        public List<IFormFile>? ImageFiles { get; set; }
        [ValidateNever]
        public List<HotelGallery> HotelGalleryList { get; set;} = [];
        [ValidateNever]
        public string? MainImageName { get; set; }
        [ValidateNever]
        public List<Room> RoomList { get; set; } = [];
        [ValidateNever]
        public int RoomId { get; set; }
    }
}

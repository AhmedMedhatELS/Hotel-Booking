using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RoomGallery
    {
        #region Properities
        public int RoomGalleryId { get; set; }
        public string ImageName { get; set; } = null!;
        public bool IsMainImage { get; set; } = false;
        #endregion
        #region Foreign Keys
        public int RoomID { get; set; }
        #endregion
        #region Relations
        public Room Room { get; set; } = null!;
        #endregion
    }
}

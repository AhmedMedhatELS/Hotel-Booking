using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class HotelGalleryRepository(ApplicationDbContext context) : Repository<HotelGallery>(context), IHotelGalleryRepository
    {
        private readonly ApplicationDbContext context = context;

        /// for additional logic
    }
}

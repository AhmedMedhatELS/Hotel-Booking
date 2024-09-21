using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class HotelRepository(ApplicationDbContext context) : Repository<Hotel>(context), IHotelRepository
    {
        private readonly ApplicationDbContext context = context;

        /// for additional logic
    }
}

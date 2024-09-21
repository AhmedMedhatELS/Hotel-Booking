using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RoomUnitRepository(ApplicationDbContext context) : Repository<RoomUnit>(context), IRoomUnitRepository
    {
        private readonly ApplicationDbContext context = context;

        /// for additional logic
    }
}

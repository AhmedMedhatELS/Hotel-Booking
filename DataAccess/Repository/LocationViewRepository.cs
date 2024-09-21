using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class LocationViewRepository(ApplicationDbContext context) : Repository<LocationView>(context), ILocationViewRepository
    {
        private readonly ApplicationDbContext context = context;

        /// for additional logic
    }
}

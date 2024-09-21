using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DataAccess.Repository
{
    public class CountryRepository(ApplicationDbContext context) : Repository<Country>(context), ICountryRepository
    {
        private readonly ApplicationDbContext context = context;

        /// for additional logic
    }
}

using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ContactRepository(ApplicationDbContext context) : Repository<Contact>(context), IContactRepository
    {
        private readonly ApplicationDbContext context = context;

        /// for additional logic
    }
}

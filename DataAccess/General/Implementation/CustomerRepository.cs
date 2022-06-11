using DataAccess.General.Interface;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.General.Implementation
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Customer> GetCustomer(long id)
        {
            return await _context.Customers.Include(x => x.Otp).SingleOrDefaultAsync(xx => xx.Id == id);
        }
    }
}

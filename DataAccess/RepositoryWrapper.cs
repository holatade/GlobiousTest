using DataAccess.General.Implementation;
using DataAccess.General.Interface;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApplicationDbContext _context;
        public ICustomerRepository _customer;
        public IOtpRepository _otp;

        public RepositoryWrapper(ApplicationDbContext context)
        {
            _context = context;
        }

        public IOtpRepository Otp
        {
            get
            {
                if (_otp == null)
                {
                    _otp = new OtpRepository(_context);
                }
                return _otp;
            }
        }

        public ICustomerRepository Customer
        {
            get
            {
                if (_customer == null)
                {
                    _customer = new CustomerRepository(_context);
                }
                return _customer;
            }
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

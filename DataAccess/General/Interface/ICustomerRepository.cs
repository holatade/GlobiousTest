using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.General.Interface
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<Customer> GetCustomer(long id);
    }
}

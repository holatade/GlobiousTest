using DataAccess.General.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IRepositoryWrapper
    {
        ICustomerRepository Customer { get; }
        IOtpRepository Otp { get; }

        Task<int> Save();
    }
}

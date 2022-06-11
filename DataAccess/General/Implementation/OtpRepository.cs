using DataAccess.General.Interface;
using Domain;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.General.Implementation
{
    public class OtpRepository : BaseRepository<Otp>, IOtpRepository
    {
        public OtpRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Otp
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string OtpCode { get; set; } 
    }
}

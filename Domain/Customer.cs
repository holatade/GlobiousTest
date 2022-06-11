using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Customer
    {
        public long Id { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string State { get; set; }
        public string LGA { get; set; } 
        public bool Activated { get; set; }
        public Otp Otp { get; set; }
    }
}

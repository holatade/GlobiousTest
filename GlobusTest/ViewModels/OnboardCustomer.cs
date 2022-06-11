using System.ComponentModel.DataAnnotations;

namespace GlobusTest.ViewModels
{
    public class OnboardCustomer
    {
        [Required]
        public string Phonenumber { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string LGA { get; set; }
    }
}

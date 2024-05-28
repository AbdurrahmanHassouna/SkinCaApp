using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APIdemo.DTOs
{

    public class RegisterModel
    {
        [MaxLength(50), Required]
        public string FirstName { get; set; }
        [MaxLength(100), Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Address { get; set; }
        [EmailAddress, StringLength(128), Required]
        public string Email { get; set; }
        [PasswordPropertyText, StringLength(128), Required]
        public string Password { get; set; }
        public IFormFile? File { get; set; }
    }
}

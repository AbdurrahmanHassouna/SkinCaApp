using APIdemo.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APIdemo.DTOs
{

    public class RegisterModel
    {
        
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public Governorate Governorate { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Address { get; set; }
        [EmailAddress, StringLength(128)]
        public string Email { get; set; }
        [PasswordPropertyText, StringLength(128)]
        public string Password { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}

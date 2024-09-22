using SkinCaApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkinCaApp.DTOs
{
    public class ProfileDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        [Phone]
        public string PhoneNumber {  get; set; }
        public Governorate Governorate { get; set; }
        [RegularExpression(@"^-(?:\d{1,2}|1[0-7]\d|180)(\.\d{1,10})?|^\d{1,2}(\.\d{1,10})?$", ErrorMessage = "Invalid Latitude.")]
        public double? Latitude { get; set; }
        [RegularExpression(@"^-(?:\d{1,2}|1[0-7]\d|180)(\.\d{1,10})?|^\d{1,2}(\.\d{1,10})?$", ErrorMessage = "Invalid Longitude.")]
        public double? Longitude { get; set; }
        public IFormFile? ProfilePicture { get; set; }

    }
}

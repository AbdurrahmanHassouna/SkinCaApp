using SkinCaApp.Models;
using System.ComponentModel.DataAnnotations;

namespace SkinCaApp.DTOs
{
    public class ProfileResponseDto
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
        public string PhoneNumber { get; set; }
        [RegularExpression(@"^\d{1,10}(\.\d{1,7})?$", ErrorMessage = "Invalid Latitude.")]
        public double? Latitude { get; set; }
        [RegularExpression(@"^\d{1,3}(\.\d{1,10})?$", ErrorMessage = "Invalid Longitude.")]
        public double? Longitude { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}

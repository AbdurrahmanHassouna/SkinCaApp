using System.ComponentModel.DataAnnotations;

namespace APIdemo.DTOs
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
        public string? Country { get; set; }
        public string? Address { get; set; }
        [Phone]
        public string PhoneNumber {  get; set; }


    }
}

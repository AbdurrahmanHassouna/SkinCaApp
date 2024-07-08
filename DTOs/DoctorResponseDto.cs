using APIdemo.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APIdemo.DTOs
{
    public class DoctorResponseDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Experience { get; set; }
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal ClinicFees { get; set; }
        public string[] Services { get; set; }
        public string Specialization { get; set; }
        public ICollection<WorkingDayResponseDto> WorkingDays { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}

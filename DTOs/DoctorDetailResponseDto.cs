using System.ComponentModel.DataAnnotations;

namespace SkinCaApp.DTOs
{
    public class DoctorDetailResponseDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Experience { get; set; }
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double Rating { get; set; } = Math.Round(5 * Random.Shared.Next(90, 100) * 0.01, 2);
        [DataType(DataType.Currency)]
        public decimal ClinicFees { get; set; }
        public string[] Services { get; set; }
        public string Specialization { get; set; }
        public bool IsWorking { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public WorkingDayResponseDto? WorkingTime { get; set; }
        public List<WorkingDayResponseDto> ClinicSchedule { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}

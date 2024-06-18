using System.ComponentModel.DataAnnotations;

namespace APIdemo.DTOs
{
    public class DoctorDto
    {
        public string UserId { get; set; }
        public string Email {  get; set; }
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public int Experience { get; set; }
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [DataType(DataType.Currency)]
        public decimal ClinicFees { get; set; }
        public string Services { get; set; }
        public string Specialization { get; set; }
        public ICollection<DoctorWorkingDayDto> WorkingDays { get; set; }
    }
}

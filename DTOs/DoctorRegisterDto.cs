
using System.ComponentModel.DataAnnotations;

namespace SkinCaApp.DTOs
{
    public class DoctorRegisterDto:RegisterModel
    { 
       
        public int Experience { get; set; }
        [DataType(DataType.Currency)]
        public decimal ClinicFees { get; set; }
        public string Description { get; set; }
        public string[] Services { get; set; }
        public string Specialization { get; set; }
        public DoctorWorkingDayDto[] WorkingDays { get; set; }
    }
}


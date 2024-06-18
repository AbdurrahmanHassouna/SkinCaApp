using System.ComponentModel.DataAnnotations;

namespace APIdemo.Models
{
    public class DoctorWorkingDay
    {
        public int Id { get; set; }
        public int DoctorInfoId { get; set; }
        public DoctorInfo DoctorInfo { get; set; }
        public TimeSpan OpenAt { get; set; }
        public TimeSpan CloseAt { get; set; }
        public DayOfWeek Day { get; set; }
    }
}

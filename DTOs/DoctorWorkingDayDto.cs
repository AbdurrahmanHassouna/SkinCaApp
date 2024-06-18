using APIdemo.Models;

namespace APIdemo.DTOs
{
    public class DoctorWorkingDayDto
    {
        public TimeSpan OpenAt { get; set; }
        public TimeSpan CloseAt { get; set; }
        public DayOfWeek Day { get; set; }
    }
}

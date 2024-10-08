﻿using SkinCaApp.Models;

namespace SkinCaApp.DTOs
{
    public class DoctorWorkingDayDto
    {
        public TimeSpan OpenAt { get; set; }
        public TimeSpan CloseAt { get; set; }
        public DayOfWeek Day { get; set; }
    }
}

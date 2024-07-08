namespace APIdemo.DTOs
{
    public class WorkingDayResponseDto
    {
        public TimeSpan OpenAt { get; set; }
        public TimeSpan CloseAt { get; set; }
        public string Day { get; set; }
    }
}

using APIdemo.Models;

namespace APIdemo.DTOs
{
    public class ReminderDto
    {
        public int? Id { get; set; }    
        public string Name { get; set; }
        public int Amount { get; set; }
        public DateTime ExpiresAt { get; set; }
        public TimeSpan Alarm { get; set; }
        public BillType BillType { get; set; }
        
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace SkinCaApp.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public DateTime ExpiresAt { get; set; }
        public TimeSpan Alarm { get; set; }
        public BillType BillType { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [NotMapped]
        public bool IsExpired
        {
            get
            {
                return DateTime.Now.Subtract(ExpiresAt).TotalMilliseconds > 0;
            }
        }
    }
    public enum BillType
    {
        BEFORE = 0,
        IN,
        AFTER
    }
}

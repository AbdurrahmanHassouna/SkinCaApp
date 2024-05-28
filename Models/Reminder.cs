namespace APIdemo.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime Alarm { get; set; }
        public BillType BillType { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
    public enum BillType
    {
        BEFORE = 0,
        IN,
        AFTER

    }
}

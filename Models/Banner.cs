namespace SkinCaApp.Models
{
    public class Banner
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public byte[] Image { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
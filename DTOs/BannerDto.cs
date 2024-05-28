namespace APIdemo.Models
{
    public class BannerDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[]? Image { get; set; }
        public IFormFile File { get; set; }
    }
}
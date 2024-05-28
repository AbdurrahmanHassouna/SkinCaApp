using APIdemo.Models;

namespace APIdemo.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string? SenderId {  get; set; }
        public string? Content { get; set; }
        public byte[]? Image { get; set; }
        public MStatus Status { get; set; }
    }
}

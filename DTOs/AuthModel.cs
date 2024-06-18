using System.ComponentModel.DataAnnotations;

namespace APIdemo.DTOs
{
    public class AuthModel
    {
        public string? Id {  get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public bool IsAuthenticated { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public bool? IsAdmin { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresOn { get; set; }
    }
}

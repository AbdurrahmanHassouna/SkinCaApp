using NuGet.Protocol;
using System.ComponentModel.DataAnnotations;

namespace APIdemo.DTOs
{
    public class AuthModel
    {
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? UserName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public bool? IsDoctor { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresOn { get; set; }
        public byte[]? ProfilePicture {  get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace APIdemo.DTOs
{
    public class AuthAndLangModel
    {
        [Required]
        public string Lang { get; set; } = "en";
        [Required]
        public string Authorization { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace APIdemo.DTOs
{
    public class ChangePasswordModel
    {
        [Required]
        public string CurrentPassword { get; set; } = "";
        [Required]
        public string NewPassword { get; set; } = "";

    }
}

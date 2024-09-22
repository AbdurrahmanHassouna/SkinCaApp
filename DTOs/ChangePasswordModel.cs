using System.ComponentModel.DataAnnotations;

namespace SkinCaApp.DTOs
{
    public class ChangePasswordModel
    {
        [Required]
        public string CurrentPassword { get; set; } = "";
        [Required]
        public string NewPassword { get; set; } = "";

    }
}

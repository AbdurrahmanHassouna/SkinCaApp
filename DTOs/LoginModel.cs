using System.ComponentModel.DataAnnotations;

namespace APIdemo.DTOs
{
    public class LoginModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

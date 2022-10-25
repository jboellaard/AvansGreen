using System.ComponentModel.DataAnnotations;

namespace AvansGreen.WebApp.Models
{

    public class LoginViewModel
    {
        [Required]
        public string? Nr { get; set; }

        [Required]
        [UIHint("password")]
        public string? Password { get; set; }

        public string ReturnUrl { get; set; } = "/";

        public TypeOfUser TypeOfUser { get; set; }
    }

}

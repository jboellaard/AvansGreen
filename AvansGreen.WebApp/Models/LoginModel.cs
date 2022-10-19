using System.ComponentModel.DataAnnotations;

namespace AvansGreen.WebApp.Models
{

    public class LoginModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [UIHint("password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";

        public TypeOfUser UserType { get; set; }
    }

}

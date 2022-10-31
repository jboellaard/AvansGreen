using System.ComponentModel.DataAnnotations;

namespace UI.RESTfulAPIWebService.Models
{
    public class AuthenticationCredentials
    {
        [Required]
        public string Nr { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

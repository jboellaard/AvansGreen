using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UI.AvansGreenApp.Security
{
    public class AvansGreenUser : IdentityUser
    {
        [MaxLength(256)]
        public string FullName { get; set; } = "Avans Green User";

    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.AG_EF
{
    public class AvansGreenUser : IdentityUser
    {
        [MaxLength(256)]
        public string FullName { get; set; } = "Avans Green User";
    }
}

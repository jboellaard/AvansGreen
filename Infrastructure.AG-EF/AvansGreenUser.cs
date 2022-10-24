using Microsoft.AspNetCore.Identity;

namespace Infrastructure.AG_EF
{
    public class AvansGreenUser : IdentityUser
    {
        public int StudentId { get; set; }
        public int CanteenEmployeeId { get; set; }
    }
}

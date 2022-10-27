using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UI.AvansGreenApp.Security
{
    public class AuthDbContext : IdentityDbContext<AvansGreenUser>
    {

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

    }

}


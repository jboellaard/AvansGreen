using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UI_Infra.Security
{
    public class AuthDbContext : IdentityDbContext<AvansGreenUser>
    {

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

    }

}


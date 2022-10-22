using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.AG_EF
{
    public class AuthDbSeed
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthDbSeed(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task EnsurePopulated()
        {
            string adminEmail = "adminmail@avans.nl";
            string adminPassword = "Secret123$";

            string student1Email = "je.boellaard@student.avans.nl";
            string student1Password = "Secret123$";

            string student2Email = "em.degroot@student.avans.nl";
            string student2Password = "Secret123$";

            string student3Email = "b.dejong@student.avans.nl";
            string student3Password = "Secret123$";

            string student4Email = "d.li@student.avans.nl";
            string student4Password = "Secret123$";

            string canteenEmployeeEmail = "n.devries@avans.nl";
            string canteenEmployeePassword = "Secret123$";

            await AddUser(adminEmail, adminPassword, "Admin");
            await AddUser(student1Email, student1Password, "Student");
            await AddUser(student2Email, student2Password, "Student");
            await AddUser(student3Email, student3Password, "Student");
            await AddUser(student4Email, student4Password, "Student");
            await AddUser(canteenEmployeeEmail, canteenEmployeePassword, "CanteenEmployee");
        }

        public async Task AddUser(string email, string password, string claim)
        {
            string claimUserType = "UserType";
            IdentityUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser(email)
                {
                    Email = email,
                    EmailConfirmed = true
                };
                await _userManager.CreateAsync(user, password);
                await _userManager.AddClaimAsync(user, new Claim(claimUserType, claim));
            }
        }
    }
}

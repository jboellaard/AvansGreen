using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace UI.AvansGreenApp.Security
{
    public class AuthDbSeed
    {
        private readonly UserManager<AvansGreenUser> _userManager;

        public AuthDbSeed(UserManager<AvansGreenUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task EnsurePopulated()
        {
            string password = "Secret123$";

            string adminUniqueNr = "a0000000";
            string adminEmail = "adminmail@avans.nl";
            string adminFullName = "Admin";

            string student1UniqueNr = "s2182556";
            string student1Email = "je.boellaard@student.avans.nl";
            string student1FullName = "Joy Boellaard";

            string student2UniqueNr = "s2192233";
            string student2Email = "em.degroot@student.avans.nl";
            string student2FullName = "Emma de Groot";

            string student3UniqueNr = "s2192344";
            string student3Email = "b.dejong@student.avans.nl";
            string student3FullName = "Ben de Jong";

            string student4UniqueNr = "s2184399";
            string student4Email = "d.li@student.avans.nl";
            string student4FullName = "Diana Li";

            string canteenEmployee1UniqueNr = "e1234567";
            string canteenEmployee1FullName = "Naomi de Vries";

            string canteenEmployee2UniqueNr = "e2345678";
            string canteenEmployee2FullName = "Peter Smit";

            string canteenEmployee3UniqueNr = "e3456789";
            string canteenEmployee3FullName = "Lennart de Groot";

            await AddUser(adminUniqueNr, password, adminFullName, "Admin", adminEmail);
            await AddUser(student1UniqueNr, password, student1FullName, "Student", student1Email);
            await AddUser(student2UniqueNr, password, student2FullName, "Student", student2Email);
            await AddUser(student3UniqueNr, password, student3FullName, "Student", student3Email);
            await AddUser(student4UniqueNr, password, student4FullName, "Student", student4Email);
            await AddUser(canteenEmployee1UniqueNr, password, canteenEmployee1FullName, "CanteenEmployee");
            await AddUser(canteenEmployee2UniqueNr, password, canteenEmployee2FullName, "CanteenEmployee");
            await AddUser(canteenEmployee3UniqueNr, password, canteenEmployee3FullName, "CanteenEmployee");

        }

        public async Task AddUser(string uniqueId, string password, string fullName, string claim, string email = null)
        {
            string claimUserType = "UserType";
            AvansGreenUser user = await _userManager.FindByNameAsync(uniqueId);
            if (user == null)
            {
                user = new AvansGreenUser()
                {
                    UserName = uniqueId,
                    FullName = fullName
                };
                if (email != null)
                {
                    user.Email = email;
                    user.EmailConfirmed = true;
                }
                await _userManager.CreateAsync(user, password);
                await _userManager.AddClaimAsync(user, new Claim(claimUserType, claim));
            }
        }
    }
}

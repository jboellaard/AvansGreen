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

            //IdentityUser user = await _userManager.FindByEmailAsync(adminEmail);
            //if (user == null)
            //{
            //    user = new IdentityUser("Admin1")
            //    {
            //        Email = adminEmail,
            //        EmailConfirmed = true
            //    };
            //    await _userManager.CreateAsync(user, adminPassword);
            //    await _userManager.AddClaimAsync(user, new Claim(claimUserType, "Admin"));
            //}

            //IdentityUser student1 = await _userManager.FindByEmailAsync(student1Email);
            //if (student1 == null)
            //{
            //    student1 = new IdentityUser("Student1")
            //    {
            //        Email = student1Email,
            //        EmailConfirmed = true
            //    };
            //    await _userManager.CreateAsync(student1, student1Password);
            //    await _userManager.AddClaimAsync(student1, new Claim(claimUserType, "Student"));
            //}

            //IdentityUser student2 = await _userManager.FindByEmailAsync(student2Email);
            //if (student2 == null)
            //{
            //    student2 = new IdentityUser("Student2")
            //    {
            //        Email = student2Email,
            //        EmailConfirmed = true
            //    };
            //    await _userManager.CreateAsync(student2, student2Password);
            //    await _userManager.AddClaimAsync(student2, new Claim(claimUserType, "Student"));
            //}

            //IdentityUser student3 = await _userManager.FindByEmailAsync(student3Email);
            //if (student3 == null)
            //{
            //    student3 = new IdentityUser("Student3")
            //    {
            //        Email = student3Email,
            //        EmailConfirmed = true
            //    };
            //    await _userManager.CreateAsync(student3, student3Password);
            //    await _userManager.AddClaimAsync(student3, new Claim(claimUserType, "Student"));
            //}

            //IdentityUser student4 = await _userManager.FindByEmailAsync(student4Email);
            //if (student4 == null)
            //{
            //    student4 = new IdentityUser("Student4")
            //    {
            //        Email = student4Email,
            //        EmailConfirmed = true
            //    };
            //    await _userManager.CreateAsync(student4, student4Password);
            //    await _userManager.AddClaimAsync(student4, new Claim(claimUserType, "Student"));
            //}

            //IdentityUser canteenWorker = await _userManager.FindByEmailAsync(canteenEmployeeEmail);
            //if (canteenWorker == null)
            //{
            //    canteenWorker = new IdentityUser("CanteenWorker1")
            //    {
            //        Email = canteenEmployeeEmail,
            //        EmailConfirmed = true
            //    };
            //    await _userManager.CreateAsync(canteenWorker, canteenEmployeePassword);
            //    await _userManager.AddClaimAsync(canteenWorker, new Claim(claimUserType, "CanteenWorker"));
            //}

        }

        public async Task AddUser(string email, string password, string claim)
        {
            string claimUserType = "UserType";
            IdentityUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser(email.Split("@")[0])
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

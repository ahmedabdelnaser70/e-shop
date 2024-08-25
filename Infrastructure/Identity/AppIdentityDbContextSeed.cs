using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "bob Hop",
                    Email = "bob@test.com",
                    UserName = "bob@test.com",
                    Address = new Address
                    {
                        FirstName = "Awel",
                        LastName = "Makrm",
                        Street = "10 st",
                        City = "Cairo",
                        State = "Nasr City",
                        ZipCode = "102030"
                    }
                };
                await userManager.CreateAsync(user, "P@ssw0rd");
            }
        }
    }
}

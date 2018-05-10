using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SportStore.Api.Data
{
    public static class IdentityDataSeeder
    {
        private const string adminUser = "admin";
        private const string adminPassword = "MySecret123$";
        private const string adminRole = "Administrator";

        public static async void Seed(IdentityDataContext context,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.Migrate();

            var role = await roleManager.FindByNameAsync(adminRole);
            var user = await userManager.FindByNameAsync(adminUser);

            if (role == null)
            {
                role = new IdentityRole(adminRole);
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    throw new Exception("Cannot create role: " + 
                                        result.Errors.FirstOrDefault());
                }
            }

            if (user == null)
            {
                user = new IdentityUser(adminUser);
                var result = await userManager.CreateAsync(user, adminPassword);

                if (!result.Succeeded)
                {
                    throw new Exception("Cannot create user: " +
                                        result.Errors.FirstOrDefault());
                }
            }

            if (!await userManager.IsInRoleAsync(user, adminRole))
            {
                var result = await userManager.AddToRoleAsync(user, adminRole);

                if (!result.Succeeded)
                {
                    throw new Exception("Cannot add user to role: " +
                                        result.Errors.FirstOrDefault());
                }
            }
        }
    }
}
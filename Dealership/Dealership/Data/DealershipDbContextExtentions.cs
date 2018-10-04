namespace Dealership.Data
{
    using Dealership.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Data.SqlClient;

    public static class DealershipDbContextExtentions
    {
        public static void EnsureDbSeeded(
            DealershipDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            var roles = new[]
                       {
                            "Admin",
                            "Moderator"
                        };

            foreach (var role in roles)
            {
                var roleExists = roleManager.RoleExistsAsync(role).Result;

                if (!roleExists)
                {
                    roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role
                    }).Wait();
                }
            }

            var adminEmail = configuration.GetSection("AdminEmail").Value.ToString();
            var adminUser = userManager.FindByEmailAsync(adminEmail).Result;

            var admins = userManager.GetUsersInRoleAsync(configuration.GetSection("AdminRole").Value.ToString()).Result.Count;

            if (admins <= 0)
            {
                adminUser = new ApplicationUser
                {
                    Email = adminEmail,
                    UserName = adminEmail
                };

                userManager.CreateAsync(adminUser, configuration.GetSection("AdminPassword").Value.ToString()).Wait();
                userManager.AddToRoleAsync(adminUser, configuration.GetSection("AdminRole").Value.ToString()).Wait();
            }
        }

        public static bool DatabaseExists(IConfiguration configuration)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

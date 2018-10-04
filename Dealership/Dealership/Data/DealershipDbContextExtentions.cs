
namespace Dealership.Data
{
    using Dealership.Models;
    using Dealership.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    public static class DealershipDbContextExtentions
    {     
        public static void EnsureDbSeeded(this DealershipDbContext context,
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

            if (adminUser == null)
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
    
    }
}

namespace Dealership.Migrations
{
    using Dealership.Data;
    using Dealership.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public partial class SeedData : Migration
    {
        private static IApplicationBuilder _app;
        private static DealershipDbContext _context;
        private static UserManager<ApplicationUser> _userManager;
        private static RoleManager<IdentityRole> _roleManager;
        private static IConfiguration _configuration;
        private static string _adminEmail;

        public SeedData()
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var roles = new[]
                       {
                            "Admin",
                            "Moderator"
                        };

            foreach (var role in roles)
            {
                var roleExists = _roleManager.RoleExistsAsync(role).Result;

                if (!roleExists)
                {
                    _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role
                    }).Wait();
                }
            }

            var adminUser = _userManager.FindByEmailAsync(_adminEmail).Result;
            var admins = _userManager.GetUsersInRoleAsync(_configuration.GetSection("AdminRole").Value.ToString()).Result.Count;

            if (admins <= 0)
            {
                adminUser = new ApplicationUser
                {
                    Email = _adminEmail,
                    UserName = _adminEmail
                };

                _userManager.CreateAsync(adminUser, _configuration.GetSection("AdminPassword").Value.ToString()).Wait();
                _userManager.AddToRoleAsync(adminUser, _configuration.GetSection("AdminRole").Value.ToString()).Wait();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var adminUser = _userManager.FindByEmailAsync(_adminEmail).Result;
            var roles = _roleManager.Roles;

            _context.Users.Remove(adminUser);
            _context.Roles.RemoveRange(roles);
        }

        public void InitialDependencies(IApplicationBuilder app)
        {
            _app = app;

            var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            _context = scope.ServiceProvider.GetService<DealershipDbContext>();
            _userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            _roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            _configuration = scope.ServiceProvider.GetService<IConfiguration>();
            _adminEmail = _configuration.GetSection("AdminEmail").Value.ToString();
        }
    }
}
